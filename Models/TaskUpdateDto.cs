namespace TaskManagerApi.Models
{
    public class TaskUpdateDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public string? AssignedUserId { get; set; }
    }
}