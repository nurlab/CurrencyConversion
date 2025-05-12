using Autofac;
using AutoMapper;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Domain.Contracts;

namespace CC.Application
{
    /// <summary>
    /// Autofac module for registering application-level dependencies.
    /// </summary>
    /// <remarks>
    /// This module configures the dependency injection container with:
    /// <list type="bullet">
    ///   <item><description>Generic response and result contract implementations</description></item>
    ///   <item><description>Domain validation services</description></item>
    ///   <item><description>AutoMapper configuration and services</description></item>
    /// </list>
    /// All registrations use instance-per-lifetime-scope or singleton as appropriate to ensure proper
    /// lifecycle management across application requests.
    /// </remarks>
    public class ApplicationModule : Module
    {
        /// <summary>
        /// Registers application components with the Autofac container.
        /// </summary>
        /// <param name="builder">The Autofac container builder used to register components.</param>
        /// <remarks>
        /// This method delegates to helper methods to register:
        /// <list type="bullet">
        ///   <item><description>Generic implementations for <see cref="IResponseContract{T}"/> and <see cref="IResultContract{T}"/></description></item>
        ///   <item><description>Validation services like <see cref="IConversionValidator"/> and <see cref="IAccountValidator"/></description></item>
        ///   <item><description>AutoMapper configuration and registration of <see cref="IMapper"/></description></item>
        /// </list>
        /// </remarks>
        protected override void Load(ContainerBuilder builder)
        {
            RegisterResponseAndResultContracts(builder);
            RegisterValidationServices(builder);
            RegisterAutoMapper(builder);
        }

        /// <summary>
        /// Registers generic implementations for response and result contracts.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        /// <remarks>
        /// Registers <see cref="ResponseContract{T}"/> and <see cref="ResultContract{T}"/> as implementations
        /// of <see cref="IResponseContract{T}"/> and <see cref="IResultContract{T}"/> respectively, using scoped lifetime.
        /// </remarks>
        private void RegisterResponseAndResultContracts(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ResultContract<>))
                   .As(typeof(IResultContract<>))
                   .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers domain validation services with the container.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        /// <remarks>
        /// Registers <see cref="ConversionValidator"/> and <see cref="AccountValidator"/>
        /// as implementations of their respective interfaces, using scoped lifetime.
        /// </remarks>
        private void RegisterValidationServices(ContainerBuilder builder)
        {
            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<AccountValidator>()
                   .As<IAccountValidator>()
                   .InstancePerLifetimeScope();
        }

        /// <summary>
        /// Registers AutoMapper configuration and services.
        /// </summary>
        /// <param name="builder">The Autofac container builder.</param>
        /// <remarks>
        /// Registers <see cref="MapperConfiguration"/> as a singleton and <see cref="IMapper"/> as a scoped service.
        /// The AutoMapper profile <see cref="ApplicationMappingProfile"/> is applied during configuration.
        /// </remarks>
        private void RegisterAutoMapper(ContainerBuilder builder)
        {
            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ApplicationMappingProfile>();
            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                   .As<IMapper>()
                   .InstancePerLifetimeScope();
        }
    }
}
