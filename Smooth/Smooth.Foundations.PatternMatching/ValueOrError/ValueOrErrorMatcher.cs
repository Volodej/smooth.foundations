using Smooth.Foundations.Algebraics;
using Smooth.Foundations.PatternMatching.ValueOrError.Action;
using Smooth.Foundations.PatternMatching.ValueOrError.Function;

namespace Smooth.Foundations.PatternMatching.ValueOrError
{
    public struct ValueOrErrorMatcher<T>
    {
        private readonly ValueOrErrorMatchActionSelector<T> _actionSelector;
        private readonly ValueOrError<T> _item;

        internal ValueOrErrorMatcher(ValueOrError<T> item)
        {
            _item = item;
            _actionSelector = new ValueOrErrorMatchActionSelector<T>(
                () => { throw new NoMatchException($"No match action exists for value of {item}"); });
        }

        public ValueMatcher<T> Value() => new ValueMatcher<T>(this,
            _actionSelector.AddPredicateAndAction,
            _actionSelector.SetDefaultOnValueAction,
            _item.IsError);

        public ErrorMatcher<T> Error() => new ErrorMatcher<T>(this, _actionSelector.AddErrorAction, _item.IsError);

        public ValueOrErrorResultMatcher<T, TResult> To<TResult>() => new ValueOrErrorResultMatcher<T, TResult>(_item);

        public void Exec() => _actionSelector.InvokeMatchedOrDefaultAction(_item);
    }
}