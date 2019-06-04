using System;

namespace Smooth.Algebraics.Results
{
    public static class Result
    {
        public static Result<TValue> FromValue<TValue>(TValue value)
        {
            return Result<TValue>.FromValue(value);
        }

        public static Error FromError(string error) => new Error(error);
    }

    public static class ResultEx
    {
        public static ResultEx<TValue> FromValue<TValue>(TValue value)
        {
            return ResultEx<TValue>.FromValue(value);
        }

        public static ResultEx<TResult> Try<TResult>(Func<TResult> func)
        {
            try
            {
                return ResultEx<TResult>.FromValue(func());
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }

        public static ResultEx<TResult> Try<TResult, TArg>(Func<TArg, TResult> func, TArg arg)
        {
            try
            {
                return ResultEx<TResult>.FromValue(func(arg));
            }
            catch (Exception e)
            {
                return ResultEx<TResult>.FromError(e);
            }
        }
        
        public static ErrorEx FromError(Exception error) => new ErrorEx(error);
    }

    public static class ResultGeneric
    {
        public static ErrorGeneric<TError> FromError<TError>(TError error) => new ErrorGeneric<TError>(error);
    }
}