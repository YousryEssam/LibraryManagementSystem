namespace Library.Infrastructure.Repositories;

/// <summary>
/// Seeds the database with required roles and a default admin user.
/// Called once at application startup — safe to call repeatedly (idempotent).
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SystemUser>>();

        foreach (var roleName in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
        }

        var adminEmail = configuration["DefaultAdminUser:Email"]
            ?? throw new InvalidOperationException("DefaultAdminUser:Email is not configured.");
        var adminPassword = configuration["DefaultAdminUser:Password"]
            ?? throw new InvalidOperationException("DefaultAdminUser:Password is not configured.");

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
            return;

        var admin = new SystemUser
        {
            FullName = "System Administrator",
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Failed to seed admin user: {string.Join("; ", result.Errors.Select(e => e.Description))}");

        await userManager.AddToRoleAsync(admin, Roles.Administrator);
    }
}
