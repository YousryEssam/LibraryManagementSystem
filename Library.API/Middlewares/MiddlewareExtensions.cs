namespace Library.API.Middlewares;

/// <summary>
/// Extension methods for registering custom middleware.
/// </summary>
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();


    public static IApplicationBuilder UseCorrelationId(
        this IApplicationBuilder app) =>
        app.UseMiddleware<CorrelationIdMiddleware>();


    public static IApplicationBuilder UseStructuredRequestLogging(
        this IApplicationBuilder app) =>
        app.UseSerilogRequestLogging(options =>
        {
            // Concise message template shown in the console
            options.MessageTemplate =
                "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

            // Add extra properties to every request log entry
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value ?? string.Empty);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
                diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
                diagnosticContext.Set("CorrelationId", httpContext.Items["CorrelationId"]?.ToString() ?? string.Empty);

                if (httpContext.User.Identity?.IsAuthenticated == true)
                    diagnosticContext.Set("UserName", httpContext.User.Identity.Name ?? "unknown");
            };

            // Suppress noisy health-check / swagger hits; escalate errors
            options.GetLevel = (httpContext, elapsed, ex) =>
            {
                if (ex is not null || httpContext.Response.StatusCode >= 500)
                    return Serilog.Events.LogEventLevel.Error;

                if (httpContext.Response.StatusCode >= 400)
                    return Serilog.Events.LogEventLevel.Warning;

                var path = httpContext.Request.Path.Value ?? string.Empty;
                if (path.StartsWith("/health", StringComparison.OrdinalIgnoreCase) ||
                    path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
                    return Serilog.Events.LogEventLevel.Debug;

                return Serilog.Events.LogEventLevel.Information;
            };
        });
}
