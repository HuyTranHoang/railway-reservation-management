using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RoundTripConfiguration : IEntityTypeConfiguration<RoundTrip>
{
    public void Configure(EntityTypeBuilder<RoundTrip> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.TrainCompany)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}