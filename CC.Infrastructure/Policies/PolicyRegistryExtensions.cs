using Polly;
using Polly.Registry;

namespace CC.Infrastructure.Policies;

/// <summary>
/// Provides extension methods for creating and registering Polly resilience policies.
/// </summary>
public static class PolicyRegistryExtensions
{
    /// <summary>
    /// Creates and registers a set of shared resilience policies.
    /// </summary>
    /// <returns>A <see cref="PolicyRegistry"/> with HTTP retry and circuit breaker policies.</returns>
    public static PolicyRegistry CreateSharedPolicies()
    {
        return new PolicyRegistry
        {
            { "HttpRetryPolicy", CreateHttpRetryPolicy() },
            { "CircuitBreakerPolicy", CreateCircuitBreakerPolicy() }
        };
    }

    /// <summary>
    /// Creates an HTTP retry policy with exponential backoff and jitter.
    /// </summary>
    /// <returns>A policy for retrying HTTP requests on transient failures.</returns>
    private static IAsyncPolicy<HttpResponseMessage> CreateHttpRetryPolicy()
    {
        return Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .OrResult(x => !x.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                    TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)),
                onRetry: (outcome, delay, retryCount, context) =>
                {
                    PolicyConfig.LogRetryAttempt(outcome, delay, retryCount);
                });
    }

    /// <summary>
    /// Creates a circuit breaker policy for systemic failures.
    /// </summary>
    /// <returns>A policy for breaking the circuit after repeated failures.</returns>
    private static IAsyncPolicy CreateCircuitBreakerPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1));
    }
}
