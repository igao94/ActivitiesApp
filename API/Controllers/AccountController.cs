using API.DTOs;
using API.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(UserManager<AppUser> userManager,
    TokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            return BadRequest("Username is already taken.");

        if (await userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            return BadRequest("Email is already taken.");

        var user = new AppUser
        {
            UserName = registerDto.Username,
            DisplayName = registerDto.DisplayName,
            Email = registerDto.Email,
        };

        var result = await userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return Unauthorized();

        return Ok(CreateUserObject(user));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (user is null) return Unauthorized();

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized();

        return Ok(CreateUserObject(user));
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = await userManager.Users
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.UserName == User.FindFirstValue(ClaimTypes.Name));

        if (user is null) return NotFound();

        return Ok(CreateUserObject(user));
    }

    private UserDto CreateUserObject(AppUser user)
    {
        return new UserDto
        {
            Username = user.UserName!,
            DisplayName = user.DisplayName,
            Token = tokenService.CreateToken(user),
            Image = user.Photos?.FirstOrDefault(p => p.IsMain)?.Url
        };
    }
}
