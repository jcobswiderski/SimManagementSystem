using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestResultsController : ControllerBase
    {
        private readonly ITestResultsService _testResultsService;

        public TestResultsController(ITestResultsService testResultsService)
        {
            _testResultsService = testResultsService;
        }

        /// <summary>
        /// Get all qtg test results.
        /// </summary>
        /// <returns>List of all test results stored in system</returns>
        [HttpGet]
        public async Task<IActionResult> GetTestResults()
        {
            return await _testResultsService.GetTestResults();
        }

        /// <summary>
        /// Get single test result.
        /// </summary>
        /// <param name="id">Target test result id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestResult(int id)
        {
            return await _testResultsService.GetTestResult(id);
        }

        /// <summary>
        /// Delete single test result
        /// </summary>
        /// <param name="id">Target test result id</param>
        /// <returns>NoContent</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestResult(int id)
        {
            return await _testResultsService.DeleteTestResult(id);
        }

        /// <summary>
        /// Add new test result.
        /// </summary>
        /// <param name="newTest">All data needed to add new test</param>
        /// <returns>Created</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTestResult(CreateTestResultDTO newTest)
        {
            return await _testResultsService.CreateTestResult(newTest);
        }
    }
}
