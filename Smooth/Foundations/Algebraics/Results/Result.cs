using System;
using Smooth.Algebraics.Results.Exceptions;
using Smooth.Collections;

// ReSharper disable UnusedMember.Global

namespace Smooth.Algebraics.Results
{
    /// <summary>
    /// Struct represents the result of any work that could end not only with a valid value but also with error.
    /// <para/>This monadic type could be used for error-handling and operations chaining (railway oriented programming from F#).
    /// <para/>Has one of two states:
    /// <list type="bullet">
    ///     <item>
    ///         <description>
    ///             <see cref="Value"/> of type <see cref="TValue"/> that represents successful work completion
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <description>
    ///             <see cref="Error"/> of type <see cref="string"/> that represents error result of some work
    ///         </description>
    ///     </item>
    /// </list>   
    /// </summary>
    /// <typeparam name="TValue">Type of Result's value</typeparam>
    public struct Result<TValue> : IEquatable<Result<TValue>>
    {
        /// <summary>
        /// Value of a successful <see cref="Result"/>.
        /// <para/>Result always should be checked for for <see cref="IsError"/> is false before calling this property. 
        /// </summary>
        public TValue Value
        {
            get
            {
                ResultHelpers.ThrowIfError(this);
                return _value;
            }
        }

        /// <summary>
        /// True if <see cref="Result"/> contains error.
        /// </summary>
        public bool IsError { get; }

        /// <summary>
        /// Error value of an unsuccessful <see cref="Result"/>.
        /// <para/>Result always should be checked for <see cref="IsError"/> is true before calling this property. 
        /// </summary>
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

        /// <summary>
        /// Create successful Result from some value.
        /// </summary>
        public static Result<TValue> FromValue(TValue value)
        {
            return new Result<TValue>(value, string.Empty, false);
        }

        /// <summary>
        /// Create Result with error from error message.
        /// </summary>
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

        /// <summary>
        /// Map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// </summary>
        /// <param name="func">Map function</param>
        public Result<TResult> Then<TResult>(Func<TValue, TResult> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : Result<TResult>.FromValue(func(Value));
        }

        /// <summary>
        /// Map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain. 
        /// </summary>
        /// <param name="func">Map function</param>
        /// <param name="arg">Parameter that will be passed to map function to avoid closer allocation.</param>
        public Result<TResult> Then<TResult, TArg>(Func<TValue, TArg, TResult> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : Result<TResult>.FromValue(func(Value, arg));
        }

        /// <summary>
        /// Map Result's value to Result with value of type <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// </summary>
        /// <param name="func">Map function</param>
        public Result<TResult> Then<TResult>(Func<TValue, Result<TResult>> func)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(Value);
        }

        /// <summary>
        /// Map Result's value to Result with value of type <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain. 
        /// </summary>
        /// <param name="func">Map function</param>
        /// <param name="arg">Parameter that will be passed to map function to avoid closer allocation.</param>
        public Result<TResult> Then<TResult, TArg>(Func<TValue, TArg, Result<TResult>> func, TArg arg)
        {
            return IsError
                ? Result<TResult>.FromError(Error)
                : func(Value, arg);
        }

        /// <summary>
        /// Try to map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// <para/>If during execution of <paramref name="func"/> an exception occurs it will be captured
        /// and returned as error in <see cref="ResultEx{TResult}"/>
        /// </summary>
        /// <param name="func">Map function</param>
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

        /// <summary>
        /// Try to map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// <para/>If during execution of <paramref name="func"/> an exception occurs it will be captured
        /// and returned as error in <see cref="ResultEx{TResult}"/>
        /// </summary>
        /// <param name="func">Map function</param>
        /// <param name="arg">Parameter that will be passed to map function to avoid closer allocation.</param>
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

        /// <summary>
        /// Try to map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// <para/>If during execution of <paramref name="func"/> an exception occurs it will be captured
        /// and transformed to <see cref="string"/> value using <paramref name="catchFunc"/>.
        /// </summary>
        /// <param name="func">Map function</param>
        /// <param name="catchFunc"></param>
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

        /// <summary>
        /// Try to map Result's value to <typeparamref name="TResult"/> if Result is successful using <paramref name="func"/>.
        /// Otherwise bypass an error to the end of a chain.
        /// <para/>If during execution of <paramref name="func"/> an exception occurs it will be captured
        /// and transformed to <see cref="string"/> value using <paramref name="catchFunc"/>.
        /// </summary>
        /// <param name="func">Map function</param>
        /// <param name="catchFunc"></param>
        /// <param name="arg">Parameter that will be passed to map and catch function to avoid closer allocations.</param>
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

