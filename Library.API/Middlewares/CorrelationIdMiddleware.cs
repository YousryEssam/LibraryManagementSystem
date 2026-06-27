namespace Library.API.Middlewares;

public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // Reuse caller's ID or mint a new one
        var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var incoming)
            && !string.IsNullOrWhiteSpace(incoming)
                ? incoming.ToString()
                : Guid.NewGuid().ToString("N")[..12];   // short 12-char id

        // Make it available to downstream code via HttpContext
        context.Items["CorrelationId"] = correlationId;

        // Always echo it back to the caller
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = correlationId;
            return Task.CompletedTask;
        });

        // Push into Serilog LogContext for the duration of this request
        using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
        {
            await _next(context);
        }
    }
}
