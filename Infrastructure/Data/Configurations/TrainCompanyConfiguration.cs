using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TrainCompanyConfiguration : IEntityTypeConfiguration<TrainCompany>
{
    public void Configure(EntityTypeBuilder<TrainCompany> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}