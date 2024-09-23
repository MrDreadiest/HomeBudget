using HomeBudget.Api.Data;
using HomeBudget.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeBudget.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _dataContext.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);

            return user != null ? Ok(user) : BadRequest();
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser([FromBody] User user)
        {
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<List<User>>> UpdateUser([FromBody] User user)
        {
            var dbUser = await _dataContext.Users.FindAsync(user.Id);

            if (dbUser != null)
            {
                return NotFound();
            }

            dbUser.Email = user.Email;
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;

            await _dataContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var dbUser = await _dataContext.Users.FindAsync(id);

            if (dbUser != null)
            {
                return NotFound();
            }
            _dataContext.Users.Remove(dbUser);
            await _dataContext.SaveChangesAsync();

            return Ok();
        }
    }
}
