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
        public async Task<IActionResult> Post([FromBody] SignIn user1)
        {
            User? u = await _service.SignIn(user1);
            if (u == null)
                return Unauthorized("Invalid email or password.");
            return Ok(u);
        }
    }
}
