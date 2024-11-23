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
    public class RolesServiceTests
    {
        private RolesService _rolesService;
        private SimManagementSystemContext _context;

        public RolesServiceTests()
        {
            var options = new DbContextOptionsBuilder<SimManagementSystemContext>()
            .UseInMemoryDatabase("TestDatabase" + Guid.NewGuid())
            .Options;

            _context = new SimManagementSystemContext(options);
            _rolesService = new RolesService(_context);
        }

        [TestMethod]
        public async Task GetRoles_ReturnOk_WhenRolesExist()
        {
            // Arrange
            var role1 = new Role { Id = 1, Name = "Admin" };
            var role2 = new Role { Id = 2, Name = "Engineer" };

            await _context.Roles.AddRangeAsync(role1, role2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _rolesService.GetRoles();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var roles = okResult.Value as List<Role>;
            Assert.IsNotNull(roles);
            Assert.AreEqual(2, roles.Count);
        }

        [TestMethod]
        public async Task GetRoles_ReturnNotFound_WhenNoRolesExist()
        {
            // Act
            var result = await _rolesService.GetRoles();

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual("Roles not found.", notFoundResult.Value);
        }

        [TestMethod]
        public async Task AssignRole_ReturnOk_WhenRoleAssigned()
        {
            // Arrange
            var user1 = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1" };
            var role = new Role { Id = 1, Name = "Admin" };

            await _context.Users.AddAsync(user1);
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();

            var assignRoleDTO = new AssignRoleDTO { Id = 1 };

            // Act
            var result = await _rolesService.AssignRole(1, assignRoleDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Role assigned to user.", okResult.Value);

            var updatedUser = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == 1);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(1, updatedUser.Roles.Count);
        }

        [TestMethod]
        public async Task RemoveRole_ReturnOk_WhenRoleRemoved()
        {
            // Arrange
            var role = new Role { Id = 1, Name = "Admin" };
            var user = new User { Id = 1, FirstName = "FirstName", LastName = "LastName", Login = "Login123", Password = "YUI^&*yui678", Salt = "q21e1fe1256fe1", Roles = new List<Role> { role } };

            await _context.Roles.AddAsync(role);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var removeRoleDTO = new RemoveRoleDTO { Id = 1 };

            // Act
            var result = await _rolesService.RemoveRole(1, removeRoleDTO);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Role sucessfuly removed", okResult.Value);

            var updatedUser = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == 1);
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(0, updatedUser.Roles.Count);
        }
    }
}