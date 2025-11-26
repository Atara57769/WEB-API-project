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
        private readonly ISignUpService _service;
        private readonly ISignUpService _passwordService;

        public SignUpController(ISignUpService service)
        {
            _service = service;
            _passwordService = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            // Validate password strength FIRST in controller
            int passwordScore = _passwordService.StrongPassword(user);
            if (passwordScore < 3)
                return BadRequest($"Password is too weak (score: {passwordScore}/4). Please choose a stronger password with a score of at least 3.");

            User? u = _service.SignUp(user);
            if (u == null)
                return BadRequest("Registration failed. Please ensure all required fields are filled.");
            return CreatedAtAction(nameof(Post), new { id = u.UserId }, u);

        }
    }
}

