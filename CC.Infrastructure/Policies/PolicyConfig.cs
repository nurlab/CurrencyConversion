using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Infrastructure.Policies
{
    public static class PolicyConfig
    {
        public static readonly IAsyncPolicy<HttpResponseMessage> HttpRetryPolicy =
            Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .OrResult(x => !x.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                        TimeSpan.FromMilliseconds(Random.Shared.Next(0, 100)), // Add jitter
                    onRetry: (outcome, delay, retryCount, context) =>
                    {
                        // Use proper logging here
                        LogRetryAttempt(outcome, delay, retryCount);
                    });

        public static readonly IAsyncPolicy TimeoutPolicy =
            Policy.TimeoutAsync(TimeSpan.FromSeconds(15));

        public static void LogRetryAttempt(DelegateResult<HttpResponseMessage> outcome, TimeSpan delay, int retryCount)
        {
            // Implement proper logging
            //var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger("Polly");
            //logger.LogWarning($"Retry {retryCount} after {delay.TotalSeconds} seconds due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
        }
    }
}