        /// <summary>
        /// Specifies Result's <see cref="Error"/> if Result is not successful.
        /// </summary>
        /// <param name="error">Error's clarification text. Will be added at the beginning.</param>
        public Result<TValue> SpecifyError(string error)
        {
            return IsError ? FromError($"{error}\nInnerError: {Error}") : this;
        }

        /// <summary>
        /// Specifies Result's <see cref="Error"/> if Result is not successful.
        /// </summary>
        /// <param name="errorSelector">Function to convert existing error text to more specific.</param>
        public Result<TValue> SelectIfError(Func<string, string> errorSelector)
        {
            return IsError ? FromError(errorSelector(Error)) : this;
        }

        /// <summary>
        /// Specifies Result's <see cref="Error"/> if Result is not successful.
        /// </summary>
        /// <param name="errorSelector">Function to convert existing error text to more specific.</param>
        /// <param name="arg">Parameter that will be passed to selector function to avoid closer allocation.</param>
        public Result<TValue> SelectIfError<TArg>(Func<string, TArg, string> errorSelector, TArg arg)
        {
            return IsError ? FromError(errorSelector(Error, arg)) : this;
        }

        /// <summary>
        /// Return current Result if it is successful or otherwise <paramref name="other"/> Result.
        /// </summary>
        public Result<TValue> Or(Result<TValue> other)
        {
            return IsError ? other : this;
        }

        /// <summary>
        /// Return current Result if it is successful or otherwise <paramref name="other"/> Result.
        /// </summary>
        /// <param name="other">Function to create other Result if current is not successful.</param>
        public Result<TValue> Or(Func<Result<TValue>> other)
        {
            return IsError ? other() : this;
        }

        /// <summary>
        /// Return current Result if it is successful or otherwise <paramref name="other"/> Result.
        /// </summary>
        /// <param name="other">Function to create other Result if current is not successful.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="other"/> function to avoid closer allocation.</param>
        public Result<TValue> Or<TArg>(Func<TArg, Result<TValue>> other, TArg arg)
        {
            return IsError ? other(arg) : this;
        }

        /// <summary>
        /// Return Result's value if Result is successful, otherwise return <paramref name="elseValue"/>.
        /// </summary>
        public TValue ValueOr(TValue elseValue)
        {
            return IsError ? elseValue : Value;
        }

        /// <summary>
        /// Return Result's value if Result is successful, otherwise return <paramref name="elseValue"/>.
        /// </summary>
        /// <param name="elseValue">Function to create value if current Result is not successful.</param>
        public TValue ValueOr(Func<TValue> elseValue)
        {
            return IsError ? elseValue() : Value;
        }

        /// <summary>
        /// Return Result's value if Result is successful, otherwise return <paramref name="elseValue"/>.
        /// </summary>
        /// <param name="elseValue">Function to create value if current Result is not successful.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="elseValue"/> function to avoid closer allocation.</param>
        public TValue ValueOr<TArg>(Func<TArg, TValue> elseValue, TArg arg)
        {
            return IsError ? elseValue(arg) : Value;
        }

        /// <summary>
        /// Check if value from successful Result is valid using <paramref name="predicate"/>.
        /// If no returns Result with error equals to <paramref name="errorMessage"/>.
        /// </summary>
        /// <param name="predicate">Predicate function.</param>
        /// <param name="errorMessage">Error message for Result if <paramref name="predicate"/> returns false.</param>
        public Result<TValue> Where(Func<TValue, bool> predicate, string errorMessage)
        {
            return IsError || predicate(Value)
                ? this
                : FromError($"Value didn't satisfy condition: {errorMessage}");
        }


        /// <summary>
        /// Check if value from successful Result is valid using <paramref name="predicate"/>.
        /// If no returns Result with error equals to <paramref name="errorMessage"/>.
        /// </summary>
        /// <param name="predicate">Predicate function.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="predicate"/> function to avoid closer allocation.</param>
        /// <param name="errorMessage">Error message for Result if <paramref name="predicate"/> returns false.</param>
        public Result<TValue> Where<TArg>(Func<TValue, TArg, bool> predicate, TArg arg,
            string errorMessage)
        {
            return IsError || predicate(Value, arg)
                ? this
                : FromError($"Value didn't satisfy condition: {errorMessage}");
        }

