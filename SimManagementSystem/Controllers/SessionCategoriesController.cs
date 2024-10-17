using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetSessionCategories()
        {
            var categories = _context.SessionCategories.ToList();
            return Ok(categories);
        }
    }
}
