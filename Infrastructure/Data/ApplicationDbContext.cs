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
    public DbSet<Carriage> Carriages { get; set; }
    public DbSet<Compartment> Compartments { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<TrainStation> TrainStations { get; set; }
    public DbSet<DistanceFare> DistanceFares { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TrainCompanyConfiguration());
        modelBuilder.ApplyConfiguration(new TrainConfiguration());
        modelBuilder.Entity<Train>()
            .HasOne(t => t.TrainCompany)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Carriage>()
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Compartment>()
            .HasOne(t => t.Carriage)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<DistanceFare>()
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<DistanceFare>()
            .HasOne(t => t.DepartureStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<DistanceFare>()
            .HasOne(t => t.ArrivalStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Schedule>()
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Schedule>()
            .HasOne(t => t.DepartureStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Schedule>()
            .HasOne(t => t.ArrivalStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);


        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.DistanceFare)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Schedule)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Seat)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Carriage)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(t => t.Passenger)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(t => t.Ticket)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cancellation>()
            .HasOne(t => t.Ticket)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

    }
}