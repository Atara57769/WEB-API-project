using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyProject.Model;
using System.Text.Json;

namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        [HttpPut]
        public IActionResult Put([FromBody] User user)
        {
            string textToReplace = string.Empty;
            using (StreamReader reader = System.IO.File.OpenText("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\MyWebApiProject\\DataFile.txt"))
            {
                string currentUserInFile;
                while ((currentUserInFile = reader.ReadLine()) != null)
                {

                    User user1 = JsonSerializer.Deserialize<User>(currentUserInFile);
                    if (user.userId == user1.userId)
                        textToReplace = currentUserInFile;
                }
            }

            if (textToReplace != string.Empty)
            {
                string text = System.IO.File.ReadAllText("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\MyWebApiProject\\DataFile.txt");
                text = text.Replace(textToReplace, JsonSerializer.Serialize(user));
                System.IO.File.WriteAllText("D:\\WEB Api\\MyWebApiProject\\MyWebApiProject\\MyWebApiProject\\DataFile.txt", text);
                return Ok(user);
            }
            return NotFound();
        }
    }
}
