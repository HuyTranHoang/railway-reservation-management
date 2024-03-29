using Application;
using Hangfire;
using Infrastructure;
using Infrastructure.Data;
using Serilog;
using WebApi.Controllers;
using WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddWebApi(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHangfire(x => x.UseSqlServerStorage(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddDistributedMemoryCache();

builder.Host.UseSerilog();

builder.Services.AddSignalR();

builder.Services.AddSession();

var app = builder.Build();

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
    "*/10 * * * *",  // Chạy cứ sau 10 phút
                     // */10: Cho biết thực hiện công việc mỗi phút thứ 10 trong một giờ (0, 5, 10, 15, v.v.).
                     // * * * *: Các dấu sao còn lại giữ nguyên các giá trị khác cho giờ, ngày, tháng và ngày trong tuần, có nghĩa là công việc sẽ chạy cứ sau 5 phút trong mọi giờ, mọi ngày.

    options);

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseCors(b => b.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200",
        "http://localhost:4300", "https://localhost:4300"));

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PaymentHub>("/paymentHub");
    endpoints.MapControllers();
});


app.MapControllers();

app.MigrateAndSeedDatabase();

app.Run();