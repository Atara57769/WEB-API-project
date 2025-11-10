using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Model;
using System.Text.Json;

namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            if(user.firstName != "" && user.lastName != "" && user.password != "" && user.userName != "")
            { 
            int numberOfUsers = System.IO.File.ReadLines("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\DataFile.txt").Count();
            user.userId = numberOfUsers + 1;
            string userJson = JsonSerializer.Serialize(user);
            System.IO.File.AppendAllText("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\DataFile.txt", userJson + Environment.NewLine);
            return CreatedAtAction(nameof(Post), new { id = user.userId }, user);
            }
            return Unauthorized();
        }
    }
}

