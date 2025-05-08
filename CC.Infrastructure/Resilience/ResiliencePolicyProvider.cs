using Polly.Registry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Infrastructure.Resilience
{
    public class ResiliencePolicyProvider : IResiliencePolicyProvider
    {
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;

        public ResiliencePolicyProvider(IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            _policyRegistry = policyRegistry;
        }

        public IAsyncPolicy GetHttpRetryPolicy() =>
            _policyRegistry.Get<IAsyncPolicy>("HttpRetryPolicy");

        public IAsyncPolicy GetCircuitBreakerPolicy() =>
            _policyRegistry.Get<IAsyncPolicy>("CircuitBreakerPolicy");
    }
}
