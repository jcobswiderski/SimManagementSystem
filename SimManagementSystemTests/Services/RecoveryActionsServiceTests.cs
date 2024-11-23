using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace SimManagementSystem.Services.Tests
{
    [TestClass()]
    public class RecoveryActionsServiceTests
    {
        private RecoveryActionsService _service;
        private SimManagementSystemContext _context;

        public RecoveryActionsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _service = new RecoveryActionsService(_context);
        }

        [TestMethod]
        public async Task GetRecoveryActionsForMalfunction_ReturnOk_WhenActionsExists()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user2 = new User { Id = 2, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

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

            var recoveryAction1 = new RecoveryAction
            {
                Date = DateTime.UtcNow,
                Description = "Action1",
                MalfunctionId = 1
            };

            var recoveryAction2 = new RecoveryAction
            {
                Date = DateTime.UtcNow,
                Description = "Action2",
                MalfunctionId = 1
            };

            await _context.RecoveryActions.AddAsync(recoveryAction1);
            await _context.RecoveryActions.AddAsync(recoveryAction2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetRecoveryActionsForMalfunction(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetRecoveryActionsForMalfunction_ReturnNotFound_WhenNoActionsExist()
        {
            // Act
            var result = await _service.GetRecoveryActionsForMalfunction(18618);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Recovery actions not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task CreateRecoveryAction_ReturnCreated_WhenActionCreated()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user2 = new User { Id = 2, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

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

            var newAction = new CreateRecoveryActionDTO
            {
                Date = DateTime.UtcNow,
                Description = "New Recovery Action",
                MalfunctionId = 1
            };

            // Act
            var result = await _service.CreateRecoveryAction(newAction);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedResult));

            var actionInDb = await _context.RecoveryActions.FirstOrDefaultAsync(a => a.Description == "New Recovery Action");
            Assert.IsNotNull(actionInDb);
            Assert.AreEqual(newAction.Description, actionInDb.Description);
            Assert.AreEqual(newAction.MalfunctionId, actionInDb.MalfunctionId);
        }

        [TestMethod]
        public async Task DeleteRecoveryAction_ReturnNoContent_WhenActionDeleted()
        {
            // Arrange
            var recoveryAction = new RecoveryAction
            {
                Id = 1,
                Date = DateTime.UtcNow,
                Description = "Action",
                MalfunctionId = 1
            };

            await _context.RecoveryActions.AddAsync(recoveryAction);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteRecoveryAction(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

    }
}