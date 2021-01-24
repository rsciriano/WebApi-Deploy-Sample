using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Infrastructure;
using Microsoft.Owin.Testing;
using Infrastructure.Initializers;

namespace Api.IntegrationTests.Infrastructure.CollectionFixtures
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {         
            //Database.SetInitializer(new DropCreateDatabaseAlways());
            

            using (var context = new DatabaseContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Initiaizer.Seed(context);
                context.SaveChanges();

                var firstCinema = context.Cinemas
                    .AsNoTracking()
                    .Include(c => c.Screens)
                        .ThenInclude(s => s.Seats)
                    .First();

                var films = context.Films
                    .AsNoTracking()
                    .ToArray();

                var sessions = context.Sessions
                    .AsNoTracking()
                    .Include(s => s.Seats)
                        .ThenInclude(se => se.Ticket)
                    .ToArray();

                SeedData = new SeedData
                {
                    Cinema = firstCinema,
                    Films = films,
                    Sessions = sessions
                };
            }

            // Build the test server
            Server = TestServer.Create<Startup>();
        }

        public TestServer Server { get; }

        public SeedData SeedData { get; }

        public void Dispose()
        {
            //Database.Delete("cinematic");
            using (var context = new DatabaseContext())
            {
                //context.Database.EnsureDeleted();
            }

            Server.Dispose();
        }
    }
}
