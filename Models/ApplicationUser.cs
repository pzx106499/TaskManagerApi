using Microsoft.AspNetCore.Identity;

namespace TaskManagerApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();
    }
}