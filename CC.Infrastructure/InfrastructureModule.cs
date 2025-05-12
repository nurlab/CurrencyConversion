using Autofac;
using CC.Application.Contracts;
using CC.Application.Decorators;
using CC.Application.ExceptionHandlers;
using CC.Application.Interfaces;
using CC.Domain.Interfaces;
using CC.Infrastructure.Factory;
using CC.Infrastructure.Repositories.AccountRepository;
using CC.Infrastructure.Services.Conversion;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Registry;
using Serilog;

namespace CC.Infrastructure;

/// <summary>
/// Autofac module for configuring infrastructure components including:
/// <list type="bullet">
///   <item><description>HTTP client with resilience policies</description></item>
///   <item><description>Policy registry and policies</description></item>
///   <item><description>Exception handling infrastructure</description></item>
///   <item><description>Core service implementations</description></item>
/// </list>
/// </summary>
public class InfrastructureModule : Module
{
    /// <summary>
    /// Registers infrastructure components with the Autofac container.
    /// </summary>
    /// <param name="builder">The container builder.</param>
    protected override void Load(ContainerBuilder builder)
    {
        RegisterPolicyRegistry(builder);
        RegisterCoreInfrastructure(builder);
        RegisterServiceImplementationsAndFactories(builder);
        RegisterCrossCuttingConcerns(builder);
    }

    #region Policy Registry & Resilience Configuration

    /// <summary>
    /// Registers the policy registry and the HTTP client with resilience policies.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    private void RegisterPolicyRegistry(ContainerBuilder builder)
    {
        builder.RegisterType<PolicyRegistry>()
            .AsSelf()
            .As<IReadOnlyPolicyRegistry<string>>()
            .SingleInstance();

        builder.Register(_ => CreatePolicyRegistry())
            .As<PolicyRegistry>()
            .SingleInstance();

        builder.Register(ctx =>
        {
            var policyRegistry = ctx.Resolve<PolicyRegistry>();
            var retryPolicy = policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("HttpRetryPolicy");

            var handler = new PolicyHttpMessageHandler(retryPolicy)
            {
                InnerHandler = new HttpClientHandler()
            };

            return new HttpClient(handler);
        }).As<HttpClient>().InstancePerDependency();
    }

    #endregion

    #region Core Infrastructure

    /// <summary>
    /// Registers core infrastructure components such as UnitOfWork and UserRepository.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    private void RegisterCoreInfrastructure(ContainerBuilder builder)
    {
        builder.RegisterType<CC.Infrastructure.UnitOfWork.UnitOfWork>()
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();

        builder.RegisterType<UserRepository>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();
    }

    #endregion

    #region Service Implementations & Factories

    /// <summary>
    /// Registers service implementations and factories, including the conversion validator and exchange service.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    private void RegisterServiceImplementationsAndFactories(ContainerBuilder builder)
    {
        builder.RegisterType<ConversionValidator>()
            .As<IConversionValidator>()
            .InstancePerLifetimeScope();

        builder.RegisterType<ExchangeServiceFactory>()
            .As<IExchangeServiceFactory>()
            .InstancePerLifetimeScope();

        builder.RegisterType<FrankfurterProvider>()
            .As<IExchangeService>()
            .InstancePerLifetimeScope();
    }

    #endregion

    #region Cross-cutting Concerns (Exception Handling, Contracts)

    /// <summary>
    /// Registers exception handling and response contract services.
    /// </summary>
    /// <param name="builder">The Autofac container builder.</param>
    private void RegisterCrossCuttingConcerns(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(FrankfurterExceptionHandler<>))
            .As(typeof(IExceptionHandler<>))
            .InstancePerLifetimeScope();

        builder.RegisterGeneric(typeof(ResponseContract<>))
            .As(typeof(IResponseContract<>))
            .InstancePerLifetimeScope();
    }

    #endregion

    #region Resilience Policies

    /// <summary>
    /// Creates and configures the policy registry with resilience policies.
    /// </summary>
    private static PolicyRegistry CreatePolicyRegistry()
    {
        var registry = new PolicyRegistry
        {
            { "HttpRetryPolicy", CreateHttpRetryPolicy() },
            { "CircuitBreakerPolicy", CreateCircuitBreakerPolicy() }
        };

        return registry;
    }

    /// <summary>
    /// Creates an HTTP retry policy with exponential backoff and jitter.
    /// </summary>
    private static IAsyncPolicy<HttpResponseMessage> CreateHttpRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                3,
                retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                    TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                onRetry: (outcome, delay, retryCount, context) =>
                {
                    Log.Information(
                        "Retry attempt {RetryCount} after {Delay}ms for {Operation}. Error: {ErrorMessage}",
                        retryCount,
                        delay.TotalMilliseconds,
                        context.OperationKey,
                        outcome.Exception?.Message
                    );
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy for systemic failure protection.
    /// </summary>
    private static IAsyncPolicy CreateCircuitBreakerPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1),
                onBreak: (exception, breakDelay) =>
                {
                    Log.Error(
                        "Circuit breaker opened! Will remain open for {BreakDuration}s. Reason: {Error}",
                        breakDelay.TotalSeconds,
                        exception?.Message
                    );
                },
                onReset: () =>
                {
                    Log.Information("Circuit breaker reset - requests are allowed again");
                },
                onHalfOpen: () =>
                {
                    Log.Warning("Circuit breaker half-open - testing next request");
                });
    }

    #endregion
}
