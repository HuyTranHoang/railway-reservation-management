using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class Seed
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string ProjectRoot => Directory.GetParent(BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static string DataPath => Path.Combine(ProjectRoot, @"Infrastructure\Data\SeedData\");

    private const string PassengerData = "Passenger.json";

    public static async Task SeedPassenger(ApplicationDbContext context)
    {
        if (await context.Passengers.AnyAsync()) return;

        var passengerData = await File.ReadAllTextAsync(DataPath + PassengerData);

        var passengers = JsonSerializer.Deserialize<List<Passenger>>(passengerData, JsonOptions);

        context.Passengers.AddRange(passengers);

        await context.SaveChangesAsync();
    }

}