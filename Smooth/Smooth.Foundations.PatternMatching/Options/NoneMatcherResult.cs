using System;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class NoneMatcherResult<T, TResult>
    {
        private readonly FuncSelectorForOption<T, TResult> _addPredicateAndFunc;
        private readonly ResultOptionMatcher<T, TResult> _matcher;

        internal NoneMatcherResult(ResultOptionMatcher<T, TResult> matcher,
                           FuncSelectorForOption<T, TResult> addPredicateAndFunc)
        {
            _addPredicateAndFunc = addPredicateAndFunc;
            _matcher = matcher;
        }

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(Func<TResult> func) => Return(func);

        public ResultOptionMatcher<T, TResult> Return(TResult result)
        {
            _addPredicateAndFunc.AddPredicateAndResult(o => o.isNone, result);
            return _matcher;
        }

        public ResultOptionMatcher<T, TResult> Return(Func<TResult> result)
        {
            _addPredicateAndFunc.AddPredicateAndOptionFunc(o => o.isNone, o => result());
            return _matcher;
        }
    }
}