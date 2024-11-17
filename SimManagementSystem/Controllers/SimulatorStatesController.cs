using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorStatesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public SimulatorStatesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetSimulatorStates()
        {
            var simulatorStates = _context.SimulatorStates
                .OrderByDescending(s => s.StartupTime)
                .Select(s => new
                {
                    s.Id,
                    startupTime = s.StartupTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    s.MeterState,
                    s.Operator
                })
                .ToList();
            return Ok(simulatorStates);
        }

        [HttpGet("difference")]
        public IActionResult GetSimulatorStateDifference(DateTime date1, DateTime date2)
        {
            if (date1 > date2)
            {
                return NotFound("Nie znaleziono odpowiednich stanów dla podanych dat.");
            }

            var firstState = _context.SimulatorStates
                .OrderBy(s => s.StartupTime)
                .Where(s => s.StartupTime.Date >= date1.Date)
                .Select(s => new
                {
                    s.StartupTime,
                    s.MeterState
                })
                .FirstOrDefault();

            var lastState = _context.SimulatorStates
                .OrderByDescending(s => s.StartupTime)
                .Where(s => s.StartupTime.Date <= date2.Date)
                .Select(s => new
                {
                    s.StartupTime,
                    s.MeterState
                })
                .FirstOrDefault();

            if (firstState != null && lastState != null)
            {
                int result = lastState.MeterState - firstState.MeterState;
                return Ok(result);
            }

            return NotFound("Nie znaleziono odpowiednich stanów dla podanych dat.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateSimulatorState(CreateSimulatorStateDTO createSimulatorStateDTO)
        {
            var simulatorState = new SimulatorState
            {
                StartupTime = createSimulatorStateDTO.StartupTime,
                MeterState = createSimulatorStateDTO.MeterState,
                Operator = createSimulatorStateDTO.Operator
            };

            await _context.SimulatorStates.AddAsync(simulatorState);
            await _context.SaveChangesAsync();

            return Ok(simulatorState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimulatorState(int id)
        {
            var simulatorStateToDelete = new SimulatorState { Id = id };
            _context.SimulatorStates.Attach(simulatorStateToDelete);
            _context.SimulatorStates.Remove(simulatorStateToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}