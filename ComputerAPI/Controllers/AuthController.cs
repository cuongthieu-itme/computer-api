using ComputerAPI.DTOs;
using ComputerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerAPI.Controllers;

/// <summary>
/// Controller for handling authentication operations like registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="request">Registration details</param>
    /// <returns>Authentication response with JWT token if successful</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            _logger.LogInformation("Registration attempt for {Email}", request.Email);
            
            var response = await _authService.RegisterAsync(request);
            
            if (response.Success)
            {
                _logger.LogInformation("Registration successful for {Email}", request.Email);
                return Ok(response);
            }
            
            _logger.LogWarning("Registration failed for {Email}: {Message}", request.Email, response.Message);
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new AuthResponse { Success = false, Message = "An error occurred during registration." });
        }
    }

    /// <summary>
    /// Log in an existing user
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>Authentication response with JWT token if successful</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);
            
            var response = await _authService.LoginAsync(request);
            
            if (response.Success)
            {
                _logger.LogInformation("Login successful for {Email}", request.Email);
                return Ok(response);
            }
            
            _logger.LogWarning("Login failed for {Email}", request.Email);
            return BadRequest(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}", request.Email);
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new AuthResponse { Success = false, Message = "An error occurred during login." });
        }
    }

    /// <summary>
    /// Get current user profile information
    /// </summary>
    /// <returns>User profile information</returns>
    [HttpGet("profile")]
    [Authorize]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = User.FindFirst("sub")?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }
            
            var user = await _authService.GetCurrentUserAsync(userId);
            
            if (user == null)
            {
                return Unauthorized(new { Message = "User not found" });
            }
            
            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.full_name,
                DateOfBirth = user.date_of_birth,
                Roles = User.FindAll(System.Security.Claims.ClaimTypes.Role).Select(c => c.Value).ToList()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile");
            return StatusCode(StatusCodes.Status500InternalServerError, 
                new { Message = "An error occurred while retrieving user profile." });
        }
    }
}
