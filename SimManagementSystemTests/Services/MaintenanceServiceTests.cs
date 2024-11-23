using Azure;
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
    public class MaintenanceServiceTests
    {
        private MaintenanceService _maintenanceService;
        private SimManagementSystemContext _context;

        public MaintenanceServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _maintenanceService = new MaintenanceService(_context);
        }

        [TestMethod]
        public async Task GetMaintenances_ShouldReturnOkObjectResult_WhenMaintenancesExist()
        {
            // Arrange
            var maintenanceType = new MaintenanceType { Id = 1, Name = "MT", Tasks = "TasksList" };
            var user = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

            _context.Maintenances.Add(new Maintenance
            {
                Id = 1,
                TypeNavigation = maintenanceType,
                ExecutorNavigation = user,
                Date = System.DateTime.UtcNow,
                Realized = false
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _maintenanceService.GetMaintenances();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetMaintenances_NotFound_WhenMaintenancesDoesNotExists()
        {
            // Act
            var result = await _maintenanceService.GetMaintenances();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetIncompleteMaintenancesCount_WhenIncompletedMaintenancesExist()
        {
            // Arrange
            _context.Maintenances.Add(new Maintenance { Id = 1, Date = System.DateTime.UtcNow, Realized = false });
            _context.Maintenances.Add(new Maintenance { Id = 2, Date = System.DateTime.UtcNow, Realized = true });
            await _context.SaveChangesAsync();

            // Act
            var result = await _maintenanceService.GetIncompleteMaintenancesCount();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(1, okResult.Value);
        }

        [TestMethod]
        public async Task CreateMaintenance_AddMaintenance_ReturnOkObjectResult()
        {
            // Arrange
            var newMaintenance = new CreateMaintenanceDTO
            {
                Type = 1,
                Date = System.DateTime.UtcNow
            };

            // Act
            var result = await _maintenanceService.CreateMaintenance(newMaintenance);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Maintenance successfully added.", okResult.Value);
            Assert.AreEqual(1, _context.Maintenances.Count());
        }

        [TestMethod]
        public async Task DeleteMaintenance_ReturnNotFoundObjectResult()
        {
            // Act
            var result = await _maintenanceService.DeleteMaintenance(87412);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}