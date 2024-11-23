using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimManagementSystem.Services.Tests
{
    [TestClass()]
    public class PredefinedSessionsServiceTests
    {
        private PredefinedSessionsService _predefinedSessionsService;
        private SimManagementSystemContext _context;

        public PredefinedSessionsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _predefinedSessionsService = new PredefinedSessionsService(_context);
        }

        [TestMethod]
        public async Task GetPredefinedSessions_ReturnOk_WhenSessionsExist()
        {
            // Arrange
            var predefinedSession1 = new PredefinedSession
            {
                Category = 1,
                Name = "Session1",
                Description = "Description1",
                Duration = 60,
                Abbreviation = "S1"
            };

            var predefinedSession2 = new PredefinedSession
            {
                Category = 2,
                Name = "Session2",
                Description = "Description2",
                Duration = 120,
                Abbreviation = "S2"
            };

            await _context.PredefinedSessions.AddAsync(predefinedSession1);
            await _context.PredefinedSessions.AddAsync(predefinedSession2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _predefinedSessionsService.GetPredefinedSessions();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var sessions = okResult.Value as List<PredefinedSession>;
            Assert.IsNotNull(sessions);
            Assert.AreEqual(2, sessions.Count);
            Assert.AreEqual("Session1", sessions[0].Name);
            Assert.AreEqual("Session2", sessions[1].Name);
        }

        [TestMethod]
        public async Task GetPredefinedSessions_ReturnNotFound_WhenNoSessionsExist()
        {
            // Act
            var result = await _predefinedSessionsService.GetPredefinedSessions();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Predefined session not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task CreatePredefinedSession_ReturnsOk_WhenSessionCreatedSuccessfuly()
        {
            // Arrange
            var newSession = new CreatePredefinedSessionDTO
            {
                Category = 1,
                Name = "Session",
                Description = "This is new session.",
                Duration = 90,
                Abbreviation = "NS"
            };

            // Act
            var result = await _predefinedSessionsService.CreatePredefinedSession(newSession);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var createdSession = okResult.Value as PredefinedSession;
            Assert.IsNotNull(createdSession);
            Assert.AreEqual("Session", createdSession.Name);
            Assert.AreEqual("NS", createdSession.Abbreviation);
        }

    }
}