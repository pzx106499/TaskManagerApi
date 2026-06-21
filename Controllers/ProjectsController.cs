using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetProjects()
        {
            var userId = GetUserId();

            var projects = await _context.Projects
                .Include(p => p.Tasks)
                .Where(p =>
                    p.OwnerId == userId ||
                    p.Tasks.Any(t => t.AssignedUserId == userId))
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    TasksCount = p.Tasks.Count
                })
                .ToListAsync();

            return Ok(projects);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectResponseDto>> GetProject(int id)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .Include(p => p.Tasks)
                .Where(p =>
                    p.Id == id &&
                    (p.OwnerId == userId ||
                     p.Tasks.Any(t => t.AssignedUserId == userId)))
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    OwnerId = p.OwnerId,
                    TasksCount = p.Tasks.Count
                })
                .FirstOrDefaultAsync();

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<Project>> CreateProject(ProjectCreateDto dto)

        {
            var userId = GetUserId();

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = userId
            };

            _context.Projects.Add(project);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(int id, ProjectUpdateDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

            if (project == null)
            {
                return NotFound();
            }

            project.Name = dto.Name;
            project.Description = dto.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == id && p.OwnerId == userId);

            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}