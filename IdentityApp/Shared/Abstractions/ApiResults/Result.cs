using IdentityApp.Shared.Abstractions.Errors;

namespace IdentityApp.Shared.Abstractions.ApiResults
{
    public class Result
    {
        /// <summary>
        /// Represents the result of an operation, which can either be a success or a failure.
        /// </summary>
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        public static Result Success()
            => new(true, Error.None);
        public static Result Failure(Error error)
            => new(false, error);

        public static Result<TValue> Success<TValue>(TValue value)
            => new(value, true, Error.None);

        public static Result<TValue> Failure<TValue>(Error error)
            => new(default, false, error);
    }

    /// <summary>
    /// Represents the result of an operation that returns a value, which can either be a success or a failure.
    /// </summary>
    /// <typeparam name="TValue">The type of the value returned by the operation.</typeparam>
    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Failure cannot have a value.");

        public static implicit operator Result<TValue>(TValue? value)
            => value is not null ? Success(value) : Failure<TValue>(Error.None);

        public static implicit operator Result<TValue>(Error error)
            => Failure<TValue>(error);
    }
}
