namespace Library.Application.Responses;


public record ApiResponse<T>(bool Success, string? Message, T? Data, IReadOnlyCollection<string>? Errors = null)
{
    public static ApiResponse<T> Ok(T data, string? message = null) => new(true, message, data);

    public static ApiResponse<T> Fail(string message, IReadOnlyCollection<string>? errors = null) =>
        new(false, message, default, errors);
}

/// <summary>Non-generic variant for endpoints with no payload (e.g. delete, register).</summary>
public record ApiResponse(bool Success, string? Message, IReadOnlyCollection<string>? Errors = null)
{
    public static ApiResponse Ok(string? message = null) => new(true, message);

    public static ApiResponse Fail(string message, IReadOnlyCollection<string>? errors = null) =>
        new(false, message, errors);
}
