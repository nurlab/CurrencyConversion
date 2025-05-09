using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.ExceptionHandlers;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;

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


        }
    }
}
