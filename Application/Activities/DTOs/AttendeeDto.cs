﻿namespace Application.Activities.DTOs;

public class AttendeeDto
{
    public required string Username { get; set; }
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
    public string? Image { get; set; }
    public bool Following { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
}
