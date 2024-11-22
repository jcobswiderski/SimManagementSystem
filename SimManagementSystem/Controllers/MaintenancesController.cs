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
    public class MaintenancesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;
        private readonly IMaintenancesService _maintenancesService;

        public MaintenancesController(SimManagementSystemContext context, IMaintenancesService maintenancesService)
        {
            _context = context;
            _maintenancesService = maintenancesService;
        }

        /// <summary>
        /// Get list of maintenance objects. It is based on daily, weekly, monthly... maintenaces saved in the system.
        /// </summary>
        /// <returns>Maintenance objects list</returns>
        [HttpGet]
        public async Task<IActionResult> GetMaintenances()
        {
            return await _maintenancesService.GetMaintenances();
        }

        /// <summary>
        /// Endpoint for all Maintenances scheduled before current day, but not yet completed.
        /// </summary>
        /// <returns>List of incompleted Maintenances.</returns>
        [HttpGet("count/incomplete")]
        public async Task<IActionResult> GetIncompleteMaintenancesCount()
        {
            return await _maintenancesService.GetIncompleteMaintenancesCount();
        }
        /// <summary>
        /// Endpoint to get a single Maintenance
        /// </summary>
        /// <param name="id">Target maintenance id</param>
        /// <returns>Mainteneance object or NotFound</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaintenance(int id)
        {
            return await _maintenancesService.GetMaintenance(id);
        }

        /// <summary>
        /// Endpoint to add new maintenance to the system.
        /// </summary>
        /// <param name="newDevice">Single Maintenance object</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateMaintenance(CreateMaintenanceDTO newMaintenance)
        {
            return await _maintenancesService.CreateMaintenance(newMaintenance);
        }

        /// <summary>
        /// Deleting single maintenance.
        /// </summary>
        /// <param name="id">Id of target maintenance</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            return await _maintenancesService.DeleteMaintenance(id);
        }

        /// <summary>
        /// Changing maintenance state to completed.
        /// </summary>
        /// <param name="id">Target maintenance id</param>
        /// <param name="maintenanceToEdit">Maintenance executor data.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaintenanceState(int id, [FromBody] EditMaintenanceDTO maintenanceToEdit)
        {
            return await _maintenancesService.UpdateMaintenanceState(id, maintenanceToEdit);
        }
    }
}
