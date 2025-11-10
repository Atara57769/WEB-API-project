using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Model;
using System.Text.Json;



namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {

        [HttpPost]
        public IActionResult Post([FromBody] SignIn user1)
        {
            using (StreamReader reader = System.IO.File.OpenText("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\MyWebApiProject\\DataFile.txt"))
            {
                string? currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {
                    User user = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.userName == user1.userName1 && user.password == user1.password1)
                        return CreatedAtAction(nameof(Post), new { id = user.userId }, user);
                }
            }
            return Unauthorized();


        }
    }
}
