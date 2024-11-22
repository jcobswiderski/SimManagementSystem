using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        /// <summary>
        /// Get all available roles
        /// </summary>
        /// <returns>List of available roles</returns>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            return await _rolesService.GetRoles();
        }

        /// <summary>
        /// Asign role to user.
        /// </summary>
        /// <param name="id">Target user id</param>
        /// <param name="assignRoleDTO">Target role id</param>
        /// <returns></returns>
        [HttpPost("{id}/AssignRole")]
        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO)
        {
            return await _rolesService.AssignRole(id, assignRoleDTO);
        }

        /// <summary>
        /// Delete user role
        /// </summary>
        /// <param name="id">Target user id</param>
        /// <param name="removeRoleDTO">Target role id</param>
        /// <returns></returns>
        [HttpDelete("{id}/RemoveRole")]
        public async Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO)
        {
            return await _rolesService.RemoveRole(id, removeRoleDTO);
        }
    }
}
