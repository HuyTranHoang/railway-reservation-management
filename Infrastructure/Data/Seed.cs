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

    private static async Task SeedData<T>(ApplicationDbContext context, string fileName, Func<ApplicationDbContext, DbSet<T>> dbSetSelector) where T : class
    {
        if (await dbSetSelector(context).AnyAsync()) return;

        var dataPath = DataPath + fileName;
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

        // Bị lỗi vì thiếu CarriageTypeId
        // await SeedData<Carriage>(context, "Carriage.json", c => c.Carriages);


    }
}