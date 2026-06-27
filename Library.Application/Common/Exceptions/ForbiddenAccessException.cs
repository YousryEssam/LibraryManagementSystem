namespace Library.Application.Common.Exceptions;

/// <summary>
/// Thrown when the current user is authenticated but not allowed to perform
/// the requested operation (403).
/// </summary>
public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("You do not have permission to perform this action.") { }

    public ForbiddenAccessException(string message) : base(message) { }
}
