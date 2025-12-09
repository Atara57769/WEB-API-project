using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Entity;
using Service;


namespace MyWebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : ControllerBase
    {
        private readonly IUpdateService _service;
        private readonly ISignUpService _passwordService;

        public UpdateController(IUpdateService service, ISignUpService passwordService)
        {
            _service = service;
            _passwordService = passwordService;
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            // Validate password strength FIRST in controller
            int passwordScore = _passwordService.StrongPassword(user);
            if (passwordScore < 3)
                return BadRequest($"Password is too weak (score: {passwordScore}/4). Please choose a stronger password with a score of at least 3.");

            bool? u = await _service.Update(user);
            if (u == null)
                return BadRequest("Update failed.");
            if (u == true)
                return NoContent();
            return NotFound("User not found.");
        }
    }
}
