using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimManagementSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get all users from the system.
        /// </summary>
        /// <returns>List of User objects or NotFound</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return await _userService.GetUsers();
        }

        /// <summary>
        /// Get single user.
        /// </summary>
        /// <param name="id">Single user id.</param>
        /// <returns>User object or NotFound</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            return await _userService.GetUser(id);
        }

        /// <summary>
        /// Getting all users with role Engineer.
        /// </summary>
        /// <returns>List of User objects related with role Engineer.</returns>
        [HttpGet("role/engineer")]
        public async Task<IActionResult> GetEngineers()
        {
            return await _userService.GetEngineers(); // ZAMIENIĆ NA SUPERVISORS
        }

        /// <summary>
        /// Login to the system. Checking if form data is correct.
        /// </summary>
        /// <param name="loginRequest">Login and password.</param>
        /// <returns>Token and Refresh token for the user if data is correct, else returns Unauthorized.</returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(DataTransferObjects.LoginRequest loginRequest)
        {
            return await _userService.Login(loginRequest);
        }

        /// <summary>
        /// Register new user.
        /// </summary>
        /// <param name="newUser">User data required to register: firstName, lastName, login, password.</param>
        /// <returns>New user data.</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO newUser)
        {
            return await _userService.Register(newUser);
        }

        /// <summary>
        /// Reseting password. Function available only for Admin.
        /// </summary>
        /// <param name="id">Id of the target user.</param>
        /// <returns>Temporary password for user.</returns>
        [HttpPost("{id}/resetPassword")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            return await _userService.ResetPassword(id);
        }

        /// <summary>
        /// Changing current password by User.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <param name="data">Old and new password.</param>
        /// <returns>User if successfull, Unauthorized if failed.</returns>
        [HttpPost("{id}/changePassword")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO data)
        {
            return await _userService.ChangePassword(id, data);
        }

        /// <summary>
        /// Deleting user.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _userService.DeleteUser(id);
        }

        /// <summary>
        /// Edit user firstName and lastName.
        /// </summary>
        /// <param name="id">Target user id.</param>
        /// <param name="updatedUser">New firstName and lastName.</param>
        /// <returns>If wrong id - NotFound else returns target User.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, [FromBody] EditUserDTO updatedUser)
        {
            return await _userService.EditUser(id, updatedUser);
        }

    }
}
