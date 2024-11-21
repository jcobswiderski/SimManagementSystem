using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IUserService
    {
        Task<IActionResult> GetUsers();
        Task<IActionResult> GetUser(int id);
        Task<IActionResult> GetEngineers();
        Task<IActionResult> Login(DataTransferObjects.LoginRequest loginRequest);
        Task<IActionResult> Register(CreateUserDTO newUser);
        Task<IActionResult> ResetPassword(int id);
        Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordDTO data);
        Task<IActionResult> DeleteUser(int id);
        Task<IActionResult> EditUser(int id, [FromBody] EditUserDTO updatedUser);
        //Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO);
        //Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO);
    }
}
