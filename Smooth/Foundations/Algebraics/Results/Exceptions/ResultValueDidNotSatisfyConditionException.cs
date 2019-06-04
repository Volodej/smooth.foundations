namespace Smooth.Algebraics.Results.Exceptions
{
    public class ResultValueDidNotSatisfyConditionException<TValue> : ResultException
    {
        public TValue Value { get; }

        public ResultValueDidNotSatisfyConditionException(string message, TValue value) : base($"Value didn't satisfy condition: {message}")
        {
            Value = value;
        }
    }
}