using System.Runtime.Serialization;
using PRN232.LMS.API.Models;

namespace PRN232.LMS.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var payload = new ApiResponse<object>
            {
                Success = false,
                Message = "Internal server error",
                Data = null,
                Errors = null,
            };

            var accept = context.Request.Headers.Accept.ToString();
            if (accept.Contains("application/xml", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "application/xml";
                var serializer = new DataContractSerializer(typeof(ApiResponse<object>));
                using var buffer = new MemoryStream();
                serializer.WriteObject(buffer, payload);
                buffer.Position = 0;
                await buffer.CopyToAsync(context.Response.Body, context.RequestAborted);
                return;
            }

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(payload);
        }
    }
}
