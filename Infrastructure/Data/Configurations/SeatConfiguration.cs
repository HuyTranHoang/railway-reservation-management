using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(s => s.SeatType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(s => s.Compartment)
            .WithMany(c => c.Seats)
            .OnDelete(DeleteBehavior.Restrict);
    }
}