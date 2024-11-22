using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface ISimulatorSessionsService
    {
        Task<IActionResult> GetSimulatorSessions();
        Task<IActionResult> GetPlannedSimulatorSessionsCount();
        Task<IActionResult> GetSimulatorSessionsStatistics(DateTime dateBegin, DateTime dateEnd);
        Task<IActionResult> CheckSimSessionConflict(DateTime dateBegin, int duration);
        Task<IActionResult> GetSimulatorSession(int id);
        Task<IActionResult> GetSimulatorSessionsByDay(DateTime date);
        Task<IActionResult> GetSimulatorSessionsByUser(int userId);
        Task<IActionResult> GetSimulatorSessionsByUserCount(int userId);
        Task<IActionResult> GetLastSimulatorSessionsByUser(int userId);
        Task<IActionResult> CreateSession(CreateSimulatorSessionDTO newSession);
        Task<IActionResult> DeleteSimulatorSession(int id);
        Task<IActionResult> UpdateSimulatorSessionState(int id);
    }
}
