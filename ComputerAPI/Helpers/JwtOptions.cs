namespace ComputerAPI.Helpers;

/// <summary>
/// Configuration options for JWT token generation and validation.
/// These values are loaded from appsettings.json.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// The secret key used to sign the JWT token
    /// </summary>
    public string Secret { get; set; } = string.Empty;
    
    /// <summary>
    /// The issuer of the JWT token (typically your application)
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    
    /// <summary>
    /// The audience of the JWT token (typically your API)
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    
    /// <summary>
    /// The expiration time of the JWT token in minutes
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
}
