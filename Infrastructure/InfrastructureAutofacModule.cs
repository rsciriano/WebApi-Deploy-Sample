
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
            // DbContext configuration
            builder.Register(c => c.Resolve<IConnectionProvider>().GetDbContextOptions<DatabaseContext>());

            // Manually register DbContext
            builder.Register(c => new DatabaseContext(c.Resolve<DbContextOptions<DatabaseContext>>()))
                .AsSelf()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            // Scan assembly for other registrations
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces();
        }
    }
}
