using System.Reflection;
using Application.Common.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<ITrainCompanyService, TrainCompanyService>();
        services.AddScoped<IPassengerService, PassengerService>();
        services.AddScoped<ITrainService, TrainService>();
        services.AddScoped<ISeatTypeService, SeatTypeService>();
        services.AddScoped<ICarriageService, CarriageService>();
        return services;
    }
}