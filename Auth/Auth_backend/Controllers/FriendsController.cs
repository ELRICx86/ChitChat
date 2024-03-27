using FLiu__Auth.Models;
using FLiu__Auth.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FLiu__Auth.Controllers
{
    [Authorize]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IFriendShipRepo _friendShipRepo;

        public FriendsController(IFriendShipRepo friendShipRepo)
        {
            _friendShipRepo = friendShipRepo;
        }
        [HttpGet]
        [Route("friend/getall/{id}")]
        public async Task<IActionResult> GetAllFriends(int id)
        {
            var res = await _friendShipRepo.GetAllFriends(id);
            return Ok(res);
        }

        [HttpGet]
        [Route("friend/get/{id}")]
        public async Task<IActionResult> GetFriendById(int id)
        {

            var res = await _friendShipRepo.GetFriendById(id);
            return Ok(res);

        }

        [HttpPost]
        [Route("friend/add")]
        public async Task<IActionResult> Add([FromQuery] int user, [FromQuery] int friend)
        {

            var res = await _friendShipRepo.Add(user,friend);
            return Ok(res);
        }

        [HttpGet]
        [Route("friend/request/{id}")]
        public async Task<IActionResult> request(int id)
        {

            var res = await _friendShipRepo.GetRequest(id);
            return Ok(res);
        }

        [HttpGet]
        [Route("friend/pendings/{id}")]
        public async Task<IActionResult> pendings(int id)
        {
            var res = await _friendShipRepo.GetAllPendings(id);
            return Ok(res);
        }

        [HttpGet]
        [Route("friend/ministatement/{id}")]
        public async Task<IActionResult> ministatement(int id)
        {
            var res = await _friendShipRepo.GetMiniStatement(id);
            return Ok(res);
        }

        [HttpDelete]
        [Route("friend/remove")]
        public async Task<IActionResult> Delete([FromQuery]int user, [FromQuery]int friend)
        {
            var res = await _friendShipRepo.Delete(user, friend);
            return Ok(res);
        }

        [HttpPost]
        [Route("friend/response")]
        public async Task<IActionResult> response([FromQuery] int user, [FromQuery] int friend, [FromQuery] string action)
        {
            var res = await _friendShipRepo.Response(user, friend, action); 

            return Ok(res);
        }
        

        
    }
}
