using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorStatesController : ControllerBase
    {
        private readonly ISimulatorStatesService _statesService;

        public SimulatorStatesController(SimManagementSystemContext context, ISimulatorStatesService simulatorStatesService)
        {
            _statesService = simulatorStatesService;
        }

        /// <summary>
        /// Return all simulator meter states.
        /// </summary>
        /// <returns>List of states</returns>
        [HttpGet]
        public async Task<IActionResult> GetSimulatorStates()
        {
            return await _statesService.GetSimulatorStates();
        }

        /// <summary>
        /// Get difference beetwen two states from time scope
        /// </summary>
        /// <param name="date1">Date begin</param>
        /// <param name="date2">Date end</param>
        /// <returns>Value difference</returns>
        [HttpGet("difference")]
        public async Task<IActionResult> GetSimulatorStateDifference(DateTime date1, DateTime date2)
        {
            return await _statesService.GetSimulatorStateDifference(date1, date2);
        }

        /// <summary>
        /// Add new meter state.
        /// </summary>
        /// <param name="createSimulatorStateDTO">Meter data required to add new state.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateSimulatorState(CreateSimulatorStateDTO createSimulatorStateDTO)
        {
            return await _statesService.CreateSimulatorState(createSimulatorStateDTO);
        }

        /// <summary>
        /// Delete single meter state.
        /// </summary>
        /// <param name="id">Target meter state</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimulatorState(int id)
        {
            return await _statesService.DeleteSimulatorState(id);
        }
    }
}