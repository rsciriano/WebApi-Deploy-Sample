using System;
using System.Reflection;
using System.Threading.Tasks;
using Acheve.AspNetCore.TestHost.Security;
using Acheve.TestHost;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Api.IntegrationTests
{
    public class TestStartup
    {
        public ILifetimeScope AutofacContainer { get; private set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(TestServerDefaults.AuthenticationScheme)
                .AddTestServer(options =>
                {
                    options.Events = new TestServerEvents
                    {
                        OnMessageReceived = context => Task.CompletedTask,
                        OnTokenValidated = context => Task.CompletedTask,
                        OnAuthenticationFailed = context => Task.CompletedTask,
                        OnChallenge = context => Task.CompletedTask
                    };
                })
                .AddTestServer("Bearer", options =>
                 {
                     options.NameClaimType = "name";
                     options.RoleClaimType = "role";
                     options.Events = new TestServerEvents
                     {
                         OnMessageReceived = context => Task.CompletedTask,
                         OnTokenValidated = context => Task.CompletedTask,
                         OnAuthenticationFailed = context => Task.CompletedTask,
                         OnChallenge = context => Task.CompletedTask
                     };
                 });

            services.AddControllers()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddApplicationPart(Assembly.Load(new AssemblyName("Api")));

            services.AddApiVersioning();

            services.AddDbContext<DatabaseContext>(
                options =>
                {
                    options.UseSqlServer("name=ConnectionStrings:Cinematic");
                }
            );

            ApiConfiguration.Configure(services);


            services.AddLogging();

            // Create a container-builder and register dependencies
            var builder = new ContainerBuilder();

            // Populate the service-descriptors added to `IServiceCollection`
            // BEFORE you add things to Autofac so that the Autofac
            // registrations can override stuff in the `IServiceCollection`
            // as needed
            builder.Populate(services);

            // Register your own things directly with Autofac
            builder.RegisterModule(new ApiAutofacModule
            {
                SampleConfiguration = true
            });
            builder.RegisterModule<InfrastructureAutofacModule>();

            AutofacContainer = builder.Build();

            // this will be used as the service-provider for the application!
            return new AutofacServiceProvider(AutofacContainer);

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
