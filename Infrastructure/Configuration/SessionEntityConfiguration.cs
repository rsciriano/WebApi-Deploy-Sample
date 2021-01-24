using Domain.Aggregates.Sessions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("Sessions", "session");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Start)
                .IsRequired();

            builder.HasOne(x => x.Screen)
                .WithMany()
                .IsRequired();

            builder.HasOne(x => x.Film)
                .WithMany()
                .IsRequired();

            builder.HasMany(x => x.Seats)
                .WithOne(x => x.Session)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}