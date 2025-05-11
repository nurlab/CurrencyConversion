using CC.Application.Constants;
using Polly;
using Serilog;

namespace CC.Infrastructure.Policies;

/// <summary>
/// Configures and provides resilience policies for HTTP requests using Polly.
/// </summary>
/// <remarks>
/// This class defines:
/// <list type="bullet">
///   <item><description>Retry policy with exponential backoff and jitter</description></item>
///   <item><description>Timeout policy for HTTP requests</description></item>
///   <item><description>Logging mechanism for retry attempts</description></item>
/// </list>
/// Policies are thread-safe and can be reused across requests.
/// </remarks>
public static class PolicyConfig
{
    /// <summary>
    /// HTTP retry policy with exponential backoff and jitter.
    /// </summary>
    /// <value>
    /// An async policy that:
    /// <list type="bullet">
    ///   <item><description>Retries on HTTP request exceptions or non-success status codes</description></item>
    ///   <item><description>Uses exponential backoff (2, 4, 8 seconds) with random jitter (0-100ms)</description></item>
    ///   <item><description>Logs each retry attempt</description></item>
    ///   <item><description>Gives up after 3 attempts</description></item>
    /// </list>
    /// </value>
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
    /// <value>
    /// An async policy that times out after 15 seconds.
    /// </value>
    public static readonly IAsyncPolicy TimeoutPolicy =
        Policy.TimeoutAsync(TimeSpan.FromSeconds(15));

    /// <summary>
    /// Logs details about retry attempts.
    /// </summary>
    /// <param name="outcome">The result or exception that triggered the retry.</param>
    /// <param name="delay">The duration to wait before retrying.</param>
    /// <param name="retryCount">The current retry attempt count (1-based).</param>
    public static void LogRetryAttempt(DelegateResult<HttpResponseMessage> outcome, TimeSpan delay, int retryCount)
    {
        var message = outcome.Exception != null
            ? $"Exception: {outcome.Exception.Message}"
            : $"Status code: {(int)outcome.Result.StatusCode} {outcome.Result.StatusCode}";

        Log.Error($"[Retry Policy] Attempt {retryCount}: Retrying after {delay.TotalSeconds:F1}s. Reason: {message}");

        Log.Warning($"Retry attempt {retryCount} after {delay.TotalSeconds}s. Reason: {message}");

    }
}
