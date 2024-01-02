
using Domain.Entities;
using Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<TrainCompany> TrainCompanies { get; set; }
    public DbSet<Train> Trains { get; set; }
    public DbSet<Passenger> Passengers { get; set; }
    public DbSet<SeatType> SeatTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TrainCompanyConfiguration());
        modelBuilder.ApplyConfiguration(new TrainConfiguration());
    }
}
