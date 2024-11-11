namespace Domain.Entities;

public class UserFollowing
{
    public string ObserverId { get; set; } = null!;
    public AppUser Observer { get; set; } = null!;
    public string TargetId { get; set; } = null!;
    public AppUser Target { get; set; } = null!;
}
