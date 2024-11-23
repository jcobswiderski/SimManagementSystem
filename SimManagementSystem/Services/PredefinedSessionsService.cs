using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class PredefinedSessionsService : IPredefinedSessionsService
    {
        private readonly SimManagementSystemContext _context;

        public PredefinedSessionsService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetPredefinedSessions()
        {
            var predefinedSessions = await _context.PredefinedSessions.ToListAsync();

            if (predefinedSessions == null || !predefinedSessions.Any())
            {
                return new NotFoundObjectResult("Predefined session not found.");
            }

            return new OkObjectResult(predefinedSessions);
        }

        public async Task<IActionResult> GetPredefinedSession(int id)
        {
            var predefinedSession = _context.PredefinedSessions
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (predefinedSession == null)
            {
                return new NotFoundObjectResult("Predefined session with given ID not found.");
            }

            return new OkObjectResult(predefinedSession);
        }

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

            return new OkObjectResult(predefinedSession);
        }
    }
}
