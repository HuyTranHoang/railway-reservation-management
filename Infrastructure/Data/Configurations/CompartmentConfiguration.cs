using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CompartmentConfiguration : IEntityTypeConfiguration<Compartment>
{
    public void Configure(EntityTypeBuilder<Compartment> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.Carriage)
            .WithMany(t => t.Compartments)
            .OnDelete(DeleteBehavior.Restrict);
    }
}