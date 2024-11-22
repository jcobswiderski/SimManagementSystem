using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;
using System.Threading.Tasks;

namespace SimManagementSystem.Services
{
    public interface IRolesService
    {
        Task<IActionResult> GetRoles();
        Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO);
        Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO);
    }
}
