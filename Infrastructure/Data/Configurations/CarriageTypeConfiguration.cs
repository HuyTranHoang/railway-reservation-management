using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CarriageTypeConfiguration : IEntityTypeConfiguration<CarriageType>
{
    public void Configure(EntityTypeBuilder<CarriageType> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}