using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.Interfaces;

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

        // Register validation services
        builder.RegisterType<ConversionValidator>()
               .As<IConversionValidator>()
               .InstancePerLifetimeScope();
    }
}
