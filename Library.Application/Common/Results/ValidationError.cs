namespace Library.Application.Common.Results;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
