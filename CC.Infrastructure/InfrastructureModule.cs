using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.DTOs;
using CC.Application.ExceptionHandlers;
using CC.Application.Interfaces;
using CC.Infrastructure.Resilience;
using CC.Infrastructure.Services;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Registry;

namespace CC.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 1. First register all policies and the registry
            builder.RegisterType<PolicyRegistry>()
                .AsSelf()
                .As<IReadOnlyPolicyRegistry<string>>()
                .SingleInstance();

            // Register the pre-configured policy registry
            builder.Register(_ => CreatePolicyRegistry())
                .As<PolicyRegistry>()
                .SingleInstance();

            // 2. Register HttpClient with policy handler
            builder.Register(ctx =>
            {
                var policyRegistry = ctx.Resolve<PolicyRegistry>();
                var retryPolicy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("HttpRetryPolicy");

                // Create HttpClient with Polly handler
                var handler = new PolicyHttpMessageHandler(retryPolicy)
                {
                    InnerHandler = new HttpClientHandler()
                };

                return new HttpClient(handler);
            }).As<HttpClient>().InstancePerDependency();

           builder.RegisterGeneric(typeof(FrankfurterExceptionHandler<>))
                .As(typeof(IExceptionHandler<>))
                .InstancePerLifetimeScope();
            
            builder.RegisterGeneric(typeof(ResponseContract<>))
                .As(typeof(IResponseContract<>))
                .InstancePerLifetimeScope();

            builder.RegisterType<ConversionValidator>()
                .As<IConversionValidator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FrankfurterService>()
                .As<IExchangeService>()
                .InstancePerLifetimeScope();
        }

        private static PolicyRegistry CreatePolicyRegistry()
        {
            var registry = new PolicyRegistry
        {
            { "HttpRetryPolicy", CreateHttpRetryPolicy() },
            { "CircuitBreakerPolicy", CreateCircuitBreakerPolicy() }
        };
            return registry;
        }

        private static IAsyncPolicy<HttpResponseMessage> CreateHttpRetryPolicy()
        {
            return Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                    TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                onRetry: (outcome, delay, retryCount, context) =>
                {
                    // Add logging here if needed
                });
        }

        private static IAsyncPolicy CreateCircuitBreakerPolicy()
        {
            return Policy
                .Handle<HttpRequestException>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (ex, breakDelay) =>
                {
                    // Circuit breaker opened
                },
                onReset: () =>
                {
                    // Circuit breaker reset
                });
        }

    }
}
