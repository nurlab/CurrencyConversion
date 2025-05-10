using Polly;

namespace CC.Application.Interfaces;

/// <summary>
/// Provides resilience policies for handling transient failures and systemic issues in distributed systems.
/// </summary>
/// <remarks>
/// This interface abstracts the creation of Polly policies to:
/// <list type="bullet">
///   <item><description>Decouple policy configuration from consumption</description></item>
///   <item><description>Enable centralized policy management</description></item>
///   <item><description>Facilitate policy customization and testing</description></item>
/// </list>
/// Implementations should ensure policies are thread-safe and reusable.
/// </remarks>
public interface IResiliencePolicyProvider
{
    /// <summary>
    /// Gets a retry policy for handling transient HTTP failures.
    /// </summary>
    /// <returns>
    /// An <see cref="IAsyncPolicy"/> configured with:
    /// <list type="bullet">
    ///   <item><description>Exponential backoff with jitter</description></item>
    ///   <item><description>Transient failure detection</description></item>
    ///   <item><description>Limited retry attempts</description></item>
    /// </list>
    /// </returns>
    IAsyncPolicy GetHttpRetryPolicy();

    /// <summary>
    /// Gets a circuit breaker policy for protecting against systemic failures.
    /// </summary>
    /// <returns>
    /// An <see cref="IAsyncPolicy"/> configured with:
    /// <list type="bullet">
    ///   <item><description>Failure threshold tracking</description></item>
    ///   <item><description>Automatic circuit breaking</description></item>
    ///   <item><description>Recovery period</description></item>
    /// </list>
    /// </returns>
    IAsyncPolicy GetCircuitBreakerPolicy();
}
