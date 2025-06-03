using ComputerAPI.DTOs;
using ComputerAPI.Models;

namespace ComputerAPI.Interfaces;

/// <summary>
/// Interface for authentication service.
/// Defines methods for user registration and login.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Register a new user with the provided details
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>Authentication response with result and token if successful</returns>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    
    /// <summary>
    /// Authenticate a user with email and password
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>Authentication response with token if successful</returns>
    Task<AuthResponse> LoginAsync(LoginRequest request);
    
    /// <summary>
    /// Get current user details from the JWT token
    /// </summary>
    /// <param name="userId">User ID from the token</param>
    /// <returns>User details</returns>
    Task<ApplicationUser?> GetCurrentUserAsync(string userId);
}
