
using Autofac;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class InfrastructureAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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
