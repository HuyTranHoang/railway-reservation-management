using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TrainCompanyConfiguration : BaseEntityConfiguration<TrainCompany>
{
    public override void Configure(EntityTypeBuilder<TrainCompany> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.Name, "IX_TrainCompanies_Name")
            .IsUnique();
    }
}