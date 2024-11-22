using Microsoft.AspNetCore.Mvc;
using SimManagementSystem.DataTransferObjects;

namespace SimManagementSystem.Services
{
    public interface ITestResultsService
    {
        Task<IActionResult> GetTestResults();
        Task<IActionResult> GetTestResult(int id);
        Task<IActionResult> DeleteTestResult(int id);
        Task<IActionResult> CreateTestResult(CreateTestResultDTO newTest);
    }
}
