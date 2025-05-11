using Autofac;
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

        builder.RegisterGeneric(typeof(ResponseContract<>))
               .As(typeof(IResponseContract<>))
               .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(ResultContract<>))
               .As(typeof(IResultContract<>))
               .InstancePerLifetimeScope();
        
        builder.RegisterType<ConversionValidator>()
               .As<IConversionValidator>()
               .InstancePerLifetimeScope();

        builder.RegisterType<AccountValidator>()
               .As<IAccountValidator>()
               .InstancePerLifetimeScope();

        builder.Register(context => new MapperConfiguration(cfg =>
        {
            // Register your mappings here
            cfg.AddProfile<ApplicationMappingProfile>(); // If you have a mapping profile
        })).AsSelf().SingleInstance();

        builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
        .As<IMapper>()
        .InstancePerLifetimeScope();


    }
}
