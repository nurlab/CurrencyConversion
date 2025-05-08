using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;

namespace CC.Presentation
{
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            // Register ConversionValidator directly
            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();
            // Register ConversionValidator directly
            builder.RegisterType<FrankfurterService>()
                   .As<IFrankfurterService>()
                   .InstancePerLifetimeScope();

        }
    }
}
