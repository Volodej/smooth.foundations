namespace Smooth.Foundations.PatternMatching.ValueOrError.Action
{
    public class ErrorMatcher<T>
    {
        private readonly System.Action<System.Action<string>> _addAction;
        private readonly ValueOrErrorMatcher<T> _matcher;
        private readonly bool _isError;

        public ErrorMatcher(ValueOrErrorMatcher<T> matcher,
            System.Action<System.Action<string>> addAction, bool isError)
        {
            _addAction = addAction;
            _matcher = matcher;
            _isError = isError;
        }

        public ValueOrErrorMatcher<T> Do(System.Action<string> action)
        {
            if (_isError)
            {
                _addAction(action);
            }
            return _matcher;
        }
    }
}