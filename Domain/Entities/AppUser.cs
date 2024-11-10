﻿using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser
{
    public required string DisplayName { get; set; }
    public string? Bio { get; set; }
}