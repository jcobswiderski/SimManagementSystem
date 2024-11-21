using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQTGsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public TestQTGsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetQtgTests()
        {
            var testQtgs = await _context.TestQtgs.ToListAsync();

            if (testQtgs == null)
            {
                return NotFound("Test QTGs not found.");
            }

            return Ok(testQtgs);
        }
    }
}
