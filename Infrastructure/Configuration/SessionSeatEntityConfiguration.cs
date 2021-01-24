using Domain.Aggregates.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class SessionSeatEntityConfiguration : IEntityTypeConfiguration<SessionSeat>
    {
        public void Configure(EntityTypeBuilder<SessionSeat> builder)
        {
            builder.ToTable("SessionSeats", "session");

            builder.HasKey(x => new { x.SessionId, x.SeatRow, x.SeatNumber });
        }
    }
}