using Serilog;
using System.Diagnostics;
using System.Security.Claims;

namespace CC.Presentation.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Capture start time
            var stopwatch = Stopwatch.StartNew();

            // Get client IP
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            // Extract ClientId from JWT token (assuming "ClientId" is a claim in the JWT)
            var clientId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Get HTTP Method and Target Endpoint
            var method = context.Request.Method;
            var endpoint = context.Request.Path;

            // Continue with the request pipeline
            await _next(context);

            // Capture response code
            var responseCode = context.Response.StatusCode;

            // Stop the stopwatch to measure response time
            stopwatch.Stop();
            var responseTime = stopwatch.ElapsedMilliseconds;

            // Log the data
            Log.Information($"Client IP: {clientIp}, ClientId: {clientId}, Method: {method}, " +
                                   $"Endpoint: {endpoint}, Response Code: {responseCode}, Response Time: {responseTime}ms");
        }
    }
}
