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
    // Cron.Minutely,
    "*/5 * * * *",  // Chạy cứ sau 5 phút
                    // */5: Cho biết thực hiện công việc mỗi phút thứ 5 trong một giờ (0, 5, 10, 15, v.v.).
                    // * * * *: Các dấu sao còn lại giữ nguyên các giá trị khác cho giờ, ngày, tháng và ngày trong tuần, có nghĩa là công việc sẽ chạy cứ sau 5 phút trong mọi giờ, mọi ngày.

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