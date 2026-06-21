namespace TaskManagerApi.Models
{
    public class ProjectUpdateDto
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}