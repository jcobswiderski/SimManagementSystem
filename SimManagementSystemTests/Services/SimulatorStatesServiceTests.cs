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
    public class SimulatorStatesServiceTests
    {
        private SimulatorStatesService _simulatorStatesService;
        private SimManagementSystemContext _context;

        public SimulatorStatesServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _simulatorStatesService = new SimulatorStatesService(_context);
        }

        [TestMethod()]
        public async Task GetSimulatorStates_ReturnOk_WhenSimulatorStatesAreAvailable()
        {
            // Arrange
            var simulatorState = new SimulatorState { Id = 1, MeterState = 100, StartupTime = DateTime.UtcNow, Operator = 1 };

            // Act
            var result = await _simulatorStatesService.GetSimulatorStates();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetSimulatorStateDifference_ReturnNotFound_WhenNoStatesForDates()
        {
            // Act
            var result = await _simulatorStatesService.GetSimulatorStateDifference(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Simulator states not found for given dates.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task DeleteSimulatorState_ReturnNoContent_WhenStateExists()
        {
            // Arrange
            var state = new SimulatorState { Id = 1, MeterState = 100 };
            await _context.SimulatorStates.AddAsync(state);
            await _context.SaveChangesAsync();

            // Act
            var result = await _simulatorStatesService.DeleteSimulatorState(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var deletedState = await _context.SimulatorStates.FirstOrDefaultAsync(s => s.Id == 1);
            Assert.IsNull(deletedState);
        }

    }
}