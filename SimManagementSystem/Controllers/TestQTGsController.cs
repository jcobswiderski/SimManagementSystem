using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestQTGsController : ControllerBase
    {
        private readonly ITestQTGsService _testQTGsService;

        public TestQTGsController(SimManagementSystemContext context, ITestQTGsService testQTGsService)
        {
            _testQTGsService = testQTGsService;
        }

        /// <summary>
        /// Get all QTG tests stored in system.
        /// </summary>
        /// <returns>List of QTG test objects</returns>
        [HttpGet]
        public async Task<IActionResult> GetQtgTests()
        {
            return await _testQTGsService.GetQtgTests();
        }
    }
}
