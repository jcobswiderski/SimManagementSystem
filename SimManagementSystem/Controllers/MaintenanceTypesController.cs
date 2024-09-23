using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetMaintenanceTypes()
        {
            var maintenanceTypes = _context.MaintenanceTypes.ToList();
            return Ok(maintenanceTypes);
        }
    }
}
