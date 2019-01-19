using System;

namespace Smooth.Algebraics.Results
{
    public struct ResultEx<TValue>
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

        public Exception Error
        {
            get
            {
                ResultHelpers.ThrowIfNotError(this);
                return _error;
            }
        }

        private readonly TValue _value;
        private readonly Exception _error;

        private ResultEx(TValue value, Exception error, bool isError)
        {
            _value = value;
            _error = error;
            IsError = isError;
        }

        public static ResultEx<TValue> FromValue(TValue value)
        {
            return new ResultEx<TValue>(value, null, false);
        }

        public static ResultEx<TValue> FromError(Exception error)
        {
            return new ResultEx<TValue>(default, error, true);
        }

        public ResultEx<TResult> Then<TResult>(Func<ResultEx<TResult>> func)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func();
        }

        public ResultEx<TResult> Then<TResult, TArg>(Func<TArg, ResultEx<TResult>> func, TArg arg)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func(arg);
        }

        public ResultEx<TResult> Then<TResult>(Func<ResultEx<TValue>, ResultEx<TResult>> func)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func(this);
        }

        public ResultEx<TResult> Then<TResult, TArg>(Func<ResultEx<TValue>, TArg, ResultEx<TResult>> func, TArg arg)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func(this, arg);
        }

        public ResultEx<TResult> Then<TResult>(Func<TValue, ResultEx<TResult>> func)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func(Value);
        }

        public ResultEx<TResult> Then<TResult, TArg>(Func<TValue, TArg, ResultEx<TResult>> func, TArg arg)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : func(Value, arg);
        }

        public ResultEx<TResult> ThenTry<TResult>(Func<TValue, TResult> func)
        {
            if (IsError)
                return ResultEx<TResult>.FromError(Error);

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
                return ResultEx<TResult>.FromError(Error);

            try
            {
                return ResultEx<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public Result<TValue> ToResult()
        {
            return IsError ? Result<TValue>.FromError(Error.Message) : Result<TValue>.FromValue(Value);
        }

        public ResultGeneric<TValue, Exception> ToResultGeneric()
        {
            return IsError ? ResultGeneric<TValue, Exception>.FromError(Error) : ResultGeneric<TValue, Exception>.FromValue(Value);
        }

        public bool Equals(ResultEx<TValue> other)
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
            return obj is ResultEx<TValue> result && Equals(result);
        }

        public override int GetHashCode()
        {
            return IsError
                ? Error.GetHashCode()
                : Collections.EqualityComparer<TValue>.Default.GetHashCode(_value);
        }

        public static bool operator ==(ResultEx<TValue> lhs, ResultEx<TValue> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ResultEx<TValue> lhs, ResultEx<TValue> rhs)
        {
            return !(lhs == rhs);
        }
    }
}