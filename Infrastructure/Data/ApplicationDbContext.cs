using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TrainCompany> TrainCompanies { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<SeatType> SeatTypes { get; set; }
    public DbSet<Carriage> Carriages { get; set; }
    public DbSet<CarriageType> CarriageTypes { get; set; }
    public DbSet<Compartment> Compartments { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<TrainStation> TrainStations { get; set; }
    public DbSet<DistanceFare> DistanceFares { get; set; }
    public DbSet<RoundTrip> RoundTrips { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<CancellationRule> CancellationRules { get; set; }
    public DbSet<DailyCashTransaction> DailyCashTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sẽ apply hết tất cả config trong folder Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}