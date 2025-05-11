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
    /// This module handles the dependency injection setup for:
    /// - Generic response contracts via <see cref="ResponseContract{T}"/> as <see cref="IResponseContract{T}"/>.
    /// - Conversion validation via <see cref="ConversionValidator"/> as <see cref="IConversionValidator"/>.
    /// All registrations are scoped to the lifetime of the request or operation.
    /// </summary>
    public class PresentationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ConversionService>()
                   .As<IConversionService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AccountService>()
                   .As<IAccountService>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<MemoryCacheRateLimitCounterStore>()
           .As<IRateLimitCounterStore>()
           .SingleInstance();  // Singleton scope

            builder.RegisterType<RateLimitConfiguration>()
                   .As<IRateLimitConfiguration>()
                   .SingleInstance();  // Singleton scope


            // Register individual dependencies
            builder.Register(context =>
                context.Resolve<IOptions<SecuritySettings>>().Value)
                .As<SecuritySettings>();

            builder.RegisterType<Mapper>() // Assuming you use AutoMapper
                .As<IMapper>()
                .SingleInstance();


            builder.RegisterType<ResponseContract<SigninResponseContract>>()
                .As<IResponseContract<SigninResponseContract>>()
                .InstancePerDependency();

            builder.RegisterType<ResponseContract<SignupResponseContract>>()
                .As<IResponseContract<SignupResponseContract>>()
                .InstancePerDependency();

            // Register AccountService
            builder.RegisterType<AccountService>()
                .As<IAccountService>()
                .InstancePerLifetimeScope();
        }
    }
}
