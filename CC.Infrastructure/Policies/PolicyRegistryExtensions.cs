using Polly.Registry;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Infrastructure.Policies
{
    // PolicyRegistryExtensions.cs
    public static class PolicyRegistryExtensions
    {
        public static PolicyRegistry CreateSharedPolicies()
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
                .Or<TaskCanceledException>()
                .OrResult(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                        TimeSpan.FromMilliseconds(new Random().Next(0, 100)),
                    onRetry: (outcome, delay, retryCount, context) =>
                    {
                        PolicyConfig.LogRetryAttempt(outcome, delay, retryCount);
                    });
        }

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
}
