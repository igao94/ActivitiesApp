namespace Application.Comments.DTOs;

public class CommentDto
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Body { get; set; }
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Image { get; set; }
}
