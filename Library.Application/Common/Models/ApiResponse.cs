namespace Library.Application.Common.Models;

/// <summary>
/// Convenience factory for responses that carry no typed data (delete, logout, etc.)
/// </summary>
public static class ApiResponse
{
    public static ApiResponse<object?> Ok(string? message = null)
        => ApiResponse<object?>.Ok(null, message);

    public static ApiResponse<object?> NoContent(string? message = "Operation completed successfully.")
        => ApiResponse<object?>.NoContent(message);

    public static ApiResponse<object?> BadRequest(string message, IEnumerable<string>? errors = null)
        => ApiResponse<object?>.BadRequest(message, errors);

    public static ApiResponse<object?> NotFound(string message = "The requested resource was not found.")
        => ApiResponse<object?>.NotFound(message);

    public static ApiResponse<object?> Forbidden(string message = "You do not have permission to perform this action.")
        => ApiResponse<object?>.Forbidden(message);

    public static ApiResponse<object?> Conflict(string message)
        => ApiResponse<object?>.Conflict(message);

    public static ApiResponse<object?> ServerError(string message = "An unexpected error occurred.")
        => ApiResponse<object?>.ServerError(message);
}
