using Microsoft.AspNetCore.Mvc;

namespace SimManagementSystem.Services
{
    public interface ITestQTGsService
    {
        Task<IActionResult> GetQtgTests();
    }
}
