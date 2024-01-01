
using Domain.Entities;
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

}