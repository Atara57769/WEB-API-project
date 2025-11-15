using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiShop.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserServices _userService;

        public UsersController(IUserServices userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            IEnumerable<User> users = _userService.GetUser();
            if(users.Count() > 0)
                return Ok(users);
            return NoContent();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(int id)
        {
            User user = _userService.GetUsersById(id);
            if(user == null) 
                return NoContent();
            return Ok(user);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User newUser)
        {
            newUser=_userService.AddUser(newUser);
            if(newUser==null)
                return BadRequest("password");
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }


        [HttpPost("login")]
        public ActionResult<User> Login([FromBody] LoginUser loginUser)
        {
            User user=_userService.Login(loginUser);
            if (user == null)
                return NoContent();

            return Ok(user);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User updateUser)
        {
          bool sucsess = _userService.UpdateUser(id, updateUser);
            if (!sucsess)
                return BadRequest();
            return Ok();
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
