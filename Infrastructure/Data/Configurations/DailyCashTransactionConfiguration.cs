using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class DailyCashTransactionConfiguration : IEntityTypeConfiguration<DailyCashTransaction>
    {
        public void Configure(EntityTypeBuilder<DailyCashTransaction> builder)
        {
            builder.HasQueryFilter(e => !e.IsDeleted);
        }
    }
}