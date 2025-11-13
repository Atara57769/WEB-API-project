using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;
using System.Runtime.Intrinsics.X86;



namespace MyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        public SignUpService s = new();

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {

            User? u = new();
            u = s.SignUp(user);
            if (u == null)
                return BadRequest();
            return CreatedAtAction(nameof(Post), new { id = u?.UserId }, u);

        }
    }
}

