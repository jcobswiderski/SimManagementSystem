using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using System.Runtime.CompilerServices;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public UsersController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO newUser)
        {
            var user = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Login = newUser.Login,
                Password = newUser.Password
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userToDelete = new User {
                Id = id
            };

            _context.Users.Attach(userToDelete);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
