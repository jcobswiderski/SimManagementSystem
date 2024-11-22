using Microsoft.AspNetCore.Mvc;

namespace SimManagementSystem.Services
{
    public interface ISessionCategoriesService
    {
        Task<IActionResult> GetSessionCategories();
    }
}
