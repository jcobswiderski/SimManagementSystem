using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetQtgTests()
        {
            var testQtgs = _context.TestQtgs.ToList();
            return Ok(testQtgs);
        }
    }
}
