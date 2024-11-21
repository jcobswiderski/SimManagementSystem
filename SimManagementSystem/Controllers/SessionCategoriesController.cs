using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

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
            return Ok(categories);
        }
    }
}
