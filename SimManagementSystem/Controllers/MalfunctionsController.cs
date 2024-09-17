using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataAccessLayer;

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
                    m.Status
                })
                .Where(m => m.Id == id)
                .FirstOrDefault();
            return Ok(malfunctions);
        }
    }
}
