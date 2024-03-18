using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FLiu__Auth.Services
{
    public interface IAuthService
    {

         public Task<ServDto> login(Credentials cd);

         public Task<ServDto> register(Register model);

         public Task<ServDto> logout(string token);
    }
}
