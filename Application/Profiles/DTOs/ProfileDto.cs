using Application.Photos.DTOs;

namespace Application.Profiles.DTOs;

public class ProfileDto
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public bool Following { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public ICollection<PhotoDto> Photos { get; set; } = [];
}
