namespace CC.Presentation.Middlewares
{
    /// <summary>
    /// Middleware to enforce basic IP-based request throttling.
    /// </summary>
    /// <remarks>
    /// Limits the number of requests per client IP address over a specified time window.
    /// If the limit is exceeded, a 429 Too Many Requests status code is returned.
    /// </remarks>
    public class ThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, List<DateTime>> RequestLog = new();
        private readonly int _maxRequestsPerMinute;
        private readonly TimeSpan _timeWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="ThrottlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="maxRequestsPerMinute">Maximum allowed requests per time window per client IP.</param>
        /// <param name="timeWindowInSeconds">Time window in seconds for throttling.</param>
        public ThrottlingMiddleware(RequestDelegate next, int maxRequestsPerMinute = 100, int timeWindowInSeconds = 60)
        {
            _next = next;
            _maxRequestsPerMinute = maxRequestsPerMinute;
            _timeWindow = TimeSpan.FromSeconds(timeWindowInSeconds);
        }

        /// <summary>
        /// Invokes the middleware to evaluate request rate limits.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();
            if (clientIp == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Client IP is missing");
                return;
            }

            if (!RequestLog.ContainsKey(clientIp))
            {
                RequestLog[clientIp] = new List<DateTime>();
            }

            var requestTimes = RequestLog[clientIp];
            requestTimes.RemoveAll(r => r < DateTime.UtcNow - _timeWindow);

            if (requestTimes.Count >= _maxRequestsPerMinute)
            {
                context.Response.StatusCode = 429; // Too Many Requests
                await context.Response.WriteAsync("Rate limit exceeded");
                return;
            }

            requestTimes.Add(DateTime.UtcNow);
            await _next(context);
        }
    }
}
