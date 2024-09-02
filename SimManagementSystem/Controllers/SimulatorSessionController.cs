using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorSessionController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public SimulatorSessionController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetSimulatorSession(int id)
        {
            var simulatorSession = _context.SimulatorSessions
                .Include(s => s.PredefinedSessionNavigation)
                .Include(s => s.PilotSeatNavigation)
                .Include(s => s.CopilotSeatNavigation)
                .Include(s => s.ObserverSeatNavigation)
                .Include(s => s.SupervisorSeatNavigation)
                .Where(s => s.Id == id)
                .Select(s => new
                    {
                        s.Id,
                        s.Date,
                        PilotName = s.PilotSeatNavigation.FirstName + " " + s.PilotSeatNavigation.LastName,
                        CopilotName = s.CopilotSeatNavigation.FirstName + " " + s.CopilotSeatNavigation.LastName,
                        ObserverName = s.ObserverSeatNavigation.FirstName + " " + s.ObserverSeatNavigation.LastName,
                        SupervisorName = s.SupervisorSeatNavigation.FirstName + " " + s.SupervisorSeatNavigation.LastName,
                        PredefinedSessionName = s.PredefinedSessionNavigation.Name,
                        PredefinedSessionDescription = s.PredefinedSessionNavigation.Description,
                        PredefinedSessionAbbreviation = s.PredefinedSessionNavigation.Abbreviation,
                        PredefinedSessionDuration = s.PredefinedSessionNavigation.Duration
                })
                .FirstOrDefault();

            return Ok(simulatorSession);    
        }

        [HttpGet("bydate/{date}")]
        public IActionResult GetSimulatorSessionsByDate(DateTime date)
        {
            var sessions = _context.SimulatorSessions
                .Include(s => s.PredefinedSessionNavigation)
                .Include(s => s.PilotSeatNavigation)
                .Include(s => s.CopilotSeatNavigation)
                .Include(s => s.ObserverSeatNavigation)
                .Include(s => s.SupervisorSeatNavigation)
                .Where(s => s.Date.Date == date.Date)
                .Select(s => new
                {
                    s.Id,
                    s.Date,
                    PilotName = s.PilotSeatNavigation.FirstName + " " + s.PilotSeatNavigation.LastName,
                    CopilotName = s.CopilotSeatNavigation.FirstName + " " + s.CopilotSeatNavigation.LastName,
                    ObserverName = s.ObserverSeatNavigation.FirstName + " " + s.ObserverSeatNavigation.LastName,
                    SupervisorName = s.SupervisorSeatNavigation.FirstName + " " + s.SupervisorSeatNavigation.LastName,
                    PredefinedSessionName = s.PredefinedSessionNavigation.Name,
                    PredefinedSessionDescription = s.PredefinedSessionNavigation.Description,
                    PredefinedSessionAbbreviation = s.PredefinedSessionNavigation.Abbreviation,
                    PredefinedSessionDuration = s.PredefinedSessionNavigation.Duration
                })
                .ToList();

            if (sessions == null || !sessions.Any())
            {
                return NotFound($"No sessions found for the date {date.ToShortDateString()}.");
            }

            return Ok(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSimulatorSessionDTO newSession)
        {
            var session = new SimulatorSession
            {
                PredefinedSession = newSession.PredefinedSession,
                Date = newSession.Date,
                PilotSeat = newSession.PilotSeat,
                CopilotSeat = newSession.CopilotSeat,
                SupervisorSeat = newSession.SupervisorSeat,
                ObserverSeat = newSession.ObserverSeat,
                Realized = newSession.Realized
            };

            await _context.SimulatorSessions.AddAsync(session);
            await _context.SaveChangesAsync();

            return Ok(session);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimulatorSession(int id)
        {
            var sessionToDelete = new SimulatorSession
            {
                Id = id
            };

            _context.SimulatorSessions.Attach(sessionToDelete);
            _context.SimulatorSessions.Remove(sessionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
