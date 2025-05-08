using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;
using StackExchange.Redis;

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
            //// Register services
            //builder.RegisterType<FrankfurterService>()
            //       .As<IExchangeRateProvider>()
            //       .InstancePerLifetimeScope();

            // Register caching (if using)
            //builder.RegisterType<RedisCacheService>()
            //       .As<ICacheService>()
            //       .SingleInstance();
            builder.Register(ctx =>
            {
                var configuration = ConfigurationOptions.Parse("localhost:6379", true);
                configuration.AbortOnConnectFail = false; // Optional: prevents startup crash if Redis isn't ready
                return ConnectionMultiplexer.Connect(configuration);
            })
            .As<IConnectionMultiplexer>()
            .SingleInstance();

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
