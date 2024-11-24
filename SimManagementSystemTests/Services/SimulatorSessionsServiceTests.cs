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
    public class SimulatorSessionsServiceTests
    {
        private SimulatorSessionsService _simulatorSessionsService;
        private SimManagementSystemContext _context;

        public SimulatorSessionsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _simulatorSessionsService = new SimulatorSessionsService(_context);
        }

        [TestMethod]
        public async Task GetSimulatorSessions_ReturnOk_WhenSessionsExist()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login1d23", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user2 = new User { Id = 2, FirstName = "FirstName", LastName = "LastName", Login = "Login123h", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user3 = new User { Id = 3, FirstName = "FirstName", LastName = "LastName", Login = "Login123d", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user4 = new User { Id = 4, FirstName = "FirstName", LastName = "LastName", Login = "Login1235", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

            var session = new SimulatorSession
            {
                Id = 1,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(2),
                Realized = true,
                PredefinedSessionNavigation = new PredefinedSession
                {
                    Name = "Predefined session",
                    Abbreviation = "PS",
                    Description = "Description",
                    Duration = 120,
                    CategoryNavigation = new SessionCategory { Id = 1, Name = "Category" }
                },
                PilotSeatNavigation = user1,
                CopilotSeatNavigation = user2,
                ObserverSeatNavigation = user3,
                SupervisorSeatNavigation = user4
            };

            await _context.SimulatorSessions.AddAsync(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _simulatorSessionsService.GetSimulatorSessions();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetPlannedSimulatorSessionsCount_ReturnOk_WithCorrectCount()
        {
            // Arrange
            var session1 = new SimulatorSession { Id = 1, Realized = false };
            var session2 = new SimulatorSession { Id = 2, Realized = false };

            await _context.SimulatorSessions.AddAsync(session1);
            await _context.SimulatorSessions.AddAsync(session2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _simulatorSessionsService.GetPlannedSimulatorSessionsCount();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(2, okResult.Value);
        }

        [TestMethod]
        public async Task CheckSimSessionConflict_ReturnOk_WithConflictCount()
        {
            // Arrange
            var session = new SimulatorSession
            {
                Id = 1,
                BeginDate = DateTime.Now,
                EndDate = DateTime.Now.AddHours(1)
            };

            await _context.SimulatorSessions.AddAsync(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _simulatorSessionsService.CheckSimSessionConflict(DateTime.Now.AddMinutes(30), 60);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(1, okResult.Value);
        }

        [TestMethod]
        public async Task CreateSession_ReturnCreated_WhenSessionIsValid()
        {
            // Arrange
            var predefinedSession = new PredefinedSession
            {
                Name = "Predefined session",
                Abbreviation = "PS",
                Description = "Description",
                Duration = 120,
                CategoryNavigation = new SessionCategory { Id = 1, Name = "Category" }
            };
            
            await _context.PredefinedSessions.AddAsync(predefinedSession);
            await _context.SaveChangesAsync();

            var newSession = new CreateSimulatorSessionDTO
            {
                PredefinedSession = 1,
                BeginDate = DateTime.Now,
                PilotSeat = 1,
                CopilotSeat = 2,
                SupervisorSeat = 3,
                ObserverSeat = 4,
                Realized = false
            };

            // Act
            var result = await _simulatorSessionsService.CreateSession(newSession);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            var createdSession = await _context.SimulatorSessions.FirstOrDefaultAsync();
            Assert.IsNotNull(createdSession);
        }

        [TestMethod]
        public async Task DeleteSimulatorSession_ReturnNoContent_WhenSessionDeleted()
        {
            // Arrange
            var session = new SimulatorSession { Id = 1 };

            await _context.SimulatorSessions.AddAsync(session);
            await _context.SaveChangesAsync();

            // Act
            var result = await _simulatorSessionsService.DeleteSimulatorSession(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var deletedSession = await _context.SimulatorSessions.FirstOrDefaultAsync(s => s.Id == 1);
            Assert.IsNull(deletedSession);
        }

    }
}