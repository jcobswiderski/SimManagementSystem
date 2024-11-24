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
using static System.Net.Mime.MediaTypeNames;

namespace SimManagementSystem.Services.Tests
{
    [TestClass()]
    public class TestQTGsServiceTests
    {
        private TestQTGsService _service;
        private SimManagementSystemContext _context;

        public TestQTGsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _service = new TestQTGsService(_context);
        }

        [TestMethod]
        public async Task GetQtgTests_ReturnsAllTests()
        {
            // Arrange
            var test1 = new TestQtg { Id = 1, Title = "Title", Stage = "Stage", Description = "Description" };
            var test2 = new TestQtg { Id = 2, Title = "Title", Stage = "Stage", Description = "Description" };

            _context.TestQtgs.AddAsync(test1);
            _context.TestQtgs.AddAsync(test2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetQtgTests();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<TestQtg>));
        }
    }
}