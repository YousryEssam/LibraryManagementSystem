namespace Library.Application.Common.Exceptions;

/// <summary>
/// Thrown for generic bad-request scenarios not covered by validation (400).
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}
