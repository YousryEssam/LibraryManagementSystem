namespace Library.API.Middlewares;

/// <summary>
/// Extension methods for registering custom middleware.
/// </summary>
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app) => app.UseMiddleware<ExceptionMiddleware>();
}
