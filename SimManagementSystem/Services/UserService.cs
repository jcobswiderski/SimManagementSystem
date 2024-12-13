using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly SimManagementSystemContext _context;
        private readonly IConfiguration _configuration;

        public UserService(SimManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users
                .OrderBy(u => u.LastName)
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .ToListAsync();

            if (users == null)
            {
                return new NotFoundObjectResult("Users not found.");
            }

            return new OkObjectResult(users);
        }

        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    UserRoles = u.Roles,
                })
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundObjectResult("User with given ID not found.");
            }

            return new OkObjectResult(user);
        }

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

            if (users == null)
            {
                return new NotFoundObjectResult("Engineers not found.");
            }

            return new OkObjectResult(users);
        }

        public async Task<IActionResult> Login(DataTransferObjects.LoginRequest loginRequest)
        {
            var user = await _context.Users.Where(user => user.Login == loginRequest.Login).FirstOrDefaultAsync();

            if (user == null)
            {
                return new UnauthorizedObjectResult("User not found.");
            }

            string passwordHash = user.Password;
            string curHashedPassword = PasswordChecker.GetEncryptedPassword(loginRequest.Password, user.Salt);

            if (passwordHash != curHashedPassword)
            {
                return new UnauthorizedObjectResult("Passwords are different.");
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

            return new OkObjectResult(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = user.RefreshToken
            });
        }

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

            return new OkObjectResult(user);
        }

        public async Task<IActionResult> ResetPassword(int id)
        {
            var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundObjectResult("User with given ID not found.");
            }

            string tempPassword = RandomPasswordGenerator.GetRandomPassword();

            string tempHashedPassword = PasswordChecker.GetEncryptedPassword(tempPassword, user.Salt);
            user.Password = tempHashedPassword;

            await _context.SaveChangesAsync();

            return new OkObjectResult(new
            {
                tempPassword
            });
        }

        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO data)
        {
            var user = await _context.Users.Where(user => user.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return new NotFoundObjectResult("User with given ID not found.");
            }

            string passwordHash = user.Password;
            string curHashedPassword = PasswordChecker.GetEncryptedPassword(data.OldPassword, user.Salt);

            if (passwordHash != curHashedPassword)
            {
                return new UnauthorizedObjectResult("Passwords are different.");
            }

            string newHashedPassword = PasswordChecker.GetEncryptedPassword(data.NewPassword, user.Salt);
            user.Password = newHashedPassword;

            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var userToDelete = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);

            if (userToDelete == null)
            {
                return new NotFoundObjectResult("User with given ID not found.");
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> EditUser(int id, [FromBody] EditUserDTO updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return new NotFoundObjectResult("User with given ID not found.");
            }

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;

            await _context.SaveChangesAsync();

            return new OkObjectResult(user);
        }
    }
}
