using Entity;
using Microsoft.AspNetCore.Mvc;
using Service;


namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrongPasswordController : Controller
    {
        public SignUpService s = new();
        [HttpPost]
        public int StrongPassword([FromBody] User user)
        {

            return s.StrongPassword(user);
        }
    }
}
