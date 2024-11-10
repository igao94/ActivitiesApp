using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Security;

public class UserAccessor(IHttpContextAccessor httpContextAccessor) : IUserAccesor
{
    public string GetUsername()
    {
        return httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;
    }
}
