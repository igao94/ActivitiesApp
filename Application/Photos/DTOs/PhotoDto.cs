namespace Application.Photos.DTOs;

public class PhotoDto
{
    public required string Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
}
