using Domain.Aggregates.Sessions;
using Domain.Aggregates.Tickets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class TicketEntityConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets", "ticket");

            builder.HasKey(x => new { x.Id });

            builder.HasOne(x => x.SessionSeat)
                .WithOne(x => x.Ticket)
                .IsRequired(false);
        }
    }
}