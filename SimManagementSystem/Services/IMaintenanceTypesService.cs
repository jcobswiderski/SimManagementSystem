using Microsoft.AspNetCore.Mvc;

namespace SimManagementSystem.Services
{
    public interface IMaintenanceTypesService
    {
        Task<IActionResult> GetMaintenanceTypes();
    }
}
