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
    public class MaintenanceTypesServiceTests
    {
        private MaintenanceTypesService _maintenanceTypesService;
        private SimManagementSystemContext _context;

        public MaintenanceTypesServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _maintenanceTypesService = new MaintenanceTypesService(_context);
        }

        [TestMethod]
        public async Task GetMaintenanceTypes_OkObjectResult_IfMaintenanceTypesExist()
        {
            // Arrange
            await _context.MaintenanceTypes.AddAsync(new MaintenanceType { Id = 1, Name = "Routine Check", Tasks = "Task1, Task2" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _maintenanceTypesService.GetMaintenanceTypes();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var maintenanceTypes = okResult.Value as List<MaintenanceType>;
            Assert.IsNotNull(maintenanceTypes);
            Assert.AreEqual(1, maintenanceTypes.Count);
            Assert.AreEqual("Routine Check", maintenanceTypes[0].Name);
        }
    }
}