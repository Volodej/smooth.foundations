namespace Smooth.Algebraics.Results.Exceptions
{
    public sealed class ResultInInvalidStateException : ResultException
    {
        private const string MESSAGE = "Result of type '{0}' is in invalid state. This can happen if the struct wasn't initialised properly and then used.";
        
        public static ResultInInvalidStateException From<TValue>(Result<TValue> result) =>
            new ResultInInvalidStateException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultInInvalidStateException From<TValue>(ResultEx<TValue> result) =>
            new ResultInInvalidStateException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultInInvalidStateException From<TValue, TError>(ResultGeneric<TValue, TError> result) =>
            new ResultInInvalidStateException(string.Format(MESSAGE, typeof(ResultGeneric<TValue, TError>).Name));

        private ResultInInvalidStateException(string message) : base(message)
        {
        }
    }
}