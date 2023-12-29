
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}