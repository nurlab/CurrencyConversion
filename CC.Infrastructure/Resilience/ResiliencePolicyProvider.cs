using CC.Application.Interfaces;
using Polly;
using Polly.Registry;

namespace CC.Infrastructure.Resilience;

/// <summary>
/// Provides access to pre-configured resilience policies from a Polly policy registry.
/// </summary>
/// <remarks>
/// This implementation of <see cref="IResiliencePolicyProvider"/> serves as an adapter
/// between the application and Polly's policy registry. It facilitates:
/// <list type="bullet">
///   <item><description>Centralized policy management, allowing resilience policies to be defined and accessed in a single location.</description></item>
///   <item><description>Loose coupling between consumers of resilience policies and their configuration, enabling easier maintenance and flexibility.</description></item>
///   <item><description>Consistent access to policies across the application, ensuring that policies are applied uniformly.</description></item>
/// </list>
/// Policies are retrieved by their registry keys and are expected to be pre-configured in the policy registry.
/// </remarks>
public class ResiliencePolicyProvider : IResiliencePolicyProvider
{
    /// <summary>
    /// The Polly policy registry that holds pre-configured resilience policies.
    /// </summary>
    private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencePolicyProvider"/> class.
    /// </summary>
    /// <param name="policyRegistry">The Polly policy registry containing pre-configured policies.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="policyRegistry"/> is null.</exception>
    public ResiliencePolicyProvider(IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _policyRegistry = policyRegistry ?? throw new ArgumentNullException(nameof(policyRegistry), "Policy registry cannot be null.");
    }

    /// <inheritdoc/>
    /// <summary>
    /// Retrieves the HTTP retry policy from the policy registry.
    /// </summary>
    /// <returns>The configured HTTP retry policy.</returns>
    public IAsyncPolicy GetHttpRetryPolicy() =>
        _policyRegistry.Get<IAsyncPolicy>("HttpRetryPolicy");

    /// <inheritdoc/>
    /// <summary>
    /// Retrieves the circuit breaker policy from the policy registry.
    /// </summary>
    /// <returns>The configured circuit breaker policy.</returns>
    public IAsyncPolicy GetCircuitBreakerPolicy() =>
        _policyRegistry.Get<IAsyncPolicy>("CircuitBreakerPolicy");
}
