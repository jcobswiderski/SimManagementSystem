using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class RolesService : IRolesService
    {
        private readonly SimManagementSystemContext _context;

        public RolesService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if (roles == null || !roles.Any())
            {
                return new NotFoundObjectResult("Roles not found.");
            }

            return new OkObjectResult(roles);
        }

        public async Task<IActionResult> AssignRole(int id, [FromBody] AssignRoleDTO assignRoleDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return new NotFoundObjectResult("User not found");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == assignRoleDTO.Id);
            if (role == null)
                return new NotFoundObjectResult("Role not found.");

            if (user.Roles.Contains(role))
                return new BadRequestObjectResult("User already has this role.");

            user.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Role assigned to user.");
        }

        public async Task<IActionResult> RemoveRole(int id, [FromBody] RemoveRoleDTO removeRoleDTO)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return new NotFoundObjectResult("User not found.");

            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == removeRoleDTO.Id);

            if (role == null)
            {
                return new NotFoundObjectResult("Role not found.");
            }

            if (!user.Roles.Contains(role))
            {
                return new BadRequestObjectResult("User does not have this role.");
            }

            user.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Role sucessfuly removed");
        }
    }
}
