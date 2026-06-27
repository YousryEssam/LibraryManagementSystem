namespace Library.Application.Common.Models;

/// <summary>
/// Unified API response envelope for all endpoints.
/// </summary>
public class ApiResponse<T>
{
    public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string? Message { get; init; }
    public T? Data { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }
    public PaginationMeta? Pagination { get; init; }

    private ApiResponse() { }

    // ── Success ───────────────────────────────────────────────────────────────

    public static ApiResponse<T> Ok(T data, string? message = null) => new()
    {
        Success = true,
        StatusCode = 200,
        Message = message,
        Data = data
    };

    public static ApiResponse<T> Created(T data, string? message = "Resource created successfully.") => new()
    {
        Success = true,
        StatusCode = 201,
        Message = message,
        Data = data
    };

    public static ApiResponse<T> NoContent(string? message = "Operation completed successfully.") => new()
    {
        Success = true,
        StatusCode = 204,
        Message = message
    };

    public static ApiResponse<T> Paged(T data, int page, int pageSize, int totalCount, string? message = null) => new()
    {
        Success = true,
        StatusCode = 200,
        Message = message,
        Data = data,
        Pagination = new PaginationMeta(page, pageSize, totalCount)
    };

    // ── Failure ───────────────────────────────────────────────────────────────

    public static ApiResponse<T> Fail(int statusCode, string message, IEnumerable<string>? errors = null) => new()
    {
        Success = false,
        StatusCode = statusCode,
        Message = message,
        Errors = errors?.ToList().AsReadOnly()
    };

    public static ApiResponse<T> BadRequest(string message, IEnumerable<string>? errors = null)
        => Fail(400, message, errors);

    public static ApiResponse<T> Unauthorized(string message = "Authentication is required.")
        => Fail(401, message);

    public static ApiResponse<T> Forbidden(string message = "You do not have permission to perform this action.")
        => Fail(403, message);

    public static ApiResponse<T> NotFound(string message = "The requested resource was not found.")
        => Fail(404, message);

    public static ApiResponse<T> Conflict(string message)
        => Fail(409, message);

    public static ApiResponse<T> ServerError(string message = "An unexpected error occurred.")
        => Fail(500, message);
}
