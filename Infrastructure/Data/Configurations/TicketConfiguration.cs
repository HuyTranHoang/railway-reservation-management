using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasIndex(e => e.Code).IsUnique();

        builder
            .HasOne(t => t.DistanceFare)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Schedule)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Seat)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Train)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Carriage)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Payment)
            .WithMany(t => t.Tickets)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Cancellation)
            .WithOne(c => c.Ticket)
            .HasForeignKey<Cancellation>(c => c.TicketId);
    }
}