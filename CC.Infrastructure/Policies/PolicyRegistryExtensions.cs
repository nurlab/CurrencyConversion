using Polly;
using Polly.Registry;

namespace CC.Infrastructure.Policies;

/// <summary>
/// Provides extension methods for creating and registering Polly resilience policies.
/// </summary>
/// <remarks>
/// This static class encapsulates the configuration of:
/// <list type="bullet">
///   <item><description>HTTP retry policy with exponential backoff and jitter</description></item>
///   <item><description>Circuit breaker policy for fault tolerance</description></item>
/// </list>
/// Policies are pre-configured with recommended defaults but can be customized as needed.
/// </remarks>
public static class PolicyRegistryExtensions
{
    /// <summary>
    /// Creates and registers a set of shared resilience policies.
    /// </summary>
    /// <returns>
    /// A <see cref="PolicyRegistry"/> containing:
    /// <list type="bullet">
    ///   <item><description>"HttpRetryPolicy" - For transient HTTP failures</description></item>
    ///   <item><description>"CircuitBreakerPolicy" - For systemic failures</description></item>
    /// </list>
    /// </returns>
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
    /// <returns>
    /// A policy that:
    /// <list type="bullet">
    ///   <item><description>Retries on HTTP failures, timeouts, or non-success status codes</description></item>
    ///   <item><description>Uses exponential backoff (2, 4, 8 seconds) with random jitter (0-100ms)</description></item>
    ///   <item><description>Logs each retry attempt</description></item>
    ///   <item><description>Gives up after 3 attempts</description></item>
    /// </list>
    /// </returns>
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
    /// <returns>
    /// A policy that:
    /// <list type="bullet">
    ///   <item><description>Breaks after 5 consecutive failures</description></item>
    ///   <item><description>Stays open for 1 minute before testing recovery</description></item>
    ///   <item><description>Triggers on HTTP exceptions or timeouts</description></item>
    /// </list>
    /// </returns>
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
