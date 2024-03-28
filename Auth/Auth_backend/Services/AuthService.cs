using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto;
using FLiu__Auth.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FLiu__Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppSettings _settings;

        private readonly IAuthRepositoy _repository;

        public AuthService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> settings, IAuthRepositoy authRepositoy)
        {
            _httpContextAccessor = httpContextAccessor;
            _settings = settings.Value;
            _repository = authRepositoy;
        }


        async public Task<ServDto> login(Credentials cd)
        {
            var repoDto = await _repository.login(cd);

            if (repoDto.StatusCode != "200")
            {
                return new ServDto
                {
                         status = false,
                         Message = "User does not exist",
                         StatusCode = repoDto.StatusCode

                };
            }
            else
            {
                JwtGenerator(repoDto.User);
                return new ServDto
                {
                    status = true,
                    Message = "Login Successful",
                    StatusCode = repoDto.StatusCode,
                    Identity = repoDto.Identity

                };
            }
        }

        public async Task<ServDto> logout(string token)
        {
            var email = ExtractEmailFromToken(token);

            
            
            var res = await _repository.logout(email);

            return new ServDto
            {
                status = true,
                StatusCode = "200",
                Message = "Successfully Logged Out"
            };
        }


        public async Task<ServDto> register(Register model)
        {
            var res = await _repository.register(model);
            if(res.StatusCode == "200")
            {
                return new ServDto
                {
                    status = true,
                    StatusCode = "200",
                    Message = res.Message

                };
            }
            else if(res.StatusCode == "401")
            {
                return new ServDto
                {
                    status = false,
                    StatusCode = "401",
                    Message = res.Message
                };
            }
            else
            {
                return new ServDto
                {
                    status = false,
                    StatusCode = "400",
                    Message = res.Message
                };
            }

        }

        public string ExtractEmailFromToken(string token)
        {
            // Create a token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Read the token
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                throw new ArgumentException("Invalid JWT token");
            }

            // Retrieve the email claim
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.Equals("email", StringComparison.OrdinalIgnoreCase));

            if (emailClaim == null)
            {
                throw new ArgumentException("Email claim not found in token");
            }

            return emailClaim.Value;
        }


        private dynamic JwtGenerator(User user)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(this._settings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Guid Id",Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Email,user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypToken = tokenHandler.WriteToken(token);

            httpContext.Response.Cookies.Append("token", encrypToken,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = false,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.Strict
                });

            return (new { token = encrypToken, username = user.Email });

        }
    }
}
