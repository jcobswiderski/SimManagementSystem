using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IMaintenancesService
    {
        Task<IActionResult> GetMaintenances();
        Task<IActionResult> GetIncompleteMaintenancesCount();
        Task<IActionResult> GetMaintenance(int id);
        Task<IActionResult> CreateMaintenance(CreateMaintenanceDTO newMaintenance);
        Task<IActionResult> DeleteMaintenance(int id);
        Task<IActionResult> UpdateMaintenanceState(int id, [FromBody] EditMaintenanceDTO maintenanceToEdit);

    }
}
