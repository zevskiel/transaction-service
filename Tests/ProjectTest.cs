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
    public class ProjectControllerTests
    {
        private readonly Mock<ILogger<ProjectController>> _mockLogger;
        private readonly ApplicationDbContext _context;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);

            // Seed the database with initial data
            _context.Projects.Add(new Project { ProjectId = 1, ProjectName = "Test Project", ProjectStage = "Concept", ProjectStartDate = DateTime.Now.AddDays(1) });
            _context.SaveChanges();

            _mockLogger = new Mock<ILogger<ProjectController>>();
            _controller = new ProjectController(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task GetProjects_ReturnsOkResult_WithProjects()
        {
            // Act
            var result = await _controller.GetProjects();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var projects = Assert.IsType<List<Project>>(okResult.Value);
            Assert.Single(projects);
        }

        [Fact]
        public async Task GetProject_ReturnsNotFoundResult_WhenProjectDoesNotExist()
        {
            // Act
            var result = await _controller.GetProject(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostProject_ReturnsBadRequest_WhenStartDateIsInThePast()
        {
            // Arrange
            var project = new Project
            {
                ProjectId = 2,
                ProjectName = "Invalid Project",
                ProjectStage = "Concept",
                ProjectStartDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = await _controller.PostProject(project);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PutProject_ReturnsNoContentResult_WhenProjectIsUpdated()
        {
            // Arrange
            var project = new Project
            {
                ProjectId = 1,
                ProjectName = "Updated Project",
                ProjectStage = "Design & Documentation",
                ProjectStartDate = DateTime.Now.AddDays(2)
            };

            // Act
            var result = await _controller.PutProject(1, project);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNoContentResult_WhenProjectIsDeleted()
        {
            // Act
            var result = await _controller.DeleteProject(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProject_ReturnsNotFound_WhenProjectDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteProject(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
