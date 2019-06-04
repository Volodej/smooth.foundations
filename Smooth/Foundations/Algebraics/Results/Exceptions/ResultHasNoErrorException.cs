namespace Smooth.Algebraics.Results.Exceptions
{
    public sealed class ResultHasNoErrorException : ResultException
    {
        private static string MESSAGE = "Can't get error for result of type '{0}', it has no error.";

        public static ResultHasNoErrorException From<TValue>(Result<TValue> result) =>
            new ResultHasNoErrorException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultHasNoErrorException From<TValue>(ResultEx<TValue> _) =>
            new ResultHasNoErrorException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultHasNoErrorException From<TValue, TError>(ResultGeneric<TValue, TError> _) =>
            new ResultHasNoErrorException(string.Format(MESSAGE, typeof(ResultGeneric<TValue, TError>).Name));

        private ResultHasNoErrorException(string message) : base(message)
        {
        }
    }
}