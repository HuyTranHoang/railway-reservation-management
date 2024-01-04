using Domain.Exceptions;

namespace WebApi.Extensions;

public static class WebApiServiceExtensions
{
    public static IServiceCollection AddWebApi(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors();

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage);

                var errorResponse = new ValidateInputError(400, errors);

                return new BadRequestObjectResult(errorResponse);
            };
        });

        return services;
    }
}