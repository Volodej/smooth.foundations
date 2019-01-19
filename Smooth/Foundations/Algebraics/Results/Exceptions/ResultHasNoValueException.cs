namespace Smooth.Algebraics.Results.Exceptions
{
    public sealed class ResultHasNoValueException : ResultException
    {
        private static string MESSAGE = "Can't get value for result of type '{0}', it has no value.";

        public static ResultHasNoValueException From<TValue>(Result<TValue> result) =>
            new ResultHasNoValueException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultHasNoValueException From<TValue>(ResultEx<TValue> _) =>
            new ResultHasNoValueException(string.Format(MESSAGE, typeof(Result<TValue>).Name));

        public static ResultHasNoValueException From<TValue, TError>(ResultGeneric<TValue, TError> _) =>
            new ResultHasNoValueException(string.Format(MESSAGE, typeof(ResultGeneric<TValue, TError>).Name));

        private ResultHasNoValueException(string message) : base(message)
        {
        }
    }
}