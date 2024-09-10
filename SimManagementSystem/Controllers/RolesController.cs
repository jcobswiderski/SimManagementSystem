using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetRoles()
        {
            var roles = _context.Roles.ToList();
            return Ok(roles);
        }
    }
}
