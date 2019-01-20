using Smooth.Algebraics.Results.Exceptions;

namespace Smooth.Algebraics.Results
{
    internal static class ResultHelpers
    {
        public static void ThrowIfError<TValue>(Result<TValue> result)
        {
            if (result.IsError)
                throw ResultHasNoValueException.From(result);
        }

        public static void ThrowIfError<TValue>(ResultEx<TValue> result)
        {
            if (result.IsError)
                throw ResultHasNoValueException.From(result);
        }

        public static void ThrowIfError<TValue, TError>(ResultGeneric<TValue, TError> result)
        {
            if (result.IsError)
                throw ResultHasNoValueException.From(result);
        }

        public static void ThrowIfNotError<TValue>(Result<TValue> result)
        {
            if (!result.IsError)
                throw ResultHasNoErrorException.From(result);
        }

        public static void ThrowIfNotError<TValue>(ResultEx<TValue> result)
        {
            if (!result.IsError)
                throw ResultHasNoErrorException.From(result);
        }

        public static void ThrowIfNotError<TValue, TError>(ResultGeneric<TValue, TError> result)
        {
            if (!result.IsError)
                throw ResultHasNoErrorException.From(result);
        }
    }
}