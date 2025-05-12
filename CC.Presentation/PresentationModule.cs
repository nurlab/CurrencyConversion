using AspNetCoreRateLimit;
using Autofac;
using AutoMapper;
using CC.Application.Configrations;
using CC.Application.Contracts;
using CC.Application.Contracts.Account;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Application.Services.Account;
using CC.Application.Services.Conversion;
using Microsoft.Extensions.Options;

namespace CC.Presentation
{
    /// <summary>
    /// Autofac module for registering presentation layer dependencies.
    /// </summary>
    /// <remarks>
    /// Handles dependency injection setup for:
    /// <list type="bullet">
    ///   <item><description>Response contracts via <see cref="ResponseContract{T}"/></description></item>
    ///   <item><description>Account and conversion services</description></item>
    ///   <item><description>Validation services</description></item>
    ///   <item><description>Rate limiting configuration</description></item>
    /// </list>
    /// </remarks>
    public class PresentationModule : Module
    {
        /// <summary>
        /// Loads and registers all required services for the presentation layer.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterResponseContracts(builder);
            RegisterServices(builder);
            RegisterValidators(builder);
            RegisterRateLimiting(builder);
            RegisterSettings(builder);
            RegisterMappers(builder);
        }

        /// <summary>
        /// Registers generic and specific response contracts.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterResponseContracts(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<ResponseContract<SigninResponseContract>>()
                   .As<IResponseContract<SigninResponseContract>>()
                   .InstancePerDependency();

            builder.RegisterType<ResponseContract<SignupResponseContract>>()
                   .As<IResponseContract<SignupResponseContract>>()
                   .InstancePerDependency();
        }

        /// <summary>
        /// Registers service classes.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AccountService>()
                   .As<IAccountService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ConversionService>()
                   .As<IConversionService>()
                   .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers validation classes.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterValidators(ContainerBuilder builder)
        {
            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers rate limiting configuration and counter store.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterRateLimiting(ContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheRateLimitCounterStore>()
                   .As<IRateLimitCounterStore>()
                   .SingleInstance();

            builder.RegisterType<RateLimitConfiguration>()
                   .As<IRateLimitConfiguration>()
                   .SingleInstance();
        }

        /// <summary>
        /// Registers application configuration settings.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterSettings(ContainerBuilder builder)
        {
            builder.Register(context =>
                    context.Resolve<IOptions<SecuritySettings>>().Value)
                   .As<SecuritySettings>();
        }

        /// <summary>
        /// Registers AutoMapper.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        private void RegisterMappers(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>()
                   .As<IMapper>()
                   .SingleInstance();
        }
    }
}
