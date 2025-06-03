using ComputerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComputerAPI.Data;

/// <summary>
/// Database context for the application, extends IdentityDbContext with ApplicationUser and ApplicationRole.
/// Configures the entity models and their relationships.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// Configure the model and relationships using Fluent API
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure table names with snake_case convention
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("users");
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("roles");
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("user_roles");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("user_claims");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("user_logins");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("role_claims");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("user_tokens");
        });
    }

    /// <summary>
    /// Seeds default roles and admin user to the database
    /// </summary>
    public static async Task SeedDefaultDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Create default roles if they don't exist
        string[] roleNames = { "user", "admin", "super_admin" };
        
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var role = new ApplicationRole(roleName)
                {
                    description = $"Default {roleName} role"
                };
                await roleManager.CreateAsync(role);
            }
        }

        // Create a default super admin user if not exists
        var adminUser = await userManager.FindByEmailAsync("admin@example.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                EmailConfirmed = true,
                full_name = "System Administrator"
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin@123456");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "super_admin");
            }
        }
    }
}
