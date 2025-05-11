namespace CC.Presentation.Middlewares
{
    public class ThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Dictionary<string, List<DateTime>> RequestLog = new();
        private readonly int _maxRequestsPerMinute;
        private readonly TimeSpan _timeWindow;

        public ThrottlingMiddleware(RequestDelegate next, int maxRequestsPerMinute = 100, int timeWindowInSeconds = 60)
        {
            _next = next;
            _maxRequestsPerMinute = maxRequestsPerMinute;
            _timeWindow = TimeSpan.FromSeconds(timeWindowInSeconds);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString(); // Can also use API key or user ID
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
            requestTimes.RemoveAll(r => r < DateTime.UtcNow - _timeWindow); // Remove outdated requests

            if (requestTimes.Count >= _maxRequestsPerMinute)
            {
                context.Response.StatusCode = 429; // Too many requests
                await context.Response.WriteAsync("Rate limit exceeded");
                return;
            }

            requestTimes.Add(DateTime.UtcNow);
            await _next(context);
        }
    }

}
