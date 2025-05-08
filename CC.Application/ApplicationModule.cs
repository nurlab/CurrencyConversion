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

            builder.RegisterGeneric(typeof(ResponseContract<>))
                   .As(typeof(IResponseContract<>))
                   .InstancePerLifetimeScope();

   

            builder.RegisterType<ConversionValidator>()
                   .As<IConversionValidator>()
                   .InstancePerLifetimeScope();
        }
    }
}
