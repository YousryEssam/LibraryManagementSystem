namespace Library.Application.Common.Results;

public class Result<T> : Result
{
    private readonly T? _value;

    private Result(T value) : base(true, Error.None)
    {
        _value = value;
    }

    private Result(Error error) : base(false, error)
    {

    }

    public T Value =>
        IsSuccess ? _value! : throw new InvalidOperationException("No value for failed result.");

    public static Result<T> Success(T value) => new(value);

    public new static Result<T> Failure(Error error) => new(error);
}
