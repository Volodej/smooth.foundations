using System;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Action
{
    public sealed class ValueMatcher<T>
    {
        private readonly Action<Func<T, bool>, Action<T>> _addPredicateAndAction;
        private readonly Action<Action<T>> _addDefaultValueAction;
        private readonly ValueOrErrorMatcher<T> _matcher;
        private readonly bool _isError;

        public ValueMatcher(ValueOrErrorMatcher<T> matcher,
            Action<Func<T, bool>, Action<T>> addPredicateAndAction,
            Action<Action<T>> addAddDefaultValueAction,
            bool isError)
        {
            _addDefaultValueAction = addAddDefaultValueAction;
            _matcher = matcher;
            _addPredicateAndAction = addPredicateAndAction;
            _isError = isError;
        }

        public WhereForValue<T> Where(Func<T, bool> predicate) =>
            _isError
                ? WhereForValue<T>.Useless(_matcher)
                : new WhereForValue<T>(predicate, _addPredicateAndAction, _matcher);

        public WithForValueActionHandler<T> With(T value) => 
            new WithForValueActionHandler<T>(value, _addPredicateAndAction, _matcher, !_isError); 

        public ValueOrErrorMatcher<T> Do(Action<T> action)
        {
            _addDefaultValueAction(action);
            return _matcher;
        }
    }
}