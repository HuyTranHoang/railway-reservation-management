using Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Infrastructure.Data;

public static class ApplicationDbContextInitialiser
{
    public static async void MigrateAndSeedDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            await context.Database.MigrateAsync();
            await Seed.SeedUsers(userManager);
            await Seed.SeedAllData(context);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "An error occurred while migrating or seeding the database");
            throw;
        }
    }
}