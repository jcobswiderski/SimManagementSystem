using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceTypesController : ControllerBase
    {
        private readonly IMaintenanceTypesService _maintenanceTypesService;

        public MaintenanceTypesController(IMaintenanceTypesService maintenanceTypesService)
        {
            _maintenanceTypesService = maintenanceTypesService;
        }

        /// <summary>
        /// Get all maintenance types used in system.
        /// </summary>
        /// <returns>List of maintenance types.</returns>
        [HttpGet]
        public async Task<IActionResult> GetMaintenanceTypes()
        {
            return await _maintenanceTypesService.GetMaintenanceTypes();
        }
    }
}
