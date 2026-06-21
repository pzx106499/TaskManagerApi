namespace TaskManagerApi.Models
{
    public class TaskResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = string.Empty;

        public string? AssignedUserId { get; set; }
    }
}