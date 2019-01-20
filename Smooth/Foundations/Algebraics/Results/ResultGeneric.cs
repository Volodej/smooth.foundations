using System;
using Smooth.Algebraics.Results.Exceptions;
using Smooth.Collections;

namespace Smooth.Algebraics.Results
{
    public struct ResultGeneric<TValue, TError>
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

        public TError Error
        {
            get
            {
                ResultHelpers.ThrowIfNotError(this);
                return _error;
            }
        }

        private readonly TValue _value;
        private readonly TError _error;

        private ResultGeneric(TValue value, TError error, bool isError)
        {
            _value = value;
            _error = error;
            IsError = isError;
        }

        public static ResultGeneric<TValue, TError> FromValue(TValue value)
        {
            return new ResultGeneric<TValue, TError>(value, default, false);
        }

        public static ResultGeneric<TValue, TError> FromError(TError error)
        {
            return new ResultGeneric<TValue, TError>(default, error, true);
        }

        public ResultGeneric<TResult, TError> Then<TResult>(Func<TValue, TResult> func)
        {
            return IsError
                ? ResultGeneric<TResult, TError>.FromError(Error)
                : ResultGeneric<TResult, TError>.FromValue(func(Value));
        }

        public ResultGeneric<TResult, TError> Then<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg)
        {
            return IsError
                ? ResultGeneric<TResult, TError>.FromError(Error)
                : ResultGeneric<TResult, TError>.FromValue(func(Value, arg));
        }

        public ResultGeneric<TResult, TError> Then<TResult>(Func<TValue, ResultGeneric<TResult, TError>> func)
        {
            return IsError
                ? ResultGeneric<TResult, TError>.FromError(Error)
                : func(Value);
        }

        public ResultGeneric<TResult, TError> Then<TResult, TArg>(Func<TValue, TArg, ResultGeneric<TResult, TError>> func, TArg arg)
        {
            return IsError
                ? ResultGeneric<TResult, TError>.FromError(Error)
                : func(Value, arg);
        }

        public ResultEx<TResult> ThenTry<TResult>(Func<TValue, TResult> func)
        {
            if (IsError)
                return ResultEx<TResult>.FromError(ResultErrorException.FromError(Error));

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
                return ResultEx<TResult>.FromError(ResultErrorException.FromError(Error));

            try
            {
                return ResultEx<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public ResultGeneric<TResult, TError> ThenTry<TResult>(Func<TValue, TResult> func, Func<Exception, TError> catchFunc)
        {
            if (IsError)
                return ResultGeneric<TResult, TError>.FromError(Error);

            try
            {
                return ResultGeneric<TResult, TError>.FromValue(func(Value));
            }
            catch (Exception e)
            {
                return ResultGeneric<TResult, TError>.FromError(catchFunc(e));
            }
        }

        public ResultGeneric<TResult, TError> ThenTry<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg,
            Func<Exception, TArg, TError> catchFunc)
        {
            if (IsError)
                return ResultGeneric<TResult, TError>.FromError(Error);

            try
            {
                return ResultGeneric<TResult, TError>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ResultGeneric<TResult, TError>.FromError(catchFunc(e, arg));
            }
        }

        public ResultGeneric<TValue, TError> SelectIfError(Func<TError, TError> errorSelector)
        {
            return IsError ? FromError(errorSelector(Error)) : this;
        }

        public ResultGeneric<TValue, TError> SelectIfError<TArg>(Func<TError, TArg, TError> errorSelector, TArg arg)
        {
            return IsError ? FromError(errorSelector(Error, arg)) : this;
        }

        public ResultGeneric<TValue, TError> Or(ResultGeneric<TValue, TError> other)
        {
            return IsError ? other : this;
        }

        public ResultGeneric<TValue, TError> Or(Func<ResultGeneric<TValue, TError>> other)
        {
            return IsError ? other() : this;
        }

        public ResultGeneric<TValue, TError> Or<TArg>(Func<TArg, ResultGeneric<TValue, TError>> other, TArg param)
        {
            return IsError ? other(param) : this;
        }

        public TValue ValueOr(TValue elseValue)
        {
            return IsError ? elseValue : Value;
        }

        public TValue ValueOr(Func<TValue> elseValue)
        {
            return IsError ? elseValue() : Value;
        }

        public TValue ValueOr<TArg>(Func<TArg, TValue> elseValue, TArg param)
        {
            return IsError ? elseValue(param) : Value;
        }

        public Option<TValue> ToOption()
        {
            return !IsError ? Value.ToSome() : Option<TValue>.None;
        }

        public Result<TValue> ToResult(Func<TError, string> errorConvertFunc)
        {
            return IsError ? Result<TValue>.FromError(errorConvertFunc(Error)) : Result<TValue>.FromValue(Value);
        }

        public ResultEx<TValue> ToResultEx()
        {
            return IsError ? ResultEx<TValue>.FromError(ResultErrorException.FromError(Error)) : ResultEx<TValue>.FromValue(Value);
        }

        public bool Equals(ResultGeneric<TValue, TError> other)
        {
            if (IsError) return other.IsError && EqualityComparer<TError>.Default.Equals(Error, other.Error);

            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ResultGeneric<TValue, TError> result && Equals(result);
        }

        public override int GetHashCode()
        {
            return IsError
                ? Error.GetHashCode()
                : EqualityComparer<TValue>.Default.GetHashCode(_value);
        }

        public static bool operator ==(ResultGeneric<TValue, TError> lhs, ResultGeneric<TValue, TError> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ResultGeneric<TValue, TError> lhs, ResultGeneric<TValue, TError> rhs)
        {
            return !(lhs == rhs);
        }
    }
}