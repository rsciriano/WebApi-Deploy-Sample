using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Infrastructure;
using Infrastructure.Initializers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Xunit;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;

namespace Api.IntegrationTests.Infrastructure.CollectionFixtures
{
    public class DatabaseFixture : IDisposable, IAsyncLifetime
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
        }
        
        private IWebHost _host;

        public TestServer Server => _host.GetTestServer();

        public SeedData SeedData { get; }

        public void Dispose()
        {
            //Database.Delete("cinematic");
            using (var context = new DatabaseContext())
            {
                //context.Database.EnsureDeleted();
            }

            Server.Dispose();
            _host.Dispose();
        }

        public async Task InitializeAsync()
        {
            _host = new WebHostBuilder()
                   .UseTestServer()
                   .UseStartup<TestStartup>()
                   .Build();

            await _host.StartAsync();
        }

        public Task DisposeAsync()
        {
            // Nothing here
            return Task.CompletedTask;
        }
    }
}
