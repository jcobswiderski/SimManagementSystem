using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredefinedSessionsController : ControllerBase
    {
        private readonly IPredefinedSessionsService _predefinedSessionsService;

        public PredefinedSessionsController(IPredefinedSessionsService predefinedSessionsService)
        {
            _predefinedSessionsService = predefinedSessionsService;
        }

        /// <summary>
        /// Get all predefined sessions.
        /// </summary>
        /// <returns>List of predefined sessions</returns>
        [HttpGet]
        public async Task<IActionResult> GetPredefinedSessions()
        {
           return await _predefinedSessionsService.GetPredefinedSessions();
        }

        /// <summary>
        /// Get single predefined session.
        /// </summary>
        /// <param name="id">Targer predefined session id</param>
        /// <returns>Single predefined session object</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredefinedSession(int id)
        {
            return await _predefinedSessionsService.GetPredefinedSession(id);
        }

        /// <summary>
        /// Add new predefined session
        /// </summary>
        /// <param name="newScheme">All data required to add new sim scheme session</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePredefinedSession(CreatePredefinedSessionDTO newScheme)
        {
            return await _predefinedSessionsService.CreatePredefinedSession(newScheme);
        }
    }
}
