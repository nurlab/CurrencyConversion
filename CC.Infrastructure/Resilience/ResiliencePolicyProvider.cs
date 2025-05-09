using CC.Application.Interfaces;
using Polly;
using Polly.Registry;

namespace CC.Infrastructure.Resilience;

/// <summary>
/// Provides access to pre-configured resilience policies from a Polly policy registry.
/// </summary>
/// <remarks>
/// This implementation of <see cref="IResiliencePolicyProvider"/> serves as an adapter
/// between the application and Polly's policy registry, enabling:
/// <list type="bullet">
///   <item><description>Centralized policy management</description></item>
///   <item><description>Loose coupling between policy consumers and policy configuration</description></item>
///   <item><description>Consistent policy access across the application</description></item>
/// </list>
/// Policies are retrieved by their registry keys and expected to be pre-configured.
/// </remarks>
public class ResiliencePolicyProvider : IResiliencePolicyProvider
{
    private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResiliencePolicyProvider"/> class.
    /// </summary>
    /// <param name="policyRegistry">The Polly policy registry containing pre-configured policies.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="policyRegistry"/> is null.</exception>
    public ResiliencePolicyProvider(IReadOnlyPolicyRegistry<string> policyRegistry)
    {
        _policyRegistry = policyRegistry ?? throw new ArgumentNullException(nameof(policyRegistry));
    }

    /// <inheritdoc/>
    public IAsyncPolicy GetHttpRetryPolicy() =>
        _policyRegistry.Get<IAsyncPolicy>("HttpRetryPolicy");

    /// <inheritdoc/>
    public IAsyncPolicy GetCircuitBreakerPolicy() =>
        _policyRegistry.Get<IAsyncPolicy>("CircuitBreakerPolicy");
}
