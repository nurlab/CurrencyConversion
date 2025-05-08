using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;

namespace CC.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Frankfurter API HTTP Client
            builder.RegisterType<FrankfurterService>()
                   .As<IFrankfurterService>()
                   .WithParameter("baseUrl", "https://api.frankfurter.app")
                   .InstancePerLifetimeScope();


            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            // Register ConversionValidator directly
            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<FrankfurterService>()
               .As<IFrankfurterService>()
               .InstancePerLifetimeScope();
        }
    }
}
