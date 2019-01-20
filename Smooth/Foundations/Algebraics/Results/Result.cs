using System;
using Smooth.Algebraics.Results.Exceptions;
using Smooth.Collections;

// ReSharper disable UnusedMember.Global

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

        public Result<TResult> Then<TResult>(Func<TValue, TResult> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : Result<TResult>.FromValue(func(Value));
        }

        public Result<TResult> Then<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : Result<TResult>.FromValue(func(Value, arg));
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

        public Result<TValue> SpecifyError(string error)
        {
            return IsError ? FromError($"{error}\nInnerError: {Error}") : this;
        }

        public Result<TValue> SelectIfError(Func<string, string> errorSelector)
        {
            return IsError ? FromError(errorSelector(Error)) : this;
        }

        public Result<TValue> SelectIfError<TArg>(Func<string, TArg, string> errorSelector, TArg arg)
        {
            return IsError ? FromError(errorSelector(Error, arg)) : this;
        }

        public Result<TValue> Or(Result<TValue> other)
        {
            return IsError ? other : this;
        }

        public Result<TValue> Or(Func<Result<TValue>> other)
        {
            return IsError ? other() : this;
        }

        public Result<TValue> Or<TArg>(Func<TArg, Result<TValue>> other, TArg param)
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
            if (IsError) return other.IsError && other.Error == Error;

            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
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
                : EqualityComparer<TValue>.Default.GetHashCode(_value);
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