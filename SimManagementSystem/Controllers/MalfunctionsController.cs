using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MalfunctionsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public MalfunctionsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetMalfunctions()
        {
            var malfunctions = _context.Malfunctions
                .OrderByDescending(m => m.DateBegin)
                .Select(m => new 
                { 
                    m.Id,
                    m.Name,
                    m.Description,
                    userReporter = m.UserReporterNavigation.FirstName + " " + m.UserReporterNavigation.LastName,
                    userHandler = m.UserHandlerNavigation.FirstName + " " + m.UserHandlerNavigation.LastName,
                    dateBegin = m.DateBegin.ToString("yyyy-MM-dd HH:mm:ss"),
                    dateEnd = m.DateEnd != null ? m.DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    m.Status
                })
                .ToList();
            return Ok(malfunctions);
        }

        [HttpGet("{id}")]
        public IActionResult GetMalfunction(int id)
        {
            var malfunctions = _context.Malfunctions
                .OrderByDescending(m => m.DateBegin)
                .Select(m => new
                {
                    m.Id,
                    m.Name,
                    m.Description,
                    userReporter = m.UserReporterNavigation.FirstName + " " + m.UserReporterNavigation.LastName,
                    userHandler = m.UserHandlerNavigation.FirstName + " " + m.UserHandlerNavigation.LastName,
                    dateBegin = m.DateBegin.ToString("yyyy-MM-dd HH:mm:ss"),
                    dateEnd = m.DateEnd != null ? m.DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss") : "",
                    m.Status,
                    devices = string.Join(", ", m.Devices.Select(d => d.Name))
                })
                .Where(m => m.Id == id)
                .FirstOrDefault();
            return Ok(malfunctions);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMalfunction(CreateMalfunctionDTO newMalfunction)
        {
            var devices = await _context.Devices
                .Where(d => newMalfunction.Devices.Contains(d.Id))
                .ToListAsync();

            if (devices == null || devices.Count != newMalfunction.Devices.Count)
            {
                return BadRequest("Niektóre urządzenia nie istnieją.");
            }

            var malfunction = new Malfunction
            {
                Name = newMalfunction.Name,
                Description = newMalfunction.Description,
                UserReporter = newMalfunction.UserReporter,
                UserHandler = newMalfunction.UserHandler,
                DateBegin = newMalfunction.DateBegin,
                Status = newMalfunction.Status,
                Devices = devices
            };

            await _context.Malfunctions.AddAsync(malfunction);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMalfunction(int id)
        {
            var malfunctionToDelete = await _context.Malfunctions
                .Include(m => m.Devices)
                .Include(m => m.RecoveryActions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (malfunctionToDelete == null)
            {
                return NotFound();
            }

            _context.Malfunctions.Remove(malfunctionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateMalfunctionState(int id, [FromBody] EditMalfunctionDTO updatedUser)
        {
            var malfunction = _context.Malfunctions.FirstOrDefault(m => m.Id == id);
            if (malfunction == null)
            {
                return NotFound("Malfunction with not found");
            }

            malfunction.DateEnd = updatedUser.DateEnd;
            malfunction.Status = updatedUser.Status;

            _context.SaveChanges();

            return Ok(malfunction);
        }
    }
}
