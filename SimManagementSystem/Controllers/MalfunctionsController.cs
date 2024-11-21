using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IActionResult> GetMalfunctions()
        {
            var malfunctions = await _context.Malfunctions
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
                .ToListAsync();
            return Ok(malfunctions);
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetMalfunctionsCount(DateTime? dateBegin, DateTime? dateEnd)
        {
            if (!dateBegin.HasValue || !dateEnd.HasValue) {
                return BadRequest("Please enter date begin and date end!");
            }

            int malfunctionCount = await _context.Malfunctions
                .Where(m => m.DateBegin >= dateBegin.Value && m.DateBegin <= dateEnd.Value)
                .CountAsync();

            return Ok(malfunctionCount);
        }

        [HttpGet("count/unsolved")]
        public async Task<IActionResult> GetUnsolvedMalfunctionsCount()
        {
            int malfunctionCount = await _context.Malfunctions
                .Where(m => m.Status == false)
                .CountAsync();

            return Ok(malfunctionCount);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMalfunction(int id)
        {
            var malfunctions = await _context.Malfunctions
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
                .FirstOrDefaultAsync();
            return Ok(malfunctions);
        }

        [HttpGet("device/{deviceId}")]
        public async Task<IActionResult> GetMalfunctionForDevice(int deviceId)
        {
            var malfunctions = await _context
                .Malfunctions
                .Where(m => m.Devices.Any(d => d.Id == deviceId))
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
                .ToListAsync();
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
                return BadRequest("Some devices does not exist.");
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
                return NotFound("Malfunction with given ID not found.");
            }

            _context.Malfunctions.Remove(malfunctionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMalfunctionState(int id, [FromBody] EditMalfunctionDTO updatedUser)
        {
            var malfunction = await _context.Malfunctions.FirstOrDefaultAsync(m => m.Id == id);
            if (malfunction == null)
            {
                return NotFound("Malfunction with given ID not found.");
            }

            malfunction.DateEnd = updatedUser.DateEnd;
            malfunction.Status = updatedUser.Status;

            _context.SaveChangesAsync();

            return Ok(malfunction);
        }
    }
}
