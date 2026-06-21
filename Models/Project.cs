namespace TaskManagerApi.Models
{
    public class Project
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string OwnerId { get; set; } = string.Empty;

        public ApplicationUser Owner { get; set; } = null!;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}