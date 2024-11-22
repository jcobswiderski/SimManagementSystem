using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public class TestResultsService : ITestResultsService
    {
        private readonly SimManagementSystemContext _context;

        public TestResultsService(SimManagementSystemContext context)
        {
            _context = context;
        }

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
                return new NotFoundObjectResult("Test results not found.");
            }

            return new OkObjectResult(testResults);
        }

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
                return new NotFoundObjectResult("Test results for given ID not found.");
            }

            return new OkObjectResult(test);
        }

        public async Task<IActionResult> DeleteTestResult(int id)
        {
            var testToDelete = await _context.TestResults
                .FirstOrDefaultAsync(t => t.Id == id);

            if (testToDelete == null)
            {
                return new NotFoundObjectResult("Test result not found.");
            }

            _context.TestResults.Remove(testToDelete);
            await _context.SaveChangesAsync();

            return new NoContentResult();
        }

        public async Task<IActionResult> CreateTestResult(CreateTestResultDTO newTest)
        {
            var predefinedTest = await _context.TestQtgs
                .Where(t => t.Id == newTest.Test)
                .FirstOrDefaultAsync();

            if (predefinedTest == null)
            {
                return new NotFoundObjectResult("Predefined test not found.");
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

            return new CreatedResult();
        }
    }
}
