namespace Library.Application.Common.Exceptions;

/// <summary>
/// Thrown when a resource conflict occurs, e.g. duplicate ISBN (409).
/// </summary>
public class ConflictException : Exception
{
    public ConflictException(string name, object value) : base($"A '{name}' with value '{value}' already exists.") { }

    public ConflictException(string message) : base(message) { }
}
