using System;
using Smooth.Algebraics.Results.Exceptions;

namespace Smooth.Algebraics.Results
{
    public struct Result<TValue>
    {
        public TValue Value
        {
            get
            {
                ResultHelpers.ThrowIfError(this);
                return _value;
            }
        }

        public bool IsError { get; }

        public string Error
        {
            get
            {
                ResultHelpers.ThrowIfNotError(this);
                return _error;
            }
        }

        private readonly TValue _value;
        private readonly string _error;

        public static Result<TValue> FromValue(TValue value)
        {
            return new Result<TValue>(value, string.Empty, false);
        }

        public static Result<TValue> FromError(string error)
        {
            return new Result<TValue>(default, error, true);
        }

        private Result(TValue value, string error, bool isError)
        {
            _value = value;
            _error = error;
            IsError = isError;
        }


        public Result<TResult> Then<TResult>(Func<Result<TResult>> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func();
        }

        public Result<TResult> Then<TResult, TArg>(Func<TArg, Result<TResult>> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(arg);
        }

        public Result<TResult> Then<TResult>(Func<Result<TValue>, Result<TResult>> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(this);
        }

        public Result<TResult> Then<TResult, TArg>(Func<Result<TValue>, TArg, Result<TResult>> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(this, arg);
        }

        public Result<TResult> Then<TResult>(Func<TValue, Result<TResult>> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(Value);
        }

        public Result<TResult> Then<TResult, TArg>(Func<TValue, TArg, Result<TResult>> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(Value, arg);
        }

        public ResultEx<TResult> ThenTry<TResult>(Func<TValue, TResult> func)
        {
            if (IsError)
                return ResultEx<TResult>.FromError(new ResultErrorException(Error));

            try
            {
                return ResultEx<TResult>.FromValue(func(Value));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public ResultEx<TResult> ThenTry<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg)
        {
            if (IsError)
                return ResultEx<TResult>.FromError(new ResultErrorException(Error));

            try
            {
                return ResultEx<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public Result<TResult> ThenTry<TResult>(Func<TValue, TResult> func, Func<Exception, string> catchFunc)
        {
            if (IsError)
                return Result<TResult>.FromError(Error);

            try
            {
                return Result<TResult>.FromValue(func(Value));
            }
            catch (Exception e)
            {
                return Result<TResult>.FromError(catchFunc(e));
            }
        }

        public Result<TResult> ThenTry<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg, Func<Exception, TArg, string> catchFunc)
        {
            if (IsError)
                return Result<TResult>.FromError(Error);

            try
            {
                return Result<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return Result<TResult>.FromError(catchFunc(e, arg));
            }
        }

        public ResultEx<TValue> ToResultEx()
        {
            return IsError ? ResultEx<TValue>.FromError(new ResultErrorException(Error)) : ResultEx<TValue>.FromValue(Value);
        }

        public ResultGeneric<TValue, string> ToResultGeneric()
        {
            return IsError ? ResultGeneric<TValue, string>.FromError(Error) : ResultGeneric<TValue, string>.FromValue(Value);
        }

        public bool Equals(Result<TValue> other)
        {
            if (IsError)
            {
                return other.IsError && other.Error == Error;
            }

            return Collections.EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Result<TValue> result && Equals(result);
        }

        public override int GetHashCode()
        {
            return IsError
                ? Error.GetHashCode()
                : Collections.EqualityComparer<TValue>.Default.GetHashCode(_value);
        }

        public static bool operator ==(Result<TValue> lhs, Result<TValue> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Result<TValue> lhs, Result<TValue> rhs)
        {
            return !(lhs == rhs);
        }
    }
}