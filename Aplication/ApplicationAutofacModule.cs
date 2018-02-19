using Aplication.Pipeline;
using Autofac;

namespace Aplication
{
    public class ApplicationAutofacModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //TODO: Corredir error de MediatR behaviors en Mono
            // Manualy register behaviors
            /*builder.RegisterGeneric(typeof(TimingBehavior<,>))
                .AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(LoggingBehavior<,>))
                .AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(ValidationBehavior<,>))
                .AsImplementedInterfaces();*/

            // Scan assembly for other registrations
            var assembly = GetType().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .AsImplementedInterfaces();
        }
    }
}
