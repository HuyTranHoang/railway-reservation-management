using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.DepartureStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.ArrivalStation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(s => new { s.TrainId, s.DepartureDate, s.DepartureTime, s.ArrivalStationId, s.ArrivalDate })
            .IsUnique();
    }
}