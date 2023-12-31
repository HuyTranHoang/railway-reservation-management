
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProductExampleService, ProductExampleService>();
        return services;
    }
}