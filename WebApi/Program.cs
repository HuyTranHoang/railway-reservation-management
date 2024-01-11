using Application;
using Application.Services;
using Hangfire;
using Infrastructure;
using Infrastructure.Data;
using Serilog;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddWebApi(builder.Configuration);

builder.Services.AddHangfire(x => x.UseSqlServerStorage(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHangfireDashboard();

const string recurringJobId = "DailyCashTransactionJob";

var options = new RecurringJobOptions
{
    TimeZone = TimeZoneInfo.Local
};

RecurringJob.AddOrUpdate<IDailyCashTransactionService>(
    recurringJobId,
    service => service.DoWork(),
    Cron.Minutely,
    options);

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseCors(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MigrateAndSeedDatabase();

app.Run();