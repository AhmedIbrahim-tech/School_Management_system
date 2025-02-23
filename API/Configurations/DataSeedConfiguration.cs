using Microsoft.AspNetCore.Identity;

namespace API.Configurations;

public static class DataSeedConfiguration
{
    public static async Task SeedData(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var context = services.GetRequiredService<ApplicationDBContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await RoleSeeder.SeedAsync(roleManager);
                await UserSeeder.SeedAsync(userManager);
                await DataSeeder.SeedAsync(services);

                logger.LogInformation("Database seeding completed successfully.");

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }

        }
    }

    public static async Task UpdateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDBContext>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        try
        {
            await context.Database.MigrateAsync();
            await app.SeedData();

            logger.LogInformation("Database migration and seeding completed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Occurred While Migrating Process");
        }
    }
}
