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
    public class MalfunctionsServiceTests
    {
        private MalfunctionsService _malfunctionsService;
        private SimManagementSystemContext _context;

        public MalfunctionsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _malfunctionsService = new MalfunctionsService(_context);
        }

        [TestMethod]
        public async Task GetMalfunctions_ReturnOk_WhenMalfunctionsExists()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user2 = new User { Id = 2, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

            _context.Users.Add(user1);
            _context.Users.Add(user2);
            await _context.SaveChangesAsync();

            var malfunction1 = new Malfunction
            {
                Name = "Malfunction",
                Description = "Description",
                UserReporter = user1.Id,
                UserHandler = user2.Id,
                DateBegin = DateTime.UtcNow,
                DateEnd = DateTime.UtcNow,
                Status = false,
                RecoveryActions = new List<RecoveryAction>(),
                UserHandlerNavigation = user2,
                UserReporterNavigation = user1,
                Devices = new List<Device> { new Device { Name = "Device1" } }
            };

            await _context.Malfunctions.AddAsync(malfunction1);
            await _context.SaveChangesAsync();

            // Act
            var result = await _malfunctionsService.GetMalfunctions();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetMalfunctions_ReturnNotFoundObjectResult_WhenNoMalfunctionsExist()
        {
            // Act
            var result = await _malfunctionsService.GetMalfunctions();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task GetMalfunctionsCount_BadRequestObjectResult_DatesAreNotProvided()
        {
            // Act
            var result = await _malfunctionsService.GetMalfunctionsCount(null, null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("Please enter date begin and date end!", badRequestResult.Value);
        }

        [TestMethod]
        public async Task CreateMalfunction_ReturnCreatedResult_WhenMalfunctionCreatedSuccessfully()
        {
            // Arrange
            var newMalfunction = new CreateMalfunctionDTO
            {
                Name = "Malfunction",
                Description = "Description",
                UserReporter = 1,
                UserHandler = 2,
                DateBegin = DateTime.Now,
                Status = false,
                Devices = new List<int> { 1, 2 }
            };
            _context.Devices.Add(new Device { Id = 1, Name = "Device1"});
            _context.Devices.Add(new Device { Id = 2, Name = "Device2"});
            await _context.SaveChangesAsync();

            // Act
            var result = await _malfunctionsService.CreateMalfunction(newMalfunction);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task CreateMalfunction_BadRequestObjectResult_WhenDevicesDoNotExists()
        {
            // Arrange
            var newMalfunction = new CreateMalfunctionDTO
            {
                Name = "Malfunction",
                Description = "Description",
                UserReporter = 1,
                UserHandler = 2,
                DateBegin = DateTime.Now,
                Status = false,
                Devices = new List<int> { 7777 }
            };

            // Act
            var result = await _malfunctionsService.CreateMalfunction(newMalfunction);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            var badRequestResult = result as BadRequestObjectResult;
            Assert.AreEqual("Some devices does not exist.", badRequestResult.Value);
        }

        [TestMethod]
        public async Task DeleteMalfunction_NoContentResult_MalfunctionDeleted()
        {
            // Arrange
            var malfunction = new Malfunction
            {
                Name = "Malfunction",
                Description = "Description",
                UserReporter = 1,
                UserHandler = 2,
                DateBegin = DateTime.Now,
                Status = false
            };
            _context.Malfunctions.Add(malfunction);
            await _context.SaveChangesAsync();

            // Act
            var result = await _malfunctionsService.DeleteMalfunction(malfunction.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }
    }
}