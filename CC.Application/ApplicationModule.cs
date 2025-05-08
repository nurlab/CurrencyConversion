using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;

namespace CC.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //// Register services
            //builder.RegisterType<FrankfurterService>()
            //       .As<IExchangeRateProvider>()
            //       .InstancePerLifetimeScope();

            // Register decorators (e.g., validation)
            //builder.RegisterDecorator<ConversionValidator, IConversionValidator>();

            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            // Register ConversionValidator directly
            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();


            //#region Contracts
            //builder.RegisterGeneric(typeof(ResponseContract<>))
            //        .As(typeof(IResponseContract<>))
            //        .InstancePerLifetimeScope();
            //#endregion
        }
    }
}
