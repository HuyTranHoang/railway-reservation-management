using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TrainStationConfiguration : IEntityTypeConfiguration<TrainStation>
{
    public void Configure(EntityTypeBuilder<TrainStation> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}