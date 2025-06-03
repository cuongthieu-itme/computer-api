using ComputerAPI.DTOs;
using ComputerAPI.Helpers;
using ComputerAPI.Interfaces;
using ComputerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ComputerAPI.Services;

/// <summary>
/// Implementation of the IAuthService interface.
/// Handles user registration, authentication, and token generation.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        JwtHelper jwtHelper)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtHelper = jwtHelper;
    }

    /// <summary>
    /// Register a new user and assign default role
    /// </summary>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Check if user with the same email already exists
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new AuthResponse
            {
                Success = false,
                Message = "User with this email already exists"
            };
        }

        // Create new user
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            full_name = request.FullName,
            date_of_birth = request.DateOfBirth,
            EmailConfirmed = true // For demo purposes, email is auto-confirmed
        };

        // Attempt to create the user in the database
        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (!result.Succeeded)
        {
            // Return failure with error messages
            return new AuthResponse
            {
                Success = false,
                Message = string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }

        // Assign default 'user' role
        await _userManager.AddToRoleAsync(user, "user");

        // Generate JWT token
        var (token, expiration) = await _jwtHelper.GenerateJwtToken(user);
        var roles = await _userManager.GetRolesAsync(user);

        // Return success response with token
        return new AuthResponse
        {
            Success = true,
            Message = "Registration successful",
            Token = token,
            Expiration = expiration,
            Email = user.Email,
            FullName = user.full_name,
            Roles = roles.ToList()
        };
    }

    /// <summary>
    /// Authenticate user and generate JWT token
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        // Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);
        
        // Check if user exists and password is correct
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return new AuthResponse
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        // Generate JWT token
        var (token, expiration) = await _jwtHelper.GenerateJwtToken(user);
        var roles = await _userManager.GetRolesAsync(user);

        // Return success response with token
        return new AuthResponse
        {
            Success = true,
            Message = "Login successful",
            Token = token,
            Expiration = expiration,
            Email = user.Email,
            FullName = user.full_name,
            Roles = roles.ToList()
        };
    }

    /// <summary>
    /// Get current user details by ID
    /// </summary>
    public async Task<ApplicationUser?> GetCurrentUserAsync(string userId)
    {
        return await _userManager.FindByIdAsync(userId);
    }
}