        /// <summary>
        /// Check if value from successful Result is valid using <paramref name="predicate"/>.
        /// If no returns Result with error equals to <paramref name="errorMessageFunc"/> returned value.
        /// </summary>
        /// <param name="predicate">Predicate function.</param>
        /// <param name="errorMessageFunc">Function that creates error message for Result if <paramref name="predicate"/> returns false.</param>
        public Result<TValue> Where(Func<TValue, bool> predicate,
            Func<TValue, string> errorMessageFunc)
        {
            return IsError || predicate(Value)
                ? this
                : FromError(errorMessageFunc(Value));
        }

        /// <summary>
        /// Check if value from successful Result is valid using <paramref name="predicate"/>.
        /// If no returns Result with error equals to <paramref name="errorMessageFunc"/> returned value.
        /// </summary>
        /// <param name="predicate">Predicate function.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="predicate"/>
        /// and <paramref name="errorMessageFunc"/> function to avoid closer allocations.</param>
        /// <param name="errorMessageFunc">Function that creates error message for Result if <paramref name="predicate"/> returns false.</param>
        public Result<TValue> Where<TArg>(Func<TValue, bool> predicate, TArg arg,
            Func<TValue, TArg, string> errorMessageFunc)
        {
            return IsError || predicate(Value)
                ? this
                : FromError(errorMessageFunc(Value, arg));
        }

        /// <summary>
        /// Execute action if Result is successful.
        /// </summary>
        /// <param name="action">Action that will receive Result's value.</param>
        public void IfValue(Action<TValue> action)
        {
            if (!IsError)
                action(Value);
        }

        /// <summary>
        /// Execute action if Result is successful.
        /// </summary>
        /// <param name="action">Action that will receive Result's value.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="action"/> delegate to avoid closer allocation.</param>
        public void IfValue<TArg>(Action<TValue, TArg> action, TArg arg)
        {
            if (!IsError)
                action(Value, arg);
        }

        /// <summary>
        /// Execute action if Result is not successful.
        /// </summary>
        /// <param name="action">Action that will receive Result's error.</param>
        public void IfError(Action<string> action)
        {
            if (IsError)
                action(Error);
        }

        /// <summary>
        /// Execute action if Result is not successful.
        /// </summary>
        /// <param name="action">Action that will receive Result's error.</param>
        /// <param name="arg">Parameter that will be passed to <paramref name="action"/> delegate to avoid closer allocation.</param>
        public void IfError<TArg>(Action<string, TArg> action, TArg arg)
        {
            if (IsError)
                action(Error, arg);
        }

        /// <summary>
        /// Convert Result to <see cref="Option{TValue}"/>.
        /// Convert to Some if Result is successful. If not convert to None, error value will be ignored.  
        /// </summary>
        public Option<TValue> ToOption()
        {
            return !IsError ? Value.ToSome() : Option<TValue>.None;
        }

        /// <summary>
        /// Convert to <see cref="ResultEx{TValue}"/> using <see cref="ResultErrorException"/> as value for an error.
        /// </summary>
        public ResultEx<TValue> ToResultEx()
        {
            return IsError ? ResultEx<TValue>.FromError(new ResultErrorException(Error)) : ResultEx<TValue>.FromValue(Value);
        }

        /// <summary>
        /// Convert to <see cref="ResultGeneric{TValue,TError}"/> using <see cref="string"/> type for TError. 
        /// </summary>
        /// <returns></returns>
        public ResultGeneric<TValue, string> ToResultGeneric()
        {
            return IsError ? ResultGeneric<TValue, string>.FromError(Error) : ResultGeneric<TValue, string>.FromValue(Value);
        }

        /// <summary>
        /// Check that Results has the same state and the same value for this state.
        /// </summary>
        public bool Equals(Result<TValue> other)
        {
            if (IsError) return other.IsError && other.Error == Error;

            return EqualityComparer<TValue>.Default.Equals(_value, other._value);
        }

        /// <summary>
        /// Check objects equality.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Result<TValue> result && Equals(result);
        }

        /// <summary>
        /// Get hash code for value if Result is successful using <see cref="EqualityComparer{T}"/> or hash code of value otherwise.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return IsError
                ? Error.GetHashCode()
                : EqualityComparer<TValue>.Default.GetHashCode(_value);
        }

        /// <summary>
        /// Check results equality.
        /// </summary>
        public static bool operator ==(Result<TValue> lhs, Result<TValue> rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// Check that results are not equal.
        /// </summary>
        public static bool operator !=(Result<TValue> lhs, Result<TValue> rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// Implicitly create Result with error from <see cref="Error"/>
        /// </summary>
        public static implicit operator Result<TValue>(Error error) => FromError(error.ErrorValue);
    }
}