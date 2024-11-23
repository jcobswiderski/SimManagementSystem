using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
    public class UserServiceTests
    {
        private UserService _usersService;
        private Mock<IConfiguration> _configurationMock;
        private SimManagementSystemContext _context;

        public UserServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(config => config["Jwt:Key"]).Returns("JwtKey");
            _configurationMock.Setup(config => config["Jwt:Issuer"]).Returns("JwtIssuer");
            _configurationMock.Setup(config => config["Jwt:Audience"]).Returns("JwtAudience");

            _context = new SimManagementSystemContext(options);
            _usersService = new UserService(_context, _configurationMock.Object);
        }

        [TestMethod]
        public async Task GetUsers_ReturnsOkObjectResult_WhenUsersExists()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var user2 = new User { Id = 2, FirstName = "FirstName", LastName = "LastName", Login = "Login456", Password = "YUI^&*yUi678", Salt = "q21e1gu1256fe1" };

            await _context.Users.AddAsync(user1);
            await _context.Users.AddAsync(user2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _usersService.GetUsers();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task GetUser_ReturnsOkObjectResult_WhenUsersExist()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };

            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            // Act
            var result = await _usersService.GetUser(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
        }

        [TestMethod]
        public async Task Register_AddUserAndReturnOkObjectResult()
        {
            // Arrange
            var newUser = new CreateUserDTO
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Login = "Login123",
                Password = "YUI^&*yui678"
            };

            // Act
            var result = await _usersService.Register(newUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var createdUser = okResult.Value as User;
            Assert.IsNotNull(createdUser);
            Assert.AreEqual(1, _context.Users.Count());
        }

        [TestMethod]
        public async Task Login_ReturnUnauthorized_WhenUserNotFound()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Login = "undefined",
                Password = "undefined"
            };

            // Act
            var result = await _usersService.Login(loginRequest);

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedObjectResult));
        }

        [TestMethod]
        public async Task DeleteUser_ReturnNoContentResult_WhenUserExists()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            await _context.Users.AddAsync(user1);
            await _context.SaveChangesAsync();

            // Act
            var result = await _usersService.DeleteUser(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(0, _context.Users.Count());
        }

        [TestMethod]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _usersService.DeleteUser(12345);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
        }
    }
}