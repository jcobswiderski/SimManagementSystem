using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Services
{
    public class MaintenanceTypesService : IMaintenanceTypesService
    {
        private readonly SimManagementSystemContext _context;

        public MaintenanceTypesService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetMaintenanceTypes()
        {
            var maintenanceTypes = await _context.MaintenanceTypes.ToListAsync();

            if (maintenanceTypes == null)
            {
                return new NotFoundObjectResult("Maintenance types not found.");
            }

            return new OkObjectResult(maintenanceTypes);
        }
    }
}
