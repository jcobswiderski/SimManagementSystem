using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class SimulatorStatesService : ISimulatorStatesService
    {
        private readonly SimManagementSystemContext _context;

        public SimulatorStatesService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetSimulatorStates()
        {
            var simulatorStates = await _context.SimulatorStates
                .OrderByDescending(s => s.StartupTime)
                .Select(s => new
                {
                    s.Id,
                    startupTime = s.StartupTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    s.MeterState,
                    s.Operator
                })
                .ToListAsync();

            if (simulatorStates == null)
            {
                return new NotFoundObjectResult("Simulator states not found");
            }

            return new OkObjectResult(simulatorStates);
        }

        public async Task<IActionResult> GetSimulatorStateDifference(DateTime date1, DateTime date2)
        {
            if (date1 > date2)
            {
                return new NotFoundObjectResult("Date begin cannot be greater than date end.");
            }

            var firstState = await _context.SimulatorStates
                .OrderBy(s => s.StartupTime)
                .Where(s => s.StartupTime.Date >= date1.Date)
                .Select(s => new
                {
                    s.StartupTime,
                    s.MeterState
                })
                .FirstOrDefaultAsync();

            var lastState = await _context.SimulatorStates
                .OrderByDescending(s => s.StartupTime)
                .Where(s => s.StartupTime.Date <= date2.Date)
                .Select(s => new
                {
                    s.StartupTime,
                    s.MeterState
                })
                .FirstOrDefaultAsync();

            if (firstState != null && lastState != null)
            {
                int result = lastState.MeterState - firstState.MeterState;
                return new OkObjectResult(result);
            }

            return new NotFoundObjectResult("Simulator states not found for given dates.");
        }

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

            return new OkObjectResult(simulatorState);
        }

        public async Task<IActionResult> DeleteSimulatorState(int id)
        {
            var simulatorStateToDelete = new SimulatorState { Id = id };
            _context.SimulatorStates.Attach(simulatorStateToDelete);
            _context.SimulatorStates.Remove(simulatorStateToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }
    }
}
