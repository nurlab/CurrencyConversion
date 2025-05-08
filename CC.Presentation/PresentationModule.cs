using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.ExceptionHandlers;
using CC.Application.Interfaces;
using CC.Infrastructure.Services;

namespace CC.Presentation
{
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
