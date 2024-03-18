using FLiu__Auth.Models;
using FLiu__Auth.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace FLiu__Auth.Repository
{
    public interface IAuthRepositoy
    {
        public Task<RepoDto> login(Credentials cd);
        
        public Task<RepoDto> logout(string email);
        
        public Task<RepoDto> register(Register rg);
    }
}
