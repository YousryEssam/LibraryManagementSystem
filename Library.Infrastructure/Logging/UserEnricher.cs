namespace Library.Infrastructure.Logging;

public sealed class UserEnricher : ILogEventEnricher
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserEnricher(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        var userName = user?.FindFirstValue(ClaimTypes.Name)
                    ?? user?.FindFirstValue("sub")
                    ?? "Anonymous";

        var userRole = user?.FindFirstValue(ClaimTypes.Role) ?? "None";

        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty("UserName", userName));

        logEvent.AddPropertyIfAbsent(
            propertyFactory.CreateProperty("UserRole", userRole));
    }
}
