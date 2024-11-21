using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

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
        public async Task<IActionResult> GetPredefinedSessions()
        {
            var predefinedSessions = await _context.PredefinedSessions.ToListAsync();
            return Ok(predefinedSessions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPredefinedSession(int id)
        {
            var predefinedSession = _context.PredefinedSessions
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();
            return Ok(predefinedSession);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePredefinedSession(CreatePredefinedSessionDTO newScheme)
        {
            var predefinedSession = new PredefinedSession
            {   
                Category = newScheme.Category,
                Name = newScheme.Name,
                Description = newScheme.Description,
                Duration = newScheme.Duration,
                Abbreviation = newScheme.Abbreviation
            };

            await _context.PredefinedSessions.AddAsync(predefinedSession);
            await _context.SaveChangesAsync();

            return Ok(predefinedSession);
        }
    }
}
