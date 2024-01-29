using System.Text;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Services;
using Domain.Constants;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DepencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Data Service
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        // Identity Service
        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;

            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddRoles<IdentityRole>() // Be able to add role to user
        .AddRoleManager<RoleManager<IdentityRole>>() // Be able to make use of RoleManager
        .AddEntityFrameworkStores<ApplicationDbContext>() // Be able to use EF to store user
        .AddSignInManager<SignInManager<ApplicationUser>>() // Be able to use SignInManager to create users
        .AddUserManager<UserManager<ApplicationUser>>() // Make use of UserManager to manage users
        .AddDefaultTokenProviders(); // Be able to generate token for email confirmation

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false
                };
            });

        services.AddScoped<JwtService>();

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole(SD.SuperAdminRole));
            opt.AddPolicy("AdminPolicy", policy => policy.RequireRole(SD.AdminRole));
            opt.AddPolicy("UserPolicy", policy => policy.RequireRole(SD.UserRole));

            opt.AddPolicy("SuperAdminOrAdminPolicy", policy => policy.RequireRole(SD.SuperAdminRole, SD.AdminRole));
        });
        // Persistence Service
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
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IScheduleRepository, ScheduleRepository>();
        services.AddScoped<IDistanceFareRepository, DistanceFareRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<ICancellationRepository, CancellationRepository>();
        services.AddScoped<IDailyCashTransactionRepository, DailyCashTransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(TemplateRepository<>));
        return services;
    }
}