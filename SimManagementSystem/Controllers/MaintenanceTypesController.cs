using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTypesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public MaintenanceTypesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMaintenanceTypes()
        {
            var maintenanceTypes = await _context.MaintenanceTypes.ToListAsync();

            if (maintenanceTypes == null)
            {
                return NotFound("Maintenance types not found.");
            }

            return Ok(maintenanceTypes);
        }
    }
}
