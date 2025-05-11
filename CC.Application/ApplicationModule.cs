using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;
using CC.Domain.Contracts;
using CC.Domain.Interfaces;

namespace CC.Application;

/// <summary>
/// Autofac module for registering application-level dependencies.
/// </summary>
/// <remarks>
/// This module configures the dependency injection container with:
/// <list type="bullet">
///   <item><description>Generic response contract implementations</description></item>
///   <item><description>Validation services</description></item>
/// </list>
/// All registrations use instance-per-lifetime-scope to ensure proper
/// lifetime management within request/operation boundaries.
/// </remarks>
public class ApplicationModule : Module
{
    /// <summary>
    /// Registers application components with the Autofac container.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        // Register generic response contract implementation
        builder.RegisterGeneric(typeof(ResponseContract<>))
               .As(typeof(IResponseContract<>))
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(ResultContract<>))
               .As(typeof(IResultContract<>))
               .InstancePerLifetimeScope();

        // Register validation services
        builder.RegisterType<ConversionValidator>()
               .As<IConversionValidator>()
               .InstancePerLifetimeScope();

        builder.Register(context => new MapperConfiguration(cfg =>
        {
            // Register your mappings here
            cfg.AddProfile<ApplicationMappingProfile>(); // If you have a mapping profile
        })).AsSelf().SingleInstance();

        builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
        .As<IMapper>()
        .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IAccountService).Assembly)
        //    .Where(t => t.Name.EndsWith("Service"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IAccountValidator).Assembly)
        //    .Where(t => t.Name.EndsWith("Validator"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IConversionService).Assembly)
        //    .Where(t => t.Name.EndsWith("Service"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IConversionValidator).Assembly)
        //    .Where(t => t.Name.EndsWith("Validator"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IExceptionHandler<>).Assembly)
        //    .Where(t => t.Name.EndsWith("Handler"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IExchangeService).Assembly)
        //    .Where(t => t.Name.EndsWith("Service"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IExchangeServiceFactory).Assembly)
        //    .Where(t => t.Name.EndsWith("Factory"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IGRepository<>).Assembly)
        //    .Where(t => t.Name.EndsWith("Repository"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IResponseContract<>).Assembly)
        //    .Where(t => t.Name.EndsWith("Contract"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IResultContract<>).Assembly)
        //    .Where(t => t.Name.EndsWith("Contract"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();

        //builder.RegisterAssemblyTypes(typeof(IUserRepository).Assembly)
        //    .Where(t => t.Name.EndsWith("Repository"))
        //    .AsImplementedInterfaces()
        //    .InstancePerLifetimeScope();


    }
}
