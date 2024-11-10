using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services;

public class TokenService(IConfiguration config)
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"]
            ?? throw new Exception("Can't access token key.");

        if (tokenKey.Length < 64) throw new Exception("Token key needs to be longer.");

        if (user is null || user.UserName is null || user.Email is null)
            throw new Exception("Can't find user.");

        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email)
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
