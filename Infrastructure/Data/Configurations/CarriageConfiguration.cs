using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CarriageConfiguration : IEntityTypeConfiguration<Carriage>
{
    public void Configure(EntityTypeBuilder<Carriage> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.CarriageType)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}