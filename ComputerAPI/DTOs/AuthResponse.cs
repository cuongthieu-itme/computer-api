namespace ComputerAPI.DTOs;

/// <summary>
/// Data transfer object for authentication responses.
/// Contains token information and basic user details.
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Whether the authentication was successful
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Message describing the result of the authentication
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// JWT access token for authenticated requests
    /// </summary>
    public string? Token { get; set; }
    
    /// <summary>
    /// When the token expires
    /// </summary>
    public DateTime? Expiration { get; set; }
    
    /// <summary>
    /// Email of the authenticated user
    /// </summary>
    public string? Email { get; set; }
    
    /// <summary>
    /// Full name of the authenticated user
    /// </summary>
    public string? FullName { get; set; }
    
    /// <summary>
    /// List of roles assigned to the user
    /// </summary>
    public List<string> Roles { get; set; } = new();
}
