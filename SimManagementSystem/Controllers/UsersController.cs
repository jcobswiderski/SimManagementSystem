using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace SimManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;
        private readonly IConfiguration _configuration;

        public UsersController(SimManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .Where(u => u.Id == id)
                .FirstOrDefault();
            return Ok(users);
        }

        [HttpGet("role/engineer")]
        public IActionResult GetEngineers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .Where(u => u.UserRoles.Any(r => r.Name == "Engineer"))
                .ToArray();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var user = _context.Users.Where(user => user.Login == loginRequest.Login).FirstOrDefault();

            if (user == null)
            {
                return Unauthorized();
            }

            string passwordHash = user.Password;
            string curHashedPassword = PasswordChecker.GetEncryptedPassword(loginRequest.Password, user.Salt);

            if (passwordHash != curHashedPassword)
            {
                return Unauthorized();
            }

            List<Claim> userClaims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("login", user.Login)
            };

            var roles = _context.Users
                .Where(u => u.Login == loginRequest.Login)
                .SelectMany(u => u.Roles)
                .Select(r => r.Name)
                .ToList();

            foreach (var role in roles)
            {
                userClaims.Add(new Claim("role", role));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            user.RefreshToken = RefreshTokenGenerator.GetRefreshToken();
            user.RefreshTokenExp = DateTime.UtcNow.AddDays(1);
            _context.SaveChanges();

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = user.RefreshToken
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO newUser)
        {
            var encryptedPassword = PasswordEncryptor.GetEncryptedPassword(newUser.Password);

            var user = new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Login = newUser.Login,
                Password = encryptedPassword.Item1,
                Salt = encryptedPassword.Item2,
                RefreshToken = RefreshTokenGenerator.GetRefreshToken(),
                RefreshTokenExp = DateTime.UtcNow.AddDays(1)
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = new User
            {
                Id = id
            };

            _context.Users.Attach(userToDelete);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult EditUser(int id, [FromBody] EditUserDTO updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;

            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("{id}/AssignRole")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            var role = _context.Roles.FirstOrDefault(r => r.Id == assignRoleDTO.Id);
            if (role == null)
                return NotFound("Role not found.");

            if (user.Roles.Contains(role))
                return BadRequest("User already has this role.");

            user.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok("Role assigned to user");
        }

        [HttpDelete("{id}/RemoveRole")]
        public async Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);

            if (user == null) return NotFound("User not found.");

            var role = _context.Roles.FirstOrDefault(r => r.Id == removeRoleDTO.Id);
            if (role == null)
                return NotFound("Role not found.");

            if (!user.Roles.Contains(role))
                return BadRequest("User does not have this role");

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok("Role sucessfuly removed");
        }
    }
}
