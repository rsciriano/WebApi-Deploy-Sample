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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Api.IntegrationTests.Infrastructure.CollectionFixtures
{
    public class DatabaseFixture : IDisposable, IAsyncLifetime
    {
        public DatabaseFixture()
        {

        }

        private IWebHost _host;

        public TestServer Server => _host.GetTestServer();

        public SeedData SeedData { get; private set; }

        public void Dispose()
        {
            using (var scope = _host.Services.CreateScope())
            {
                //Database.Delete("cinematic");
                using (var context = _host.Services.GetRequiredService<DatabaseContext>())
                {
                    //context.Database.EnsureDeleted();
                }
            }

            Server.Dispose();
            _host.Dispose();
        }

        public async Task InitializeAsync()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();


            _host = new WebHostBuilder()
                .UseTestServer()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseConfiguration(configuration)
                .UseStartup<TestStartup>()
                .Build();

            var log = _host.Services.GetRequiredService<ILogger<DatabaseFixture>>();

            using (var context = _host.Services.GetRequiredService<DatabaseContext>())
            {
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


            await _host.StartAsync();
        }

        public Task DisposeAsync()
        {
            // Nothing here
            return Task.CompletedTask;
        }
    }
}
