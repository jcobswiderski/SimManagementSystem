using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System.Data;

namespace SimManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionCategoriesController : ControllerBase
    {
        private readonly ISessionCategoriesService _sessionCategoriesService;

        public SessionCategoriesController(ISessionCategoriesService sessionCategoriesService)
        {
            _sessionCategoriesService = sessionCategoriesService;
        }

        /// <summary>
        /// Get all session categories like training, exam, technical...
        /// </summary>
        /// <returns>List of categories</returns>
        [HttpGet]
        public async Task<IActionResult> GetSessionCategories()
        {
            return await _sessionCategoriesService.GetSessionCategories();
        }
    }
}
