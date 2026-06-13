using System.Diagnostics;

namespace PRN232.LMS.API.Middleware;

public sealed class RequestLoggingMiddleware
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
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();

            context.Request.Headers.TryGetValue("X-Request-Id", out var requestId);
            _logger.LogInformation(
                "HTTP {Method} {Path} -> {StatusCode} in {ElapsedMs}ms (X-Request-Id: {RequestId})",
                context.Request.Method,
                context.Request.Path.Value,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                requestId.ToString());
        }
    }
}
