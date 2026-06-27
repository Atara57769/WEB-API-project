using DTOs;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Security.Claims;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            IEnumerable<UserDTO> users = await _userService.GetUsers();
            if (users.Count() > 0)
                return Ok(users);
            return NoContent();
        }

        // GET api/<UsersController>/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (userIdClaim == null || userIdClaim != id.ToString())
            {
                return Forbid();
            }

            UserDTO user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] PostUserDTO newUser)
        {
            if (!await _userService.UserWithSameEmail(newUser.Email))
                return BadRequest("The email already exists. Please try again.");

            if (!_userService.IsPasswordStrong(newUser.Password))
                return BadRequest("The password is too weak. Please try again.");

            var result = await _userService.AddUser(newUser);

            if (result == null)
                return BadRequest();

            Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return CreatedAtAction(nameof(Get), new { id = result.User.Id }, result.User);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginUser)
        {
            var result = await _userService.Login(loginUser);

            if (result == null)
                return Unauthorized();

            Response.Cookies.Append("token", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            _logger.LogInformation(
                $"login attempted id:{result.User.Id} email:{result.User.Email} " +
                $"first name:{result.User.FirstName} last name:{result.User.LastName}"
            );

            return Ok(result.User);
        }
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostUserDTO updateUser)
        {
            if (!await _userService.UserWithSameEmail(updateUser.Email, updateUser.Id))
                return BadRequest("The email already exists. Please try again.");
            if (!_userService.IsPasswordStrong(updateUser.Password))
                return BadRequest("The password is too weak. Please try again.");
            await _userService.UpdateUser(id, updateUser);
            return NoContent();
       }
    }
}
