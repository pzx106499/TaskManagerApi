using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponseDto>>> GetTasks()
        {
            var userId = GetUserId();

            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Where(t =>
                    t.AssignedUserId == userId ||
                    t.Project.OwnerId == userId)
                .Select(t => new TaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    ProjectId = t.ProjectId,
                    ProjectName = t.Project.Name,
                    AssignedUserId = t.AssignedUserId
                })
                .ToListAsync();

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTask(TaskCreateDto dto)
        {
            var userId = GetUserId();

            var project = await _context.Projects
                .FirstOrDefaultAsync(p =>
                    p.Id == dto.ProjectId &&
                    p.OwnerId == userId);

            if (project == null)
                return NotFound("Project not found");

            if (!string.IsNullOrWhiteSpace(dto.AssignedUserId))
            {
                var assignedUser = await _userManager.FindByIdAsync(dto.AssignedUserId);

                if (assignedUser == null)
                    return BadRequest("Assigned user not found");
                }

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                AssignedUserId = dto.AssignedUserId,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                task.Id,
                task.Title,
                task.Description,
                task.IsCompleted,
                task.ProjectId,
                task.AssignedUserId
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDto dto)
        {
            var userId = GetUserId();

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.Project.OwnerId == userId);

            if (task == null)
                return NotFound();

            if (!string.IsNullOrWhiteSpace(dto.AssignedUserId))
            {
                var assignedUserExists = await _userManager.Users
                    .AnyAsync(u => u.Id == dto.AssignedUserId);

                if (!assignedUserExists)
                    return BadRequest("Assigned user not found");
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;
            task.AssignedUserId = dto.AssignedUserId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserId();

            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.Project.OwnerId == userId);

            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}