using Microsoft.AspNetCore.Identity;

namespace ComputerAPI.Models;

/// <summary>
/// Extended user model that inherits from IdentityUser.
/// Adds additional fields like full_name and date_of_birth.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// The full name of the user
    /// </summary>
    public string? full_name { get; set; }
    
    /// <summary>
    /// The date of birth of the user
    /// </summary>
    public DateTime? date_of_birth { get; set; }
    
    /// <summary>
    /// Creation date of the user account
    /// </summary>
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Last update date of the user account
    /// </summary>
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
}
