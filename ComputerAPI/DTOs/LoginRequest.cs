using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ComputerAPI.DTOs;

/// <summary>
/// Data transfer object for user login requests.
/// Contains credentials needed for authentication.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Email address of the user, used as the username
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Password for the account
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}
