using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Domain;
using Domain.Aggregates.Cinemas;
using Domain.Aggregates.Films;
using Domain.Aggregates.Sessions;
using Domain.Aggregates.Tickets;
using Infrastructure.Configuration;
using System.Configuration;

namespace Infrastructure
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<Film> Films { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Ticket> Tickets { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["Cinematic"].ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CinemaEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ScreenEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SeatEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FilmEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityConfiguration());
            modelBuilder.ApplyConfiguration(new SessionSeatEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TicketEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FilmCinemaEntityConfiguration());
        }

        async Task IUnitOfWork.CommitAsync()
        {
            await SaveChangesAsync();
        }
    }
}
