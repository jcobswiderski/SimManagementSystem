using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IRecoveryActionsService
    {
        Task<IActionResult> GetRecoveryActionsForMalfunction(int malfunctionId);
        Task<IActionResult> CreateRecoveryAction(CreateRecoveryActionDTO newRecoveryAction);
        Task<IActionResult> DeleteRecoveryAction(int id);
    }
}
