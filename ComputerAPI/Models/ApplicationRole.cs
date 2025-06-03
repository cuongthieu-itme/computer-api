using Microsoft.AspNetCore.Identity;

namespace ComputerAPI.Models;

/// <summary>
/// Custom role class that extends IdentityRole.
/// Adds additional metadata fields like description and creation date.
/// </summary>
public class ApplicationRole : IdentityRole
{
    /// <summary>
    /// Description of the role and its permissions
    /// </summary>
    public string? description { get; set; }
    
    /// <summary>
    /// Creation date of the role
    /// </summary>
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Last update date of the role
    /// </summary>
    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
