using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimManagementSystem.DataAccessLayer;
using SimManagementSystem.DataTransferObjects;
using SimManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SimManagementSystem.Services.Tests
{
    [TestClass()]
    public class InspectionsServiceTests
    {
        private InspectionsService _inspectionsService;
        private SimManagementSystemContext _context;

        public InspectionsServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _inspectionsService = new InspectionsService(_context);
        }

        [TestMethod]
        public async Task GetInspections_ReturnOkObjectResult_WhenInspectionsExists()
        {
            // Arrange
            _context.Inspections.Add(new Inspection
            {
                Id = 1,
                Date = System.DateTime.UtcNow,
                InspectionType = new InspectionType { Name = "Navigational database update" },
                OperatorNavigation = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" },
                Notice = "Inspection notice"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _inspectionsService.GetInspections();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetInspection_ReturnOkObjectResult_IfInspectionExist()
        {
            // Arrange
            _context.Inspections.Add(new Inspection
            {
                Id = 1,
                Date = System.DateTime.UtcNow,
                InspectionType = new InspectionType { Name = "Type A" },
                OperatorNavigation = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" },
                Notice = "Inspection notice"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _inspectionsService.GetInspection(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetInspection_NotFound_WhenInspectionDoesNotExist()
        {
            // Act
            var result = await _inspectionsService.GetInspection(99);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public async Task CreateInspection_AddInspection_ReturnOkObjectResult()
        {
            // Arrange
            var newInspection = new CreateInspectionDTO
            {
                InspectionTypeId = 1,
                Date = System.DateTime.UtcNow,
                Operator = 1,
                Notice = "New inspection notice"
            };

            // Act
            var result = await _inspectionsService.CreateInspection(newInspection);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var createdInspection = okResult.Value as Inspection;
            Assert.IsNotNull(createdInspection);
            Assert.AreEqual("New inspection notice", createdInspection.Notice);
            Assert.AreEqual(1, _context.Inspections.Count());
        }

        [TestMethod]
        public async Task DeleteInspection_NoContentResult_WhenInspectionExists()
        {
            // Arrange
            _context.Inspections.Add(new Inspection { Id = 1, Notice = "Inspection" });
            await _context.SaveChangesAsync();

            // Act
            var result = await _inspectionsService.DeleteInspection(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(0, _context.Inspections.Count());
        }

        [TestMethod]
        public async Task DeleteInspection_ReturnNoContent_WhenInspectionDoesNotExist()
        {
            // Act
            var result = await _inspectionsService.DeleteInspection(84153);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }

    }
}