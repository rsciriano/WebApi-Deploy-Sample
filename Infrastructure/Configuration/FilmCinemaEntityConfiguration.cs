using Domain.Aggregates.Films;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration
{
    internal class FilmCinemaEntityConfiguration : IEntityTypeConfiguration<FilmCinema>
    {
        public void Configure(EntityTypeBuilder<FilmCinema> builder)
        {
            builder.ToTable("FilmCinemas", "cine");

            builder.HasKey(x => new { x.CinemaId, x.FilmId });

            builder.HasOne(x => x.Cinema)
                .WithMany(x => x.Films)
                .HasForeignKey(x => x.FilmId);

            builder.HasOne(x => x.Film)
                .WithMany(x => x.FilmCinemas)
                .HasForeignKey(x => x.CinemaId);

        }
    }
}