using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace WebApiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _userService;

        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            IEnumerable<User> users = _userService.GetUsers();
            if (users.Count() > 0)
                return Ok(users);
            return NotFound();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user = _userService.GetUserById(id);
            if(user == null)
                return NotFound();
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User newUser)
        {
            newUser = _userService.AddUser(newUser);
            if (newUser == null)
                return BadRequest("password");
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }

        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginUser loginUser)
        {
            User user = _userService.Login(loginUser);
            if (user == null)
                return Unauthorized();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User updateUser)
        {
            bool success = _userService.UpdateUser(id, updateUser);
            if (!success)
                return BadRequest();
            return NoContent();
        }
    }
}
