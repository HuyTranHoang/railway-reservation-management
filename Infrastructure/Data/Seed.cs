using System.Reflection;
using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class Seed
{
    private const string PassengerData = "Passenger.json";
    private const string TrainCompanyData = "TrainCompany.json";
    private const string TrainData = "Train.json";
    private const string CarriageData = "Carriage.json";
    private const string SeatTypeData = "SeatType.json";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private static string BaseDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static string ProjectRoot => Directory.GetParent(BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
    private static string DataPath => Path.Combine(ProjectRoot, @"Infrastructure\Data\SeedData\");


    public static async Task SeedPassenger(ApplicationDbContext context)
    {
        if (await context.Passengers.AnyAsync()) return;

        var passengerData = await File.ReadAllTextAsync(DataPath + PassengerData);

        var passengers = JsonSerializer.Deserialize<List<Passenger>>(passengerData, JsonOptions);

        context.Passengers.AddRange(passengers);

        await context.SaveChangesAsync();
    }

    public static async Task SeedTrainCompany(ApplicationDbContext context)
    {
        if (await context.TrainCompanies.AnyAsync()) return;

        var data = await File.ReadAllTextAsync(DataPath + TrainCompanyData);

        var trainCompanies = JsonSerializer.Deserialize<List<TrainCompany>>(data, JsonOptions);

        context.TrainCompanies.AddRange(trainCompanies);

        await context.SaveChangesAsync();
    }

    public static async Task SeedTrain(ApplicationDbContext context)
    {
        if (await context.Trains.AnyAsync()) return;

        var data = await File.ReadAllTextAsync(DataPath + TrainData);

        var trains = JsonSerializer.Deserialize<List<Train>>(data, JsonOptions);

        context.Trains.AddRange(trains);

        await context.SaveChangesAsync();
    }

    public static async Task SeedCarriage(ApplicationDbContext context)
    {
        if (await context.Carriages.AnyAsync()) return;

        var data = await File.ReadAllTextAsync(DataPath + CarriageData);

        var carriages = JsonSerializer.Deserialize<List<Carriage>>(data, JsonOptions);

        context.Carriages.AddRange(carriages);

        await context.SaveChangesAsync();
    }

    public static async Task SeedSeatType(ApplicationDbContext context)
    {
        if (await context.SeatTypes.AnyAsync()) return;

        var data = await File.ReadAllTextAsync(DataPath + SeatTypeData);

        var seatTypes = JsonSerializer.Deserialize<List<SeatType>>(data, JsonOptions);

        context.SeatTypes.AddRange(seatTypes);

        await context.SaveChangesAsync();
    }
}