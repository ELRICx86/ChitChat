using FLiu__Auth.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using FLiu__Auth.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using FLiu__Auth.Models.Dto;

namespace FLiu__Auth.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        public readonly IAuthService authService;


        public Auth(IAuthService _atuhService)
        {
            authService = _atuhService;

            
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> login(Credentials cd)
        {
            var res = await authService.login(cd);

            //return   "Hello";

            if (res == null)
            {
                return NotFound("User Does not Exist");
            }
            else if (res.Message == "Invalid Email")
            {
                return Unauthorized(res);
            }
            else if (res.Message == "Invalid Password")
            {
                return Unauthorized(res);
            }
            else
            {
                //JwtGenerator(cd);
                return Ok(res);
            }

        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> register([FromBody] Register model)
        {
            var res = await authService.register(model);
            if (model.ConfirmPassword == model.Password)
            {
                return Ok(res);

            }
            else
            {
                return BadRequest("Password Dont Match");
            }

        }

        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> logout()
        {
            string token = HttpContext.Request.Cookies["token"];
            //return Ok(token);
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new ServDto{status = false, StatusCode="400", Message = "Token not found in cookie" });
            }
            HttpContext.Response.Cookies.Delete("token");
            var res = await authService.logout(token);

            return Ok(new ServDto { status = true, StatusCode = "200", Message = "Logout Successful" });
        }

    }
}
