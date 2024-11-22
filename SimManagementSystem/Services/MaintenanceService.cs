using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class MaintenanceService : IMaintenancesService
    {
        private readonly SimManagementSystemContext _context;

        public MaintenanceService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetMaintenances()
        {
            var maintenances = await _context.Maintenances
                .OrderByDescending(m => m.Date)
                .Select(m => new
                {
                    m.Id,
                    name = m.TypeNavigation.Name,
                    executor = m.ExecutorNavigation.FirstName + " " + m.ExecutorNavigation.LastName,
                    date = m.Date.ToString("yyyy-MM-dd"),
                    m.Realized
                })
                .ToListAsync();

            if (maintenances == null)
            {
                return new NotFoundObjectResult("Maintenances not found.");
            }

            return new OkObjectResult(maintenances);
        }

        public async Task<IActionResult> GetIncompleteMaintenancesCount()
        {
            var maintenancesCount = await _context.Maintenances
                .Where(m => m.Date <= DateTime.Now && m.Realized == false)
                .CountAsync();

            return new OkObjectResult(maintenancesCount);
        }

        public async Task<IActionResult> GetMaintenance(int id)
        {
            var maintenance = await _context.Maintenances
                .Select(m => new
                {
                    m.Id,
                    name = m.TypeNavigation.Name,
                    executor = m.ExecutorNavigation.FirstName + " " + m.ExecutorNavigation.LastName,
                    date = m.Date.ToString("yyyy-MM-dd"),
                    m.Realized,
                    tasks = m.TypeNavigation.Tasks
                })
                .Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            if (maintenance == null)
            {
                return new NotFoundObjectResult("Maintenance with given ID not found.");
            }

            return new OkObjectResult(maintenance);
        }

        public async Task<IActionResult> CreateMaintenance(CreateMaintenanceDTO newMaintenance)
        {
            var maintenance = new Maintenance
            {
                Type = newMaintenance.Type,
                Date = newMaintenance.Date,
                Realized = false
            };

            await _context.Maintenances.AddAsync(maintenance);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Maintenance successfully added.");
        }

        public async Task<IActionResult> DeleteMaintenance(int id)
        {
            var maintenanceToDelete = await _context.Maintenances.FirstOrDefaultAsync(m => m.Id == id);

            if (maintenanceToDelete == null)
            {
                return new NotFoundObjectResult("Maintenance with given ID not found.");
            }

            _context.Maintenances.Remove(maintenanceToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> UpdateMaintenanceState(int id, [FromBody] EditMaintenanceDTO maintenanceToEdit)
        {
            var maintenance = await _context.Maintenances.FirstOrDefaultAsync(m => m.Id == id);
            if (maintenance == null)
            {
                return new NotFoundObjectResult("Maintenance with given ID not found.");
            }

            maintenance.Realized = true;
            maintenance.Executor = maintenanceToEdit.Executor;

            await _context.SaveChangesAsync();

            return new OkObjectResult(maintenance);
        }
    }
}
