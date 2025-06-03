using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ComputerAPI.DTOs;

/// <summary>
/// Data transfer object for user registration requests.
/// Contains all necessary fields for creating a new user account.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Email address of the user, used as the username
    /// </summary>
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Password for the new account
    /// </summary>
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Confirmation of the password to ensure it's typed correctly
    /// </summary>
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [JsonPropertyName("confirm_password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    /// <summary>
    /// Full name of the user
    /// </summary>
    [Required(ErrorMessage = "Full name is required")]
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Date of birth of the user (optional)
    /// </summary>
    [JsonPropertyName("date_of_birth")]
    public DateTime? DateOfBirth { get; set; }
}
