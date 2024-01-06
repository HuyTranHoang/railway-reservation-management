using Application.Common.Interfaces.Persistence;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DepencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ICarriageRepository, CarriageRepository>();
        services.AddScoped<ICarriageTypeRepository, CarriageTypeRepository>();
        services.AddScoped<ICompartmentRepository, CompartmentRepository>();
        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
        services.AddScoped<ITrainCompanyRepository, TrainCompanyRepository>();
        services.AddScoped<ITrainRepository, TrainRepository>();
        services.AddScoped<ICancellationRuleRepository, CancellationRuleRepository>();
        services.AddScoped<ITrainStationRepository, TrainStationRepository>();
        services.AddScoped<IRoundTripRepository, RoundTripRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        return services;
    }
}