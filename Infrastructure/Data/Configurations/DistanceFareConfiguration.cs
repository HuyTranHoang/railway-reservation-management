using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class DistanceFareConfiguration : IEntityTypeConfiguration<DistanceFare>
{
    public void Configure(EntityTypeBuilder<DistanceFare> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.TrainCompany)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}