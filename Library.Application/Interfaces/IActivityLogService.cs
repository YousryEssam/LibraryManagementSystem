namespace Library.Application.Interfaces;

public interface IActivityLogService
{
    Task LogAsync(
        int userId,
        string action,
        string? entityName = null,
        int? entityId = null,
        string? details = null,
        string? ipAddress = null,
        CancellationToken ct = default);
}
