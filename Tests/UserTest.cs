using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionProjectManagement.Controllers;
using ConstructionProjectManagement.Data;
using ConstructionProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ConstructionProjectManagement.Tests
{
    public class UserControllerTests
    {
        private readonly UserController _controller;
        private readonly ApplicationDbContext _context;
        private readonly DbContextOptions<ApplicationDbContext> _options;

        public UserControllerTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(_options);
            _controller = new UserController(_context);

            // Seed the database with initial data
            _context.Users.Add(new User { UserId = 1, Username = "Test User" });
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetUsers_ReturnsOkResult_WithUsers()
        {
            // Act
            var result = await _controller.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<User>>(okResult.Value);
            Assert.Single(users);
        }

        [Fact]
        public async Task GetUser_ReturnsNotFoundResult_WhenUserDoesNotExist()
        {
            // Act
            var result = await _controller.GetUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostUser_ReturnsCreatedAtActionResult_WhenUserIsCreated()
        {
            // Arrange
            var user = new User { UserId = 2, Username = "New User" };

            // Act
            var result = await _controller.PostUser(user);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdUser = Assert.IsType<User>(createdResult.Value);
            Assert.Equal(user.UserId, createdUser.UserId);
        }

        [Fact]
        public async Task PutUser_ReturnsNoContentResult_WhenUserIsUpdated()
        {
            // Arrange
            var user = new User { UserId = 1, Username = "Updated User" };

            // Act
            var result = await _controller.PutUser(1, user);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNoContentResult_WhenUserIsDeleted()
        {
            // Act
            var result = await _controller.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
