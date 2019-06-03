using System;
using Smooth.Algebraics.Results.Exceptions;
using Smooth.Collections;

// ReSharper disable UnusedMember.Global

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

        public ResultEx<TResult> Then<TResult>(Func<TValue, TResult> func)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : ResultEx<TResult>.FromValue(func(Value));
        }

        public ResultEx<TResult> Then<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg)
        {
            return IsError
                ? ResultEx<TResult>.FromError(Error)
                : ResultEx<TResult>.FromValue(func(Value, arg));
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

        public ResultEx<TValue> SelectIfError(Func<Exception, Exception> errorSelector)
        {
            return IsError ? FromError(errorSelector(Error)) : this;
        }

        public ResultEx<TValue> SelectIfError<TArg>(Func<Exception, TArg, Exception> errorSelector, TArg arg)
        {
            return IsError ? FromError(errorSelector(Error, arg)) : this;
        }

        public ResultEx<TValue> Or(ResultEx<TValue> other)
        {
            return IsError ? other : this;
        }

        public ResultEx<TValue> Or(Func<ResultEx<TValue>> other)
        {
            return IsError ? other() : this;
        }

        public ResultEx<TValue> Or<TArg>(Func<TArg, ResultEx<TValue>> other, TArg param)
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

        public ResultEx<TValue> Where(Func<TValue, bool> predicate, string errorMessage)
        {
            return IsError || predicate(Value)
                ? this
                : FromError(new ResultValueDidNotSatisfyConditionException<TValue>(errorMessage, Value));
        }

        public ResultEx<TValue> Where<TParam>(Func<TValue, TParam, bool> predicate, TParam param,
            string errorMessage)
        {
            return IsError || predicate(Value, param)
                ? this
                : FromError(new ResultValueDidNotSatisfyConditionException<TValue>(errorMessage, Value));
        }

        public ResultEx<TValue> Where(Func<TValue, bool> predicate,
            Func<TValue, string> errorMessageFunc)
        {
            return IsError || predicate(Value)
                ? this
                : FromError(new ResultValueDidNotSatisfyConditionException<TValue>(errorMessageFunc(Value), Value));
        }

        public ResultEx<TValue> Where<TParam>(Func<TValue, bool> predicate, TParam param,
            Func<TValue, TParam, string> errorMessageFunc)
        {
            return IsError || predicate(Value)
                ? this
                : FromError(new ResultValueDidNotSatisfyConditionException<TValue>(errorMessageFunc(Value, param), Value));
        }

        public void IfValue(Action<TValue> action)
        {
            if (!IsError)
                action(Value);
        }

        public void IfValue<TP>(Action<TValue, TP> action, TP param)
        {
            if (!IsError)
                action(Value, param);
        }

        public void IfError(Action<Exception> action)
        {
            if (IsError)
                action(Error);
        }

        public void IfError<TParam>(Action<Exception, TParam> action, TParam param)
        {
            if (IsError)
                action(Error, param);
        }

        public Option<TValue> ToOption()
        {
            return !IsError ? Value.ToSome() : Option<TValue>.None;
        }

        public Result<TValue> ToResult()
        {
            return IsError ? Result<TValue>.FromError(Error.Message) : Result<TValue>.FromValue(Value);
        }

        public Result<TValue> ToResult(Func<Exception, string> errorSelector)
        {
            return IsError ? Result<TValue>.FromError(errorSelector(Error)) : Result<TValue>.FromValue(Value);
        }

        public ResultGeneric<TValue, Exception> ToResultGeneric()
        {
            return IsError ? ResultGeneric<TValue, Exception>.FromError(Error) : ResultGeneric<TValue, Exception>.FromValue(Value);
        }

        public bool Equals(ResultEx<TValue> other)
        {
            if (IsError) return other.IsError && other.Error == Error;

            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
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
                : EqualityComparer<TValue>.Default.GetHashCode(_value);
        }

        public static bool operator ==(ResultEx<TValue> lhs, ResultEx<TValue> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ResultEx<TValue> lhs, ResultEx<TValue> rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator ResultEx<TValue>(Error error) => FromError(ResultErrorException.FromError(error.ErrorValue));

        public static implicit operator ResultEx<TValue>(ErrorEx error) => FromError(error.ErrorValue);
    }
}