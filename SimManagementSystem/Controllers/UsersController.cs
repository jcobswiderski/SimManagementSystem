using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
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
        public IActionResult GetStudents()
        {
            var users = _context.Users.ToList();
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
                new Claim(ClaimTypes.Name, user.Login)
            };

            var roles = _context.Users
                .Where(u => u.Login == loginRequest.Login)
                .SelectMany(u => u.Roles)
                .Select(r => r.Name)
                .ToList();

            foreach (var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
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
        public async Task<IActionResult> Delete(int id)
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
    }
}
