namespace Library.Infrastructure.Persistence;

/// <summary>
/// Seeds the database with required roles and a default admin user.
/// Called once at application startup — safe to call repeatedly (idempotent).
/// </summary>
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<LibraryDbContext>>();

        // ── 1. Apply pending migrations automatically ─────────────────────────
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        await context.Database.MigrateAsync();
        logger.LogInformation("Database migrations applied.");

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<SystemUser>>();

        // ── 2. Seed Roles ─────────────────────────────────────────────────────
        var roles = new[] { "Administrator", "Librarian", "Staff" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
                logger.LogInformation("Role '{Role}' created.", role);
            }
        }

        // ── 3. Seed Default Admin User ────────────────────────────────────────
        var adminEmail = configuration["DefaultAdminUser:Email"];
        var adminPassword = configuration["DefaultAdminUser:Password"];

        if (!string.IsNullOrEmpty(adminEmail) && !string.IsNullOrEmpty(adminPassword))
        {
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new SystemUser
                {
                    FullName = "System Administrator",
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    Role = UserRole.Administrator
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                    logger.LogInformation("Default admin user created: {Email}", adminEmail);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    logger.LogError("Failed to create admin user: {Errors}", errors);
                }
            }
        }
    }
}