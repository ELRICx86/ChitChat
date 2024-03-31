using FLiu__Auth.Models.DTO_Message;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FLiu__Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateMessage : ControllerBase
    {
        [HttpPost]
        [Route("/message")]
        public IActionResult sendMessage([FromQuery] int from, [FromQuery] int to, [FromBody] Message message)
        {
             return Ok(message);
        }
        
    }
}
