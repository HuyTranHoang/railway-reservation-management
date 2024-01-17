using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class CompartmentTemplateConfiguration : IEntityTypeConfiguration<CompartmentTemplate>
{
    public void Configure(EntityTypeBuilder<CompartmentTemplate> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}