using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;
using System.Security.Cryptography.X509Certificates;




namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        private readonly ISignInService _service;

        public SignInController(ISignInService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SignIn user1)
        {
             User? u = new();
             u = _service.SignIn(user1);
            if (u == null)
                return BadRequest();
            return Ok(u);
            
        }
    }
}
