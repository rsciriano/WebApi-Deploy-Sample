using System;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;
using Domain.Aggregates.Cinemas;
using Domain.Aggregates.Films;
using Domain.Aggregates.Sessions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Initializers
{
    public class DemoDbInitiaizer: IDatabaseInitializer<DatabaseContext>
    {
        private readonly ILogger<DemoDbInitiaizer> logger;
        private static bool isInitialized;

        public DemoDbInitiaizer(ILogger<DemoDbInitiaizer> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public bool EnsureDatabaseInitialization(DatabaseContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (isInitialized)
            {
                return false;
            }

            InitializeDatabase(context);
            isInitialized = true;
            return true;
        }

        public void InitializeDatabase(DatabaseContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            logger.LogInformation("Trying to delete old demo database...");
            try
            {
                context.Database.EnsureDeleted();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Error trying to delete old demo database");
            }

            try
            {
                logger.LogInformation("Create new demo database...");
                context.Database.EnsureCreated();

                logger.LogInformation("Seed demo data...");
                Seed(context);
                context.SaveChanges();

                logger.LogInformation("Demo database initialized");
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex, "Error trying to initialize demo database");
                throw;
            }
        }

        public void Seed(DatabaseContext context)
        {
            var conAir = new Film("ConAir", 120);
            var batman = new Film("Batman", 95);
            var pulpFiction = new Film("Pulp fiction", 100);

            //context.Films.AddOrUpdate(c => c.Title, conAir, batman, pulpFiction);
            context.Films.AddRange(new[] { conAir, batman, pulpFiction });

            var cinema = new Cinema("Palafox");
            var screen1 = cinema.CreateScreen(name: "Aneto", rows: 5, seatsPerRow: 5);
            var screen2 = cinema.CreateScreen(name: "Monte Perdido", rows: 6, seatsPerRow: 6);
            var screen3 = cinema.CreateScreen(name: "Posets", rows: 7, seatsPerRow: 7);
            batman.Cinemas.Add(cinema);

            //context.Cinemas.AddOrUpdate(c => c.Name, cinema);
            context.Cinemas.Add(cinema);

            if (context.Sessions.Any() == false)
            {
                var sesion1 = Session.Create(screen1, conAir, new DateTime(2017, 3, 5, 18, 0, 0));
                sesion1.Publish();

                // Generate one ticket for testing
                sesion1.Seats.First().Sell(5.5m);

                var sesion2 = Session.Create(screen2, batman, new DateTime(2017, 3, 5, 19, 45, 0));
                var sesion3 = Session.Create(screen3, pulpFiction, new DateTime(2017, 3, 5, 22, 0, 0));

                context.Sessions.AddRange(new[] { sesion1, sesion2, sesion3 });
            }
        }
    }
}
