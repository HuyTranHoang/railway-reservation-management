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
        services.AddScoped<IPassengerRepository, PassengerRepository>();
        services.AddScoped<ITrainCompanyRepository, TrainCompanyRepository>();
        services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
        services.AddScoped<ITrainRepository, TrainRepository>();
        services.AddScoped<ICarriageRepository, CarriageRepository>();
        services.AddScoped<ISeatRepository, SeatRepository>();
        services.AddScoped<ICompartmentRepository, CompartmentRepository>();
        return services;
    }
}