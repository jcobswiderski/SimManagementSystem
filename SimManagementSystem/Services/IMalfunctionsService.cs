using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IMalfunctionsService
    {
        Task<IActionResult> GetMalfunctions();
        Task<IActionResult> GetMalfunctionsCount(DateTime? dateBegin, DateTime? dateEnd);
        Task<IActionResult> GetUnsolvedMalfunctionsCount();
        Task<IActionResult> GetMalfunction(int id);
        Task<IActionResult> GetMalfunctionForDevice(int deviceId);
        Task<IActionResult> CreateMalfunction(CreateMalfunctionDTO newMalfunction);
        Task<IActionResult> DeleteMalfunction(int id);
        Task<IActionResult> UpdateMalfunctionState(int id, [FromBody] EditMalfunctionDTO updatedUser);
    }
}
