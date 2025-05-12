using CC.Application.Constants;
using Polly;
using Serilog;

namespace CC.Infrastructure.Policies;

/// <summary>
/// Provides resilience policies for HTTP requests using Polly.
/// </summary>
public static class PolicyConfig
{
    /// <summary>
    /// HTTP retry policy with exponential backoff and jitter.
    /// </summary>
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
                    LogRetryAttempt(outcome, delay, retryCount);
                });

    /// <summary>
    /// Global timeout policy for HTTP requests.
    /// </summary>
    public static readonly IAsyncPolicy TimeoutPolicy =
        Policy.TimeoutAsync(TimeSpan.FromSeconds(15));

    /// <summary>
    /// Logs details about retry attempts.
    /// </summary>
    public static void LogRetryAttempt(DelegateResult<HttpResponseMessage> outcome, TimeSpan delay, int retryCount)
    {
        var message = outcome.Exception != null
            ? $"Exception: {outcome.Exception.Message}"
            : $"Status code: {(int)outcome.Result.StatusCode} {outcome.Result.StatusCode}";

        Log.Error($"[Retry Policy] Attempt {retryCount}: Retrying after {delay.TotalSeconds:F1}s. Reason: {message}");
        Log.Warning($"Retry attempt {retryCount} after {delay.TotalSeconds}s. Reason: {message}");
    }
}
