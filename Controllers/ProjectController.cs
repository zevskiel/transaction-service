using Microsoft.AspNetCore.Mvc;
using ConstructionProjectManagement.Data;
using ConstructionProjectManagement.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ConstructionProjectManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(ApplicationDbContext context, ILogger<ProjectController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Get all projects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            var projects = await _context.Projects.Include(p => p.ProjectCreator).ToListAsync();
            return Ok(projects);
        }

        // Get a project by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.Include(p => p.ProjectCreator).FirstOrDefaultAsync(p => p.ProjectId == id);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // Create a new project
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            // Validate future date for specific stages
            if (new[] { "Concept", "Design & Documentation", "Pre-Construction" }.Contains(project.ProjectStage) &&
                project.ProjectStartDate <= DateTime.Now)
            {
                return BadRequest("Project start date must be in the future for stages: Concept, Design & Documentation, Pre-Construction");
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // Update a project
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            // Validate future date for specific stages
            if (new[] { "Concept", "Design & Documentation", "Pre-Construction" }.Contains(project.ProjectStage) &&
                project.ProjectStartDate <= DateTime.Now)
            {
                return BadRequest("Project start date must be in the future for stages: Concept, Design & Documentation, Pre-Construction");
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Delete a project
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProjectId == id);
        }
    }
}
