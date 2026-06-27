using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpPost("PasswordScore")]
        [AllowAnonymous]
        public ActionResult<int> PasswordScore([FromBody] string password)
        {
            int strength = _passwordService.GetPasswordScore(password);
            return Ok(strength);
        }
    }
}
