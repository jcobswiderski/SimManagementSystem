using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public RolesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if (roles == null)
            {
                return NotFound("Roles not found.");
            }

            return Ok(roles);
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
