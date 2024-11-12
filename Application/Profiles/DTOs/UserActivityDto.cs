using System.Text.Json.Serialization;

namespace Application.Profiles.DTOs;

public class UserActivityDto
{
    public Guid ActivityId { get; set; }
    public required string Title { get; set; }
    public required string Category { get; set; }
    public DateTime Date { get; set; }
    [JsonIgnore]
    public string? HostUsername { get; set; }
}
