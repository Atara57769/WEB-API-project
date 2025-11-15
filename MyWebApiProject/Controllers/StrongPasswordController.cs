using Entity;
using Microsoft.AspNetCore.Mvc;
using Service;


namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrongPasswordController : Controller
    {
        private readonly ISignUpService _service;

        public StrongPasswordController(ISignUpService service)
        {
            _service = service;
        }
        [HttpPost]
        public int StrongPassword([FromBody] User user)
        {

            return _service.StrongPassword(user);
        }
    }
}
