namespace Library.Application.Common.Exceptions;

/// <summary>
/// Thrown when one or more model validation rules are violated (422).
/// </summary>
public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    /// <summary>Creates a ValidationException from FluentValidation failures.</summary>
    public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures) : base("One or more validation failures occurred.")
    {
        Errors = failures
            .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }

    /// <summary>Creates a ValidationException from a single field error.</summary>
    public ValidationException(string field, string error) : base("One or more validation failures occurred.")
    {
        Errors = new Dictionary<string, string[]>
        {
            { field, new[] { error } }
        };
    }
}
