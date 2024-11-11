namespace Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Body {  get; set; }
    public AppUser Author { get; set; } = null!;
    public Activity Activity { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
