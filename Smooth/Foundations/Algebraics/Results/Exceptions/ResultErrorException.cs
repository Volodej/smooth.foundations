namespace Smooth.Algebraics.Results.Exceptions
{
    public sealed class ResultErrorException : ResultException
    {
        public ResultErrorException(string message) : base(message)
        {
        }

        public static ResultErrorException FromError<TError>(TError error) =>
            new ResultErrorException($"Result has error value '{error.ToString()}'");
    }
}