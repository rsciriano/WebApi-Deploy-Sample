using Domain.Aggregates.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class SeatEntityConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.ToTable("Seats", "cine");

            builder.HasKey(x => new { x.ScreenId, x.Row, x.Number });

            builder.Property(x => x.Row)
                .IsRequired();

            builder.Property(x => x.Number)
                .IsRequired();
        }
    }
}