using ComputerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Controllers;

/// <summary>
/// Controller for managing users.
/// Requires authentication and admin role for most operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly ILogger<UserController> _logger;

    public UserController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ILogger<UserController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    /// <returns>List of users</returns>
    [HttpGet]
    [Authorize(Roles = "admin,super_admin")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userManager.Users
            .Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                FullName = u.full_name,
                DateOfBirth = u.date_of_birth,
                u.created_at,
                u.updated_at
            })
            .ToListAsync();
            
        return Ok(users);
    }

    /// <summary>
    /// Get user by ID (Admin or own user)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(string id)
    {
        // Check if current user is accessing their own data or is an admin
        var currentUserId = User.FindFirst("sub")?.Value;
        var isAdmin = User.IsInRole("admin") || User.IsInRole("super_admin");
        
        if (currentUserId != id && !isAdmin)
        {
            return Forbid();
        }
        
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        
        var roles = await _userManager.GetRolesAsync(user);
        
        return Ok(new
        {
            user.Id,
            user.UserName,
            user.Email,
            FullName = user.full_name,
            DateOfBirth = user.date_of_birth,
            user.created_at,
            user.updated_at,
            Roles = roles
        });
    }

    /// <summary>
    /// Update user role (Super Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="model">Role update model</param>
    /// <returns>Result of the operation</returns>
    [HttpPut("{id}/roles")]
    [Authorize(Roles = "super_admin")]
    public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UpdateRoleRequest model)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        
        // Validate role exists
        var roleExists = await _roleManager.RoleExistsAsync(model.Role);
        if (!roleExists)
        {
            return BadRequest(new { Message = "Role does not exist" });
        }
        
        // Get current roles
        var currentRoles = await _userManager.GetRolesAsync(user);
        
        // Remove existing roles
        if (currentRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
        }
        
        // Add new role
        var result = await _userManager.AddToRoleAsync(user, model.Role);
        
        if (result.Succeeded)
        {
            return Ok(new { Message = "User role updated successfully" });
        }
        
        return BadRequest(new { Message = "Failed to update user role", Errors = result.Errors });
    }

    /// <summary>
    /// Delete a user (Super Admin only)
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>Result of the operation</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "super_admin")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound(new { Message = "User not found" });
        }
        
        // Check if trying to delete super_admin
        if (await _userManager.IsInRoleAsync(user, "super_admin"))
        {
            var currentUserId = User.FindFirst("sub")?.Value;
            
            // Prevent deleting own super_admin account
            if (id == currentUserId)
            {
                return BadRequest(new { Message = "Cannot delete your own super_admin account" });
            }
            
            // Get how many super_admins are in the system
            var superAdmins = await _userManager.GetUsersInRoleAsync("super_admin");
            if (superAdmins.Count <= 1)
            {
                return BadRequest(new { Message = "Cannot delete the only super_admin account" });
            }
        }
        
        var result = await _userManager.DeleteAsync(user);
        
        if (result.Succeeded)
        {
            return Ok(new { Message = "User deleted successfully" });
        }
        
        return BadRequest(new { Message = "Failed to delete user", Errors = result.Errors });
    }
}

/// <summary>
/// Model for updating user roles
/// </summary>
public class UpdateRoleRequest
{
    /// <summary>
    /// The role name to assign to the user
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
