using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface IPredefinedSessionsService
    {
        Task<IActionResult> GetPredefinedSessions();
        Task<IActionResult> GetPredefinedSession(int id);
        Task<IActionResult> CreatePredefinedSession(CreatePredefinedSessionDTO newScheme);
    }
}
