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
    public class DevicesServiceTests
    {
        private DevicesService _devicesService;
        private SimManagementSystemContext _context;

        public DevicesServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _devicesService = new DevicesService(_context);
        }

        [TestMethod]
        public async Task GetDevices_ReturnsOk_WhenDevicesExist()
        {
            // Arrange
            var device1 = new Device { Id = 1, Name = "Projector", Tag = "PROJ" };
            var device2 = new Device { Id = 2, Name = "Collective", Tag = "COL" };

            await _context.Devices.AddAsync(device1);
            await _context.Devices.AddAsync(device2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _devicesService.GetDevices();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = (OkObjectResult)result;
            var devices = okResult.Value as List<Device>;
            Assert.IsNotNull(devices);
            Assert.AreEqual(2, devices.Count);
        }

        [TestMethod]
        public async Task GetDevice_ReturnsNotFound_WhenDeviceDoesNotExist()
        {
            // Act
            var result = await _devicesService.GetDevice(123456);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task CreateDevice_ReturnsCreated_WhenDeviceIsSuccessfullCreated()
        {
            // Arrange
            var newDevice = new CreateDeviceDTO
            {
                Name = "Projector",
                Tag = "PROJ"
            };

            // Act
            var result = await _devicesService.CreateDevice(newDevice);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task DeleteDevice_ReturnsNoContent_WhenDeviceIsSuccessfullDeleted()
        {
            // Arrange
            var device = new Device { Id = 1, Name = "Projector", Tag = "PROJ" };

            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();

            // Act
            var result = await _devicesService.DeleteDevice(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public async Task DeleteDevice_ReturnsNotFound_WhenDeviceDoesNotExist()
        {
            // Act
            var result = await _devicesService.DeleteDevice(12415);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}