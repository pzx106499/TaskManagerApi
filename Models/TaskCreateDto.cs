namespace TaskManagerApi.Models
{
    public class TaskCreateDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int ProjectId { get; set; }

        public string? AssignedUserId { get; set; }
    }
}