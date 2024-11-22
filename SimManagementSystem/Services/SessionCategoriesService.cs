using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimManagementSystem.DataAccessLayer;

namespace SimManagementSystem.Services
{
    public class SessionCategoriesService : ISessionCategoriesService
    {
        private readonly SimManagementSystemContext _context;

        public SessionCategoriesService(SimManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetSessionCategories()
        {
            var categories = await _context.SessionCategories.ToListAsync();

            if (categories == null)
            {
                return new NotFoundObjectResult("Sessions categories not found.");
            }

            return new OkObjectResult(categories);
        }
    }
}
