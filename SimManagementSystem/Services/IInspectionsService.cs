using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IInspectionsService
    {
        Task<IActionResult> GetInspections();
        Task<IActionResult> GetInspection(int id);
        Task<IActionResult> CreateInspection(CreateInspectionDTO newInspection);
        Task<IActionResult> DeleteInspection(int id);
        Task<IActionResult> GetInspectionTypes();
    }
}
