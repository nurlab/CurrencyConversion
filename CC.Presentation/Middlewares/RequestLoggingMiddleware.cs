using Serilog;
using System.Diagnostics;
using System.Security.Claims;

namespace CC.Presentation.Middlewares
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses.
    /// </summary>
    /// <remarks>
    /// Captures details such as client IP, HTTP method, endpoint, response status code,
    /// client identifier from JWT token, and the time taken to process the request.
    /// </remarks>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">The logger instance used for logging request details.</param>
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to log request and response details.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Capture start time
            var stopwatch = Stopwatch.StartNew();

            // Get client IP address
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            // Extract ClientId from JWT token (assumes the claim is stored in NameIdentifier)
            var clientId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Capture HTTP method and target endpoint
            var method = context.Request.Method;
            var endpoint = context.Request.Path;

            // Proceed to the next middleware in the pipeline
            await _next(context);

            // Capture response status code
            var responseCode = context.Response.StatusCode;

            // Stop the stopwatch to get response time
            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            // Log request/response details
            Log.Information($"Client IP: {clientIp}, ClientId: {clientId}, Method: {method}, " +
                            $"Endpoint: {endpoint}, Response Code: {responseCode}, Response Time: {responseTime}ms");
        }
    }
}
