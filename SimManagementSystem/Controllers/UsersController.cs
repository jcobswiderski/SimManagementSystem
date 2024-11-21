﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
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
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
            return Ok(users);
        }

        [HttpGet("role/engineer")]
        public async Task<IActionResult> GetEngineers()
        {
            var users = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .Where(u => u.UserRoles.Any(r => r.Name == "Engineer"))
                .ToArrayAsync();
            return Ok(users);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(DataTransferObjects.LoginRequest loginRequest)
        {
            var user = await _context.Users.Where(user => user.Login == loginRequest.Login).FirstOrDefaultAsync();

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
                new Claim("login", user.Login),
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName)
            };

            var roles = await _context.Users
                .Where(u => u.Login == loginRequest.Login)
                .SelectMany(u => u.Roles)
                .Select(r => r.Name)
                .ToListAsync();

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
            _context.SaveChangesAsync();

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

        [HttpPost("{id}/resetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            string tempPassword = RandomPasswordGenerator.GetRandomPassword();

            string tempHashedPassword = PasswordChecker.GetEncryptedPassword(tempPassword, user.Salt);
            user.Password = tempHashedPassword;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                tempPassword
            });
        }

        [HttpPost("{id}/changePassword")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO data)
        {
            var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            if (user == null) { return NotFound(); }

            string passwordHash = user.Password;
            string curHashedPassword = PasswordChecker.GetEncryptedPassword(data.OldPassword, user.Salt);

            if (passwordHash != curHashedPassword)
            {
                return Unauthorized();
            }

            string newHashedPassword = PasswordChecker.GetEncryptedPassword(data.NewPassword, user.Salt);
            user.Password = newHashedPassword;

            _context.SaveChangesAsync();

            return Ok();
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
        public async Task<IActionResult> EditUser(int id, [FromBody] EditUserDTO updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if (user == null)
            {
                return NotFound("User with given ID not found.");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;

            _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpPost("{id}/AssignRole")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return NotFound("User not found");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == assignRoleDTO.Id);
            if (role == null)
                return NotFound("Role not found.");

            if (user.Roles.Contains(role))
                return BadRequest("User already has this role.");

            user.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok("Role assigned to user.");
        }

        [HttpDelete("{id}/RemoveRole")]
        public async Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return NotFound("User not found.");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == removeRoleDTO.Id);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            if (!user.Roles.Contains(role))
            {
                return BadRequest("User does not have this role.");
            }

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok("Role sucessfuly removed");
        }
    }
}
