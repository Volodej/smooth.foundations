using System;

namespace Smooth.Algebraics.Results.Exceptions
{
    public abstract class ResultException : Exception
    {
        /// <inheritdoc />
        protected ResultException(string message) : base(message){}

        /// <inheritdoc />
        protected ResultException(string message, Exception innerException) : base(message, innerException){}
    }
}