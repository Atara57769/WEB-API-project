using DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> Get()
        {
            IEnumerable<UserDTO> users = await _userService.GetUsers();
            if (users.Count() > 0)
                return Ok(users);
            return NoContent();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            UserDTO user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<PostUserDTO>> Post([FromBody] PostUserDTO newUser)
        {
            if (!await _userService.UserWithSameEmail(newUser.Email))
                return BadRequest("The email already exists. Please try again.");
            if (!_userService.IsPasswordStrong(newUser.Password))
                return BadRequest("The password is too weak. Please try again.");
            newUser = await _userService.AddUser(newUser);
            if (newUser == null)
                return BadRequest();
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] LoginUserDTO loginUser)
        {
            UserDTO user = await _userService.Login(loginUser);
            if (user == null)
                return Unauthorized();
            _logger.LogInformation($"login attempted id:{user.Id} email:{user.Email} first name:{user.FirstName} last name:{user.LastName}");
            return Ok(user);
        }

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
