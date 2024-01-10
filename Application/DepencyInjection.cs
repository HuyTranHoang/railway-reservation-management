using System.Reflection;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DepencyInjection
{
  public static IServiceCollection AddApplication(this IServiceCollection services)
  {
    //Email Service
    services.AddScoped<EmailService>();

    //AutoMapper Service
    services.AddAutoMapper(Assembly.GetExecutingAssembly());

    //Persistence Services
    services.AddScoped<ICarriageService, CarriageService>();
    services.AddScoped<ICarriageTypeService, CarriageTypeService>();
    services.AddScoped<ICompartmentService, CompartmentService>();
    services.AddScoped<IPassengerService, PassengerService>();
    services.AddScoped<ISeatService, SeatService>();
    services.AddScoped<ISeatTypeService, SeatTypeService>();
    services.AddScoped<ITrainCompanyService, TrainCompanyService>();
    services.AddScoped<ITrainService, TrainService>();
    services.AddScoped<ICancellationRuleService, CancellationRuleService>();
    services.AddScoped<ITrainStationService, TrainStationService>();
    services.AddScoped<IRoundTripService, RoundTripService>();
    services.AddScoped<ITicketService, TicketService>();
    services.AddScoped<IScheduleService, ScheduleService>();
    services.AddScoped<IDistanceFareService, DistanceFareService>();
    services.AddScoped<IPaymentService, PaymentService>();
    services.AddScoped<ICancellationService, CancellationService>();
    
    // services.AddHostedService<DailyCashTransactionService>();
    return services;
  }
}