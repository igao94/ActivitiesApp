﻿using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityAttendee> ActivityAttendees { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<UserFollowing> UsersFollowings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ActivityAttendee>(k => k.HasKey(aa => new { aa.AppUserId, aa.ActivityId }));

        builder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.AppUser)
            .WithMany(u => u.Activities)
            .HasForeignKey(aa => aa.AppUserId);

        builder.Entity<ActivityAttendee>()
            .HasOne(aa => aa.Activity)
            .WithMany(a => a.Attendees)
            .HasForeignKey(aa => aa.ActivityId);

        builder.Entity<Comment>()
            .HasOne(c => c.Activity)
            .WithMany(a => a.Comments)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserFollowing>(k => k.HasKey(u => new { u.ObserverId, u.TargetId }));

        builder.Entity<UserFollowing>()
            .HasOne(o => o.Observer)
            .WithMany(u => u.Followings)
            .HasForeignKey(o => o.ObserverId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserFollowing>()
            .HasOne(t => t.Target)
            .WithMany(u => u.Followers)
            .HasForeignKey(t => t.TargetId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
