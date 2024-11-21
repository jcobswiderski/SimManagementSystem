using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using System.Data;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionCategoriesController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public SessionCategoriesController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSessionCategories()
        {
            var categories = await _context.SessionCategories.ToListAsync();

            if (categories == null)
            {
                return NotFound("Sessions categories not found.");
            }

            return Ok(categories);
        }
    }
}
