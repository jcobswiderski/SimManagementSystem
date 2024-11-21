﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SimulatorSessionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public SimulatorSessionsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSimulatorSessions()
        {
            var simulatorSession = await _context.SimulatorSessions
                .OrderByDescending(m => m.BeginDate)
                .Select(s => new
                {
                    s.Id,
                    Category = s.PredefinedSessionNavigation.CategoryNavigation.Name,
                    Name = s.PredefinedSessionNavigation.Name,
                    Abbreviation = s.PredefinedSessionNavigation.Abbreviation,
                    Description = s.PredefinedSessionNavigation.Description,
                    Duration = s.PredefinedSessionNavigation.Duration,
                    BeginDate = s.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = s.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    s.Realized,
                    PilotName = s.PilotSeatNavigation.FirstName + " " + s.PilotSeatNavigation.LastName,
                    CopilotName = s.CopilotSeatNavigation.FirstName + " " + s.CopilotSeatNavigation.LastName,
                    ObserverName = s.ObserverSeatNavigation.FirstName + " " + s.ObserverSeatNavigation.LastName,
                    SupervisorName = s.SupervisorSeatNavigation.FirstName + " " + s.SupervisorSeatNavigation.LastName,
                })
                .ToListAsync();

            if (simulatorSession == null)
            {
                return NotFound("Simulator session not found.");
            }

            return Ok(simulatorSession);
        }

        [HttpGet("count/planned")]
        public async Task<IActionResult> GetPlannedSimulatorSessionsCount()
        {
            var simulatorSessionCount = await _context.SimulatorSessions
                .Where(s => s.Realized == false)
                .CountAsync();

            return Ok(simulatorSessionCount);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetSimulatorSessionsStatistics(DateTime dateBegin, DateTime dateEnd)
        {
            var simulatorSessions = await _context.SimulatorSessions
                .Where(s => s.BeginDate.Date >= dateBegin.Date && s.EndDate.Date <= dateEnd.Date && s.Realized == true)
                .Select(s => new
                {
                    s.Id,
                    Duration = s.PredefinedSessionNavigation.Duration,
                    s.Realized,
                })
                .ToListAsync();

            if (simulatorSessions == null)
            {
                return NotFound("Simulator sessions not found.");
            }

            var totalDuration = simulatorSessions.Sum(s => s.Duration);
            var count = simulatorSessions.Count;

            var result = new { 
                Duration = totalDuration,
                Count = count
            };

            return Ok(result);
        }

        [HttpGet("checkConflict")]
        public async Task<IActionResult> CheckSimSessionConflict(DateTime dateBegin, int duration)
        {
            var dateEnd = dateBegin.AddMinutes(duration);

            var conflictingSessions = await _context.SimulatorSessions
                .Where(s => s.BeginDate < dateEnd && s.EndDate > dateBegin)
                .CountAsync();

            return Ok(conflictingSessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSimulatorSession(int id)
        {
            var simulatorSession = await _context.SimulatorSessions
                .Where(s => s.Id == id)
                .Select(s => new
                {
                    s.Id,
                    Category = s.PredefinedSessionNavigation.CategoryNavigation.Name,
                    Name = s.PredefinedSessionNavigation.Name,
                    Abbreviation = s.PredefinedSessionNavigation.Abbreviation,
                    Description = s.PredefinedSessionNavigation.Description,
                    Duration = s.PredefinedSessionNavigation.Duration,
                    BeginDate = s.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = s.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    s.Realized,
                    PilotName = s.PilotSeatNavigation.FirstName + " " + s.PilotSeatNavigation.LastName,
                    CopilotName = s.CopilotSeatNavigation.FirstName + " " + s.CopilotSeatNavigation.LastName,
                    ObserverName = s.ObserverSeatNavigation.FirstName + " " + s.ObserverSeatNavigation.LastName,
                    SupervisorName = s.SupervisorSeatNavigation.FirstName + " " + s.SupervisorSeatNavigation.LastName,
                })
                .FirstOrDefaultAsync();

            if (simulatorSession == null)
            {
                return NotFound("Simulator session with given ID not found.");
            }

            return Ok(simulatorSession);
        }

        [HttpGet("byday/{date}")]
        public async Task<IActionResult> GetSimulatorSessionsByDay(DateTime date)
        {
            var sessions = await _context.SimulatorSessions
                .OrderByDescending(m => m.BeginDate)
                .Include(s => s.PredefinedSessionNavigation)
                .Include(s => s.PilotSeatNavigation)
                .Include(s => s.CopilotSeatNavigation)
                .Include(s => s.ObserverSeatNavigation)
                .Include(s => s.SupervisorSeatNavigation)
                .Where(s => s.BeginDate.Date == date.Date)
                .Select(s => new
                {
                    s.Id,
                    BeginDate = s.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = s.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    PilotName = s.PilotSeatNavigation.FirstName + " " + s.PilotSeatNavigation.LastName,
                    CopilotName = s.CopilotSeatNavigation.FirstName + " " + s.CopilotSeatNavigation.LastName,
                    ObserverName = s.ObserverSeatNavigation.FirstName + " " + s.ObserverSeatNavigation.LastName,
                    SupervisorName = s.SupervisorSeatNavigation.FirstName + " " + s.SupervisorSeatNavigation.LastName,
                    PredefinedSessionName = s.PredefinedSessionNavigation.Name,
                    PredefinedSessionDescription = s.PredefinedSessionNavigation.Description,
                    PredefinedSessionAbbreviation = s.PredefinedSessionNavigation.Abbreviation,
                    PredefinedSessionDuration = s.PredefinedSessionNavigation.Duration
                })
                .ToListAsync();

            if (sessions == null || !sessions.Any())
            {
                return NotFound("No sessions found for the given date.");
            }

            return Ok(sessions);
        }

        [HttpGet("byuser/{userId}")]
        public async Task<IActionResult> GetSimulatorSessionsByUser(int userId)
        {
            var sessions = await _context.SimulatorSessions
                .OrderByDescending(m => m.BeginDate)
                .Include(s => s.PredefinedSessionNavigation)
                .Include(s => s.PilotSeatNavigation)
                .Include(s => s.CopilotSeatNavigation)
                .Include(s => s.ObserverSeatNavigation)
                .Include(s => s.SupervisorSeatNavigation)
                .Where(s => s.PilotSeatNavigation.Id == userId ||
                        s.SupervisorSeatNavigation.Id == userId ||
                        s.CopilotSeatNavigation.Id == userId ||
                        s.ObserverSeatNavigation.Id == userId)
                .Select(s => new
                {
                    s.Id,
                    BeginDate = s.BeginDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    EndDate = s.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Pilot = s.PilotSeat,
                    Copilot = s.CopilotSeat,
                    Observer = s.ObserverSeat,
                    Supervisor = s.SupervisorSeat,
                    Name = s.PredefinedSessionNavigation.Name,
                    Abbreviation = s.PredefinedSessionNavigation.Abbreviation,
                    s.Realized
                })
                .ToListAsync();

            if (sessions == null || !sessions.Any())
            {
                return NotFound("No sessions found for the given user.");
            }

            return Ok(sessions);
        }

        [HttpGet("byuser/{userId}/count")]
        public async Task<IActionResult> GetSimulatorSessionsByUserCount(int userId)
        {
            var sessionsAsTrained = await _context.SimulatorSessions
                .Where(s => (s.PilotSeatNavigation.Id == userId || s.CopilotSeatNavigation.Id == userId) && s.Realized == true)
                .CountAsync();

            var sessionsAsTrainedDuration = await _context.SimulatorSessions
                .Where(s => (s.PilotSeatNavigation.Id == userId || s.CopilotSeatNavigation.Id == userId) && s.Realized == true)
                .SumAsync(s => s.PredefinedSessionNavigation.Duration);

            var sessionsAsSupervisor = await _context.SimulatorSessions
                .Where(s => (s.SupervisorSeatNavigation.Id == userId) && s.Realized == true)
                .CountAsync();

            var sessionsAsSupervisorDuration = await _context.SimulatorSessions
               .Where(s => (s.SupervisorSeatNavigation.Id == userId) && s.Realized == true)
               .SumAsync(s => s.PredefinedSessionNavigation.Duration);

            var result = new
            {
                sessionsAsTrained = sessionsAsTrained,
                sessionsAsTrainedDuration = sessionsAsTrainedDuration,
                sessionsAsSupervisor = sessionsAsSupervisor,
                sessionsAsSupervisorDuration = sessionsAsSupervisorDuration
            };

            return Ok(result);
        }

        [HttpGet("byuser/{userId}/last")]
        public async Task<IActionResult> GetLastSimulatorSessionsByUser(int userId)
        {
            var sessions = await _context.SimulatorSessions
                .Where(s => s.Realized == true && (s.PilotSeatNavigation.Id == userId || s.CopilotSeatNavigation.Id == userId))
                .Include(s => s.PredefinedSessionNavigation)
                .GroupBy(s => s.PredefinedSessionNavigation.Name)
                .Select(g => new
                {
                    Session = g.Key,
                    DaysSinceLastSession = g.Max(s => EF.Functions.DateDiffDay(s.BeginDate, DateTime.Now))
                })
                .ToListAsync();

            if (sessions == null)
            {
                return NotFound("Last simulator sessions for given user not found.");
            }

            return Ok(sessions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSimulatorSessionDTO newSession)
        {
            var predefinedSession = await _context.PredefinedSessions
                .Where(s => s.Id == newSession.PredefinedSession)
                .FirstOrDefaultAsync();

            if (predefinedSession == null)
            {
                return NotFound("Predefined session not found.");
            }

            int duration = predefinedSession.Duration;
            DateTime tempEndDate = newSession.BeginDate.AddMinutes(duration);

            var session = new SimulatorSession
            {
                PredefinedSession = newSession.PredefinedSession,
                BeginDate = newSession.BeginDate,
                EndDate = tempEndDate,
                PilotSeat = newSession.PilotSeat,
                CopilotSeat = newSession.CopilotSeat,
                SupervisorSeat = newSession.SupervisorSeat,
                ObserverSeat = newSession.ObserverSeat,
                Realized = newSession.Realized
            };

            await _context.SimulatorSessions.AddAsync(session);
            await _context.SaveChangesAsync();

            return Ok();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSimulatorSessionState(int id)
        {
            var simSession = await _context.SimulatorSessions.FirstOrDefaultAsync(m => m.Id == id);
            if (simSession == null)
            {
                return NotFound("Simulator session not found");
            }

            simSession.Realized = true;
            await _context.SaveChangesAsync();

            return Ok(simSession);
        }
    }
}
