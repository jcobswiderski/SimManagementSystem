using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly SimManagementSystemContext _context;

        public TestResultsController(SimManagementSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTestResults()
        {
            var testResults = await _context.TestResults
                .Select(t => new {
                    t.Id,
                    title = t.TestNavigation.Title,
                    stage = t.TestNavigation.Stage,
                    t.IsPassed,
                    date = t.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.Observation,
                    executor = t.ExcutorNavigation.FirstName + " " + t.ExcutorNavigation.LastName
                })
                .ToListAsync();

            if (testResults == null)
            {
                return NotFound("Test results not found.");
            }

            return Ok(testResults);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestResult(int id)
        {
            var test = await _context.TestResults
                .OrderByDescending(m => m.Date)
                .Select(t => new {
                    t.Id,
                    title = t.TestNavigation.Title,
                    stage = t.TestNavigation.Stage,
                    description = t.TestNavigation.Description,
                    t.IsPassed,
                    date = t.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    t.Observation,
                    executor = t.ExcutorNavigation.FirstName + " " + t.ExcutorNavigation.LastName
                })
                .Where(t => t.Id == id)
                .FirstOrDefaultAsync();

            if (test == null)
            {
                return NotFound("Test results for given ID not found.");
            }

            return Ok(test);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestResult(int id)
        {
            var testToDelete = await _context.TestResults
                .FirstOrDefaultAsync(t => t.Id == id);

            if (testToDelete == null)
            {
                return NotFound();
            }

            _context.TestResults.Remove(testToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestResult(CreateTestResultDTO newTest)
        {
            var predefinedTest = await _context.TestQtgs
                .Where(t => t.Id == newTest.Test)
                .FirstOrDefaultAsync();

            if (predefinedTest == null)
            {
                return NotFound("Predefined test not found.");
            }

            var testResult = new TestResult
            {
                Test = predefinedTest.Id,
                IsPassed = newTest.IsPassed,
                Date = newTest.Date,
                Observation = newTest.Observation,
                Excutor = newTest.Executor
            };

            await _context.TestResults.AddAsync(testResult);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
