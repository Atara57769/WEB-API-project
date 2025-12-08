using Microsoft.AspNetCore.Mvc;
using Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private IPasswordService _passwordService;
        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }
        [HttpPost("PasswordScore")]
        public ActionResult<int> PasswordScore([FromBody] string password)
        {
           int strength = _passwordService.GetPasswordScore(password);
              return Ok(strength);
        }
    }
}
