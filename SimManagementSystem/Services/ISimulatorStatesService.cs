using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface ISimulatorStatesService
    {
        Task<IActionResult> GetSimulatorStates();
        Task<IActionResult> GetSimulatorStateDifference(DateTime date1, DateTime date2);
        Task<IActionResult> CreateSimulatorState(CreateSimulatorStateDTO createSimulatorStateDTO);
        Task<IActionResult> DeleteSimulatorState(int id);
    }
}
