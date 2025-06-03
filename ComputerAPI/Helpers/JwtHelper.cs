using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ComputerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ComputerAPI.Helpers;

/// <summary>
/// Helper class for JWT token generation and validation.
/// </summary>
public class JwtHelper
{
    private readonly JwtOptions _jwtOptions;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtHelper(IOptions<JwtOptions> jwtOptions, UserManager<ApplicationUser> userManager)
    {
        _jwtOptions = jwtOptions.Value;
        _userManager = userManager;
    }

    /// <summary>
    /// Generates a JWT token for the specified user
    /// </summary>
    /// <param name="user">The user to generate a token for</param>
    /// <returns>A JWT token string and its expiration date</returns>
    public async Task<(string token, DateTime expiration)> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        
        // Set token expiration time
        var expiration = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);
        
        // Create JWT token claims
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim("full_name", user.full_name ?? string.Empty)
        };
        
        // Add role claims
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Create signing credentials
        var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );
        
        // Create security token
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );
        
        // Return token string and expiration
        return (new JwtSecurityTokenHandler().WriteToken(token), expiration);
    }
}
