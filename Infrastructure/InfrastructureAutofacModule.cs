
using Autofac;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Infrastructure
{
    public class InfrastructureAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>  new DatabaseConfiguration<DatabaseContext>(c.Resolve<IConfiguration>()));


            // DbContext configuration
            builder.Register(c => c.Resolve<DatabaseConfiguration<DatabaseContext>>().GetDbContextOptions());

            // Manually register DbContext
            builder.Register(c =>
            {
                var context = new DatabaseContext(c.Resolve<DatabaseConfiguration<DatabaseContext>>());
                var initializer = c.Resolve<IDatabaseInitializer<DatabaseContext>>();
                initializer.EnsureDatabaseInitialization(context);

                return context;
            })
                .AsSelf()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            // Lazy DbContext
            builder.Register(c => new Lazy<DbContext>(() => c.Resolve<DatabaseContext>()));

            // Scan assembly for other registrations
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces();
        }
    }
}
