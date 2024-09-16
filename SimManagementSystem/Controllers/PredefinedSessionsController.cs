using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredefinedSessionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public PredefinedSessionsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetPredefinedSessions()
        {
            var predefinedSessions = _context.PredefinedSessions.ToList();
            return Ok(predefinedSessions);
        }

        [HttpGet("{id}")]
        public IActionResult GetPredefinedSession(int id)
        {
            var predefinedSession = _context.PredefinedSessions
                .Where(s => s.Id == id)
                .FirstOrDefault();
            return Ok(predefinedSession);
        }
    }
}
