using Polly;

namespace CC.Application.Interfaces;

/// <summary>
/// Provides resilience policies for handling transient failures and systemic issues in distributed systems.
/// </summary>
public interface IResiliencePolicyProvider
{
    /// <summary>
    /// Gets a retry policy for handling transient HTTP failures.
    /// </summary>
    /// <returns>An <see cref="IAsyncPolicy"/> with exponential backoff, transient failure detection, and limited retries.</returns>
    IAsyncPolicy GetHttpRetryPolicy();

    /// <summary>
    /// Gets a circuit breaker policy for protecting against systemic failures.
    /// </summary>
    /// <returns>An <see cref="IAsyncPolicy"/> with failure threshold tracking, automatic breaking, and recovery period.</returns>
    IAsyncPolicy GetCircuitBreakerPolicy();
}
