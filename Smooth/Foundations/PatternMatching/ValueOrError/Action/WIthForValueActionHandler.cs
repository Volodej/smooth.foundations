using System.Collections.Generic;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Action
{
    public sealed class WithForValueActionHandler<T>
    {
        private readonly DelegateAction<DelegateFunc<T, bool>, DelegateAction<T>> _addPredicateAndAction;
        private readonly ValueOrErrorMatcher<T> _matcher;
        private readonly bool _isUseless;
        private readonly List<T> _values;

        internal WithForValueActionHandler(T value, DelegateAction<DelegateFunc<T, bool>, DelegateAction<T>> addPredicateAndAction, 
                                         ValueOrErrorMatcher<T> matcher, 
                                         bool isUseless)
        {
            _addPredicateAndAction = addPredicateAndAction;
            _matcher = matcher;
            _isUseless = isUseless;
            if (isUseless)
                return;
            _values = new List<T>(3) {value};
        }

        public WithForValueActionHandler<T> Or(T value)
        {
            if (!_isUseless)
            {
                _values.Add(value);
            }
            return this;
        }

        public ValueOrErrorMatcher<T> Do(DelegateAction<T> action)
        {
            if (!_isUseless)
            {
                _addPredicateAndAction(val => _values.Slinq().Any(v => Collections.EqualityComparer<T>.Default.Equals(val, v)), action);
            }
            return _matcher;
        } 
    }
}
