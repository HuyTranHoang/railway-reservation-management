using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Data;

public static class Seed
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string ProjectRoot => Directory.GetParent(BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static string DataPath => Path.Combine(ProjectRoot, "Infrastructure", "Data", "SeedData");

    private static async Task SeedData<T>(ApplicationDbContext context, string fileName, Func<ApplicationDbContext, DbSet<T>> dbSetSelector) where T : class
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
        await SeedData<Passenger>(context, "Passenger.json", c => c.Passengers);
        await SeedData<TrainCompany>(context, "TrainCompany.json", c => c.TrainCompanies);
        await SeedData<Train>(context, "Train.json", c => c.Trains);
        await SeedData<SeatType>(context, "SeatType.json", c => c.SeatTypes);
        await SeedData<CarriageType>(context, "CarriageType.json", c => c.CarriageTypes);
        await SeedData<CancellationRule>(context, "CancellationRule.json", c => c.CancellationRules);
        await SeedData<TrainStation>(context, "TrainStation.json", c => c.TrainStations);
        await SeedData<RoundTrip>(context, "RoundTrip.json", c => c.RoundTrips);
        await SeedData<Carriage>(context, "Carriage.json", c => c.Carriages);
        await SeedData<Compartment>(context, "Compartment.json", c => c.Compartments);
        await SeedData<Seat>(context, "Seat.json", c => c.Seats);
        await SeedData<DistanceFare>(context, "DistanceFare.json", c => c.DistanceFares);

        // Chưa tạo
        // await SeedData<Ticket>(context, "Ticket.json", c => c.Tickets);
    }

    public static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync(Path.Combine(DataPath, "User.json"));
        var users = JsonSerializer.Deserialize<List<ApplicationUser>>(userData, JsonOptions);

        foreach (var user in users)
        {
            await userManager.CreateAsync(user, "Pa$$w0rd");
        }
    }
}