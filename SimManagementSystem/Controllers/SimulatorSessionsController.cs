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
    public class SimulatorSessionsController : ControllerBase
    {
        private readonly ISimulatorSessionsService _sessionService;

        public SimulatorSessionsController(ISimulatorSessionsService simulatorSessionsService)
        {
            _sessionService = simulatorSessionsService;
        }

        /// <summary>
        /// Return all simulator sessions.
        /// </summary>
        /// <returns>List of simulator sessions</returns>
        [HttpGet]
        public async Task<IActionResult> GetSimulatorSessions()
        {
            return await _sessionService.GetSimulatorSessions();
        }

        /// <summary>
        /// Return all simulator sessions planned but not completed.
        /// </summary>
        /// <returns>List of planned simulator sessions</returns>
        [HttpGet("count/planned")]
        public async Task<IActionResult> GetPlannedSimulatorSessionsCount()
        {
            return await _sessionService.GetPlannedSimulatorSessionsCount();
        }

        /// <summary>
        /// Get statistics for sessions. Sessions need to be done and be in given dates scope.
        /// </summary>
        /// <param name="dateBegin">Date begin</param>
        /// <param name="dateEnd">Date end</param>
        /// <returns>Two values: Duration and Count</returns>
        [HttpGet("statistics")]
        public async Task<IActionResult> GetSimulatorSessionsStatistics(DateTime dateBegin, DateTime dateEnd)
        {
            return await _sessionService.GetSimulatorSessionsStatistics(dateBegin, dateEnd);
        }

        /// <summary>
        /// Checks if there is no conflict with an existing session.
        /// </summary>
        /// <param name="dateBegin">Date begin of added session</param>
        /// <param name="duration">Duration of new session</param>
        /// <returns>Boolean value</returns>
        [HttpGet("checkConflict")]
        public async Task<IActionResult> CheckSimSessionConflict(DateTime dateBegin, int duration)
        {
            return await _sessionService.CheckSimSessionConflict(dateBegin, duration);
        }

        /// <summary>
        /// Get single simulator session.
        /// </summary>
        /// <param name="id">Target simulator session id</param>
        /// <returns>Single simulator session object.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSimulatorSession(int id)
        {
            return await _sessionService.GetSimulatorSession(id);
        }

        /// <summary>
        /// Returns the session for a given day.
        /// </summary>
        /// <param name="date">Date</param>
        /// <returns>List of simulator sessions</returns>
        [HttpGet("byday/{date}")]
        public async Task<IActionResult> GetSimulatorSessionsByDay(DateTime date)
        {
            return await _sessionService.GetSimulatorSessionsByDay(date);
        }

        /// <summary>
        /// Get sessions for the selected user 
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <returns>List of simulator sessions</returns>
        [HttpGet("byuser/{userId}")]
        public async Task<IActionResult> GetSimulatorSessionsByUser(int userId)
        {
            return await _sessionService.GetSimulatorSessionsByUser(userId);
        }

        /// <summary>
        /// Get statistics for user and session as supervisor and trained.
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <returns>sessionsAsTrained, sessionsAsTrainedDuration, sessionsAsSupervisor, sessionsAsSupervisorDuration</returns>
        [HttpGet("byuser/{userId}/count")]
        public async Task<IActionResult> GetSimulatorSessionsByUserCount(int userId)
        {
            return await _sessionService.GetSimulatorSessionsByUserCount(userId);
        }

        /// <summary>
        /// Calculates and returns the time since the last completed session of each type.
        /// </summary>
        /// <param name="userId">Target user id</param>
        /// <returns>A summary of sessions names and time.</returns>
        [HttpGet("byuser/{userId}/last")]
        public async Task<IActionResult> GetLastSimulatorSessionsByUser(int userId)
        {
            return await _sessionService.GetLastSimulatorSessionsByUser(userId);
        }

        /// <summary>
        /// Add new simulator session
        /// </summary>
        /// <param name="newSession">Simulator session required data</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSession(CreateSimulatorSessionDTO newSession)
        {
            return await _sessionService.CreateSession(newSession);
        }

        /// <summary>
        /// Delete single session
        /// </summary>
        /// <param name="id">Target session id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSimulatorSession(int id)
        {
            return await _sessionService.DeleteSimulatorSession(id);
        }

        /// <summary>
        /// Updating simulator session state to complete.
        /// </summary>
        /// <param name="id">Target session id</param>
        /// <returns>Simulator session after update</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSimulatorSessionState(int id)
        {
            return await _sessionService.UpdateSimulatorSessionState(id);
        }
    }
}
