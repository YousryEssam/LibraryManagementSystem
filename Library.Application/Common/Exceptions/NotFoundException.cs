namespace Library.Application.Common.Exceptions;

/// <summary>
/// Thrown when a requested resource is not found (404).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) : base($"'{name}' with key '{key}' was not found.") { }

    public NotFoundException(string message) : base(message) { }
}
