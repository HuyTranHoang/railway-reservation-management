using System.Reflection;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ICarriageService, CarriageService>();
        services.AddScoped<ICarriageTypeService, CarriageTypeService>();
        services.AddScoped<ICompartmentService, CompartmentService>();
        services.AddScoped<IPassengerService, PassengerService>();
        services.AddScoped<ISeatService, SeatService>();
        services.AddScoped<ISeatTypeService, SeatTypeService>();
        services.AddScoped<ITrainCompanyService, TrainCompanyService>();
        services.AddScoped<ITrainService, TrainService>();

        return services;
    }
}