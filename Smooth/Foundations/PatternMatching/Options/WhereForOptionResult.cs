using System;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class WhereForOptionResult<T, TResult>
    {
        private readonly Func<T, bool> _predicate;
        private readonly FuncSelectorForOption<T, TResult> _predicateAndResultManager;
        private readonly ResultOptionMatcher<T, TResult> _matcher;

        internal WhereForOptionResult(Func<T, bool> predicate,
                                FuncSelectorForOption<T, TResult> predicateAndResultManager,
                                ResultOptionMatcher<T, TResult> matcher)
        {
            _predicate = predicate;
            _predicateAndResultManager = predicateAndResultManager;
            _matcher = matcher;
        }

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(Func<T, TResult> func) => Return(func);

        public ResultOptionMatcher<T, TResult> Return(TResult result)
        {
            _predicateAndResultManager.AddPredicateAndResult(o => o.isSome && _predicate(o.value), result);
            return _matcher;
        } 

        public ResultOptionMatcher<T, TResult> Return(Func<T, TResult> func)
        {
            _predicateAndResultManager.AddPredicateAndValueFunc(o => o.isSome && _predicate(o.value), func);
            return _matcher;
        }
    }
}
