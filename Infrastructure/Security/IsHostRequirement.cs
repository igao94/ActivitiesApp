using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Security.Claims;

namespace Infrastructure.Security;

public class IsHostRequirement : IAuthorizationRequirement
{
}

public class IsHostRequirementHandler(DataContext dbContext,
    IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<IsHostRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        IsHostRequirement requirement)
    {
        var userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null) return Task.CompletedTask;

        var activityId = Guid.Parse(httpContextAccessor.HttpContext.Request.RouteValues
            .FirstOrDefault(k => k.Key == "id").Value!.ToString()!);

        var attendee = dbContext.ActivityAttendees
            .AsNoTracking()
            .FirstOrDefaultAsync(aa => aa.AppUserId == userId && aa.ActivityId == activityId)
            .Result;

        if (attendee is null) return Task.CompletedTask;

        if (attendee.IsHost) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
