using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TrainConfiguration : IEntityTypeConfiguration<Train>
{
    public void Configure(EntityTypeBuilder<Train> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasIndex(e => e.Name, "IX_Trains_Name")
            .IsUnique();

        builder
            .HasOne(t => t.TrainCompany)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}