using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder
            .HasOne(t => t.Passenger)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Ticket)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}