using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CancellationRuleConfiguration : IEntityTypeConfiguration<CancellationRule>
{
    public void Configure(EntityTypeBuilder<CancellationRule> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}