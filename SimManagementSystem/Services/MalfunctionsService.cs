using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class MalfunctionsService : IMalfunctionsService
    {
        private readonly SimManagementSystemContext _context;

        public MalfunctionsService(SimManagementSystemContext context)
        {
            _context = context;
        }

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

            if (malfunctions == null || !malfunctions.Any())
            {
                return new NotFoundObjectResult("Malfunctions not found.");
            }

            return new OkObjectResult(malfunctions);
        }

        public async Task<IActionResult> GetMalfunctionsCount(DateTime? dateBegin, DateTime? dateEnd)
        {
            if (!dateBegin.HasValue || !dateEnd.HasValue)
            {
                return new BadRequestObjectResult("Please enter date begin and date end!");
            }

            int malfunctionCount = await _context.Malfunctions
                .Where(m => m.DateBegin >= dateBegin.Value && m.DateBegin <= dateEnd.Value)
                .CountAsync();

            return new OkObjectResult(malfunctionCount);
        }

        public async Task<IActionResult> GetUnsolvedMalfunctionsCount()
        {
            int malfunctionCount = await _context.Malfunctions
                .Where(m => m.Status == false)
                .CountAsync();

            return new OkObjectResult(malfunctionCount);
        }

        public async Task<IActionResult> GetMalfunction(int id)
        {
            var malfunction = await _context.Malfunctions
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


            if (malfunction == null)
            {
                return new NotFoundObjectResult("Malfunction with given ID not found.");
            }

            return new OkObjectResult(malfunction);
        }

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

            if (malfunctions == null)
            {
                return new NotFoundObjectResult("Malfunctions for specific device not found.");
            }

            return new OkObjectResult(malfunctions);
        }

        public async Task<IActionResult> CreateMalfunction(CreateMalfunctionDTO newMalfunction)
        {
            var devices = await _context.Devices
                .Where(d => newMalfunction.Devices.Contains(d.Id))
                .ToListAsync();

            if (devices == null || devices.Count != newMalfunction.Devices.Count)
            {
                return new BadRequestObjectResult("Some devices does not exist.");
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

            return new CreatedResult();
        }

        public async Task<IActionResult> DeleteMalfunction(int id)
        {
            var malfunctionToDelete = await _context.Malfunctions
                .Include(m => m.Devices)
                .Include(m => m.RecoveryActions)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (malfunctionToDelete == null)
            {
                return new NotFoundObjectResult("Malfunction with given ID not found.");
            }

            _context.Malfunctions.Remove(malfunctionToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> UpdateMalfunctionState(int id, [FromBody] EditMalfunctionDTO updatedUser)
        {
            var malfunction = await _context.Malfunctions.FirstOrDefaultAsync(m => m.Id == id);
            if (malfunction == null)
            {
                return new NotFoundObjectResult("Malfunction with given ID not found.");
            }

            malfunction.DateEnd = updatedUser.DateEnd;
            malfunction.Status = updatedUser.Status;

            await _context.SaveChangesAsync();

            return new OkObjectResult(malfunction);
        }
    }
}
