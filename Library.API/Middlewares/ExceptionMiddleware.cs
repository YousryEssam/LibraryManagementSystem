namespace Library.API.Middlewares;

/// <summary>
/// Global exception-handling middleware that catches all unhandled exceptions
/// and maps them to structured ProblemDetails responses.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, errors) = MapException(exception);

        // Log accordingly
        if (statusCode >= 500)
            _logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        else
            _logger.LogWarning(exception, "Handled exception: {Message}", exception.Message);

        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Instance = context.Request.Path,
        };

        // Attach validation errors if present
        if (errors is { Count: > 0 })
            problem.Extensions["errors"] = errors;

        // Expose stack trace only in Development
        if (_env.IsDevelopment())
            problem.Extensions["stackTrace"] = exception.StackTrace;

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem, _jsonOptions));
    }

    /// <summary>
    /// Maps a known exception type to (HTTP status, title, validation errors).
    /// </summary>
    private static (int StatusCode, string Title, Dictionary<string, string[]>? Errors)
        MapException(Exception exception)
    {
        return exception switch
        {
            // ── Application layer exceptions ──────────────────────────────────
            ValidationException ve => (
                (int)HttpStatusCode.UnprocessableEntity,
                "Validation Failed",
                ve.Errors),

            NotFoundException ne => (
                (int)HttpStatusCode.NotFound,
                ne.Message,
                null),

            ForbiddenAccessException => (
                (int)HttpStatusCode.Forbidden,
                "Access Denied",
                null),

            UnauthorizedAccessException => (
                (int)HttpStatusCode.Unauthorized,
                "Unauthorized",
                null),

            ConflictException ce => (
                (int)HttpStatusCode.Conflict,
                ce.Message,
                null),

            BadRequestException be => (
                (int)HttpStatusCode.BadRequest,
                be.Message,
                null),

            // ── Infrastructure / unexpected ───────────────────────────────────
            OperationCanceledException => (
                499,   // Client Closed Request (nginx convention)
                "Request Cancelled",
                null),

            _ => (
                (int)HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.",
                null)
        };
    }
}
