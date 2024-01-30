using System.Reflection;
using System.Security.Claims;
using System.Text.Json;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class Seed
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string ProjectRoot => Directory.GetParent(BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static string DataPath => Path.Combine(ProjectRoot, "Infrastructure", "Data", "SeedData");

    private static async Task SeedData<T>(ApplicationDbContext context, string fileName,
        Func<ApplicationDbContext, DbSet<T>> dbSetSelector) where T : class
    {
        if (await dbSetSelector(context).AnyAsync()) return;

        var dataPath = Path.Combine(DataPath, fileName);
        var data = await File.ReadAllTextAsync(dataPath);
        var items = JsonSerializer.Deserialize<List<T>>(data, JsonOptions);

        dbSetSelector(context).AddRange(items);
        await context.SaveChangesAsync();
    }

    public static async Task SeedAllData(ApplicationDbContext context)
    {
        // await SeedData<Passenger>(context, "Passenger.json", c => c.Passengers);
        await SeedData<TrainCompany>(context, "TrainCompany.json", c => c.TrainCompanies);

        await SeedData<SeatType>(context, "SeatType.json", c => c.SeatTypes);
        await SeedData<CarriageType>(context, "CarriageType.json", c => c.CarriageTypes);
        await SeedData<CancellationRule>(context, "CancellationRule.json", c => c.CancellationRules);
        await SeedData<TrainStation>(context, "TrainStation.json", c => c.TrainStations);
        await SeedData<RoundTrip>(context, "RoundTrip.json", c => c.RoundTrips);
        // await SeedData<Train>(context, "Train.json", c => c.Trains);
        // await SeedData<Carriage>(context, "Carriage.json", c => c.Carriages);
        // await SeedData<Compartment>(context, "Compartment.json", c => c.Compartments);
        // await SeedData<Seat>(context, "Seat.json", c => c.Seats);
        await SeedData<DistanceFare>(context, "DistanceFare.json", c => c.DistanceFares);
        await SeedData<CompartmentTemplate>(context, "CompartmentTemplate.json", c => c.CompartmentTemplates);

        // Chưa tạo
        // await SeedData<Ticket>(context, "Ticket.json", c => c.Tickets);
    }

    public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var superAdmin = new ApplicationUser
        {
            FirstName = "Super",
            LastName = "Admin",
            Email = SD.SuperAdminEmail,
            UserName = SD.SuperAdminEmail,
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(superAdmin, "123456");
        await userManager.AddToRolesAsync(superAdmin, new[] { SD.SuperAdminRole, SD.AdminRole, SD.UserRole });
        await userManager.AddClaimsAsync(superAdmin, new Claim[]
        {
            new(ClaimTypes.Email, superAdmin.Email),
            new(ClaimTypes.Surname, superAdmin.LastName),
        });

        var admin = new ApplicationUser
        {
            FirstName = "Admin",
            LastName = "RailTicketHub",
            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(admin, "123456");
        await userManager.AddToRolesAsync(admin, new[] { SD.AdminRole });
        await userManager.AddClaimsAsync(admin, new Claim[]
        {
            new(ClaimTypes.Email, admin.Email),
            new(ClaimTypes.Surname, admin.LastName),
        });

        var user = new ApplicationUser
        {
            FirstName = "User",
            LastName = "RailTicketHub",
            Email = "user@gmail.com",
            UserName = "user@gmail.com",
            EmailConfirmed = true,
        };

        await userManager.CreateAsync(user, "123456");
        await userManager.AddToRolesAsync(user, new[] { SD.UserRole });
        await userManager.AddClaimsAsync(user, new Claim[]
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Surname, user.LastName),
        });
    }

    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (await roleManager.Roles.AnyAsync()) return;

        var roles = new List<IdentityRole>
        {
            new() { Name = SD.SuperAdminRole },
            new() { Name = SD.AdminRole },
            new() { Name = SD.UserRole }
        };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }
}