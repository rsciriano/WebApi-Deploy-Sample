using System.Data.Entity.ModelConfiguration;
using Domain.Aggregates.Sessions;
using Domain.Aggregates.Tickets;

namespace Infrastructure.Configuration
{
    internal class TicketEntityConfiguration : EntityTypeConfiguration<Ticket>
    {
        public TicketEntityConfiguration()
        {
            ToTable("Tickets", "ticket");

            HasKey(x => new { x.Id });

            this.HasRequired(x => x.SessionSeat).WithOptional(x => x.Ticket);
        }
    }
}