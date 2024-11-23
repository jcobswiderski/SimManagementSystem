using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManagementSystem.Services.Tests
{
    [TestClass()]
    public class SessionCategoriesServiceTests
    {
        private SessionCategoriesService _sessionCategoriesService;
        private SimManagementSystemContext _context;

        public SessionCategoriesServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _sessionCategoriesService = new SessionCategoriesService(_context);
        }

        [TestMethod()]
        public async Task GetSessionCategories_ReturnOk_WhenSessionCategoriesExist()
        {
            // Arrange
            var category1 = new SessionCategory { Id = 1, Name = "Exam" };
            var category2 = new SessionCategory { Id = 2, Name = "Training" };

            await _context.SessionCategories.AddAsync(category1);
            await _context.SessionCategories.AddAsync(category2);
            await _context.SaveChangesAsync();

            // Act
            var getSessionCategories = await _sessionCategoriesService.GetSessionCategories();

            // Assert
            Assert.IsInstanceOfType(getSessionCategories, typeof(OkObjectResult));
            var okResult = (OkObjectResult)getSessionCategories;
            var sessionCategories = okResult.Value as List<SessionCategory>;
            Assert.IsNotNull(sessionCategories);
        }

        [TestMethod()]
        public async Task GetSessionCategories_ReturnNotFound_WhenSessionCategoryIsEmpty()
        {
            // Act
            var getSessionCategories = await _sessionCategoriesService.GetSessionCategories();

            // Assert
            Assert.IsInstanceOfType(getSessionCategories, typeof(NotFoundObjectResult));
        }
    }
}