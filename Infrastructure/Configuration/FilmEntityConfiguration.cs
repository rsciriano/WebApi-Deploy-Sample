using Domain.Aggregates.Films;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class FilmEntityConfiguration : IEntityTypeConfiguration<Film>
    {
        public void Configure(EntityTypeBuilder<Film> builder)
        {
            builder.ToTable("Films", "cine");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(256)
                .IsRequired();

            builder.Property(x => x.DurationInMinutes)
                .IsRequired();

            /*
            builder.HasMany(x => x.Cinemas)
                .WithMany()
                .Map(config =>
                {
                    config.ToTable("FilmCinemas", "cine");
                });
            */
        }
    }
}