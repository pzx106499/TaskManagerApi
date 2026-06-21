namespace TaskManagerApi.Models
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string OwnerId { get; set; } = string.Empty;

        public int TasksCount { get; set; }
    }
}