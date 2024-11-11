namespace Domain.Entities;

public class Photo
{
    public required string Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
}
