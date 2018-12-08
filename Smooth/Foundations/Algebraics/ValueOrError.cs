using System;

namespace Smooth.Foundations.Algebraics
{

    public static class ValueOrError 
    {
        public static ValueOrError<T> FromValue<T>(T value)
        {
            return ValueOrError<T>.FromValue(value);
        }
    }

    public struct ValueOrError<T> : IEquatable<ValueOrError<T>>
    {
        public T Value
        {
            get
            {
                if (IsError)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
        }
        public bool IsError { get; }

        public string Error { get; }

        private readonly T _value;

        public static ValueOrError<T> FromValue(T value)
        {
            return new ValueOrError<T>(value, string.Empty, false);
        }

        public static ValueOrError<T> FromError(string error)
        {
            return new ValueOrError<T>(default(T), error, true);
        }

        private ValueOrError(T value, string error, bool isError)
        {
            _value = value;
            Error = error;
            IsError = isError;
        }


        public ValueOrError<TResult> ContinueWith<TResult>(Func<ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func();
        }

        public ValueOrError<TResult> ContinueWith<TResult, T1>(Func<T1, ValueOrError<TResult>> func, T1 arg)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(arg);
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<ValueOrError<T>, ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(this);
        }

        public ValueOrError<TResult> ContinueWith<TResult, T1>(Func<ValueOrError<T>, T1, ValueOrError<TResult>> func, T1 arg)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(this, arg);
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<T, ValueOrError<TResult>> func)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(Value);
        }

        public ValueOrError<TResult> ContinueWith<TResult, T1>(Func<T, T1, ValueOrError<TResult>> func, T1 arg)
        {
            return IsError
                ? ValueOrError<TResult>.FromError(Error)
                : func(Value, arg);
        }

        public ValueOrError<TResult> ContinueWith<TResult>(Func<T, TResult> func)
        {
            if (IsError)
            {
                return ValueOrError<TResult>.FromError(Error);
            }
            try
            {
                return ValueOrError<TResult>.FromValue(func(Value));
            }
            catch (Exception e)
            {
                return ValueOrError<TResult>.FromError(e.Message);
            }
        }

        public ValueOrError<TResult> ContinueWith<TResult, T1>(Func<T, T1, TResult> func, T1 arg)
        {
            if (IsError)
            {
                return ValueOrError<TResult>.FromError(Error);
            }
            try
            {
                return ValueOrError<TResult>.FromValue(func(Value, arg));
            }
            catch (Exception e)
            {
                return ValueOrError<TResult>.FromError(e.Message);
            }
        }

        public bool Equals(ValueOrError<T> other)
        {
            if (IsError)
            {
                return other.IsError && other.Error == Error;
            }
            return Collections.EqualityComparer<T>.Default.Equals(_value, other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ValueOrError<T> && Equals((ValueOrError<T>)obj);
        }

        public override int GetHashCode()
        {
            return IsError
                ? Error.GetHashCode()
                : Collections.EqualityComparer<T>.Default.GetHashCode(_value);
        }

        public static bool operator ==(ValueOrError<T> lhs, ValueOrError<T> rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ValueOrError<T> lhs, ValueOrError<T> rhs)
        {
            return !(lhs == rhs);
        }
    }
}