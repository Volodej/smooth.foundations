using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class ResultOptionMatcher<T, TResult>
    {
        private readonly FuncSelectorForOption<T, TResult> _funcSelector;
        private readonly Option<T> _item;

        public ResultOptionMatcher(Option<T> item)
        {
            _item = item;
            _funcSelector = new FuncSelectorForOption<T, TResult>(x =>
            {
                throw new NoMatchException($"No match exist for value of {x}");
            });
        }
        public SomeMatcherResult<T, TResult> Some() => 
            new SomeMatcherResult<T, TResult>(this, _funcSelector);
        public NoneMatcherResult<T, TResult> None() => 
            new NoneMatcherResult<T, TResult>(this, _funcSelector);
        
        public ResultOptionMatcherAfterElse<T, TResult> Else(DelegateFunc<Option<T>, TResult> elseResult) => 
            new ResultOptionMatcherAfterElse<T, TResult>(_funcSelector, elseResult, _item);

        public ResultOptionMatcherAfterElse<T, TResult> Else(TResult elseResult) =>
            new ResultOptionMatcherAfterElse<T, TResult>(_funcSelector, elseResult, _item);

        public TResult Result() => _funcSelector.GetMatchedOrDefaultResult(_item);

    }
}