using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Services
{
    public class TestQTGsService : ITestQTGsService
    {
        private readonly SimManagementSystemContext _context;

        public TestQTGsService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetQtgTests()
        {
            var testQtgs = await _context.TestQtgs.ToListAsync();

            if (testQtgs == null)
            {
                return new NotFoundObjectResult("Test QTGs not found.");
            }

            return new OkObjectResult(testQtgs);
        }
    }
}
