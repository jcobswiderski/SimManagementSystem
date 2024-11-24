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
    public class TestResultsServiceTests
    {
        private TestResultsService _service;
        private SimManagementSystemContext _context;

        public TestResultsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _service = new TestResultsService(_context);
        }

        [TestMethod]
        public async Task GetTestResults_ReturnOk_WhenResultsExist()
        {
            // Arrange
            var executor = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var test = new TestQtg { Id = 1, Title = "Title", Stage = "Stage", Description = "Description" };
            var testResult = new SimManagementSystem.DataAccessLayer.TestResult
            {
                Id = 1,
                TestNavigation = test,
                IsPassed = true,
                Date = DateTime.Now,
                Observation = "Observation",
                ExcutorNavigation = executor
            };

            await _context.Users.AddAsync(executor);
            await _context.TestQtgs.AddAsync(test);
            await _context.TestResults.AddAsync(testResult);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetTestResults();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetTestResult_ReturnNotFound_WhenResultDoesNotExist()
        {
            // Act
            var result = await _service.GetTestResult(7541);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));

            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);

            Assert.AreEqual("Test results for given ID not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task DeleteTestResult_ReturnNoContent_WhenResultExists()
        {
            // Arrange
            var executor = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var test = new TestQtg { Id = 1, Title = "Title", Stage = "Stage", Description = "Description" };
            var testResult = new SimManagementSystem.DataAccessLayer.TestResult
            {
                Id = 1,
                TestNavigation = test,
                IsPassed = true,
                Date = DateTime.Now,
                Observation = "Observation",
                ExcutorNavigation = executor
            };

            await _context.TestResults.AddAsync(testResult);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteTestResult(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var deletedTestResult = await _context.TestResults.FirstOrDefaultAsync(t => t.Id == 1);

            Assert.IsNull(deletedTestResult);
        }

    }
}