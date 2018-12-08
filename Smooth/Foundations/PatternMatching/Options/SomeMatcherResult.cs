using System;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class SomeMatcherResult<T, TResult>
    {
        private readonly ResultOptionMatcher<T, TResult> _matcher;
        private readonly FuncSelectorForOption<T, TResult> _predicateAndResultManager;

        internal SomeMatcherResult(ResultOptionMatcher<T, TResult> matcher, 
            FuncSelectorForOption<T, TResult> predicateAndResultManager)
        {
            _matcher = matcher;
            _predicateAndResultManager = predicateAndResultManager;
        }

        public OfMatcherResult<T, TResult> Of(T value) => new OfMatcherResult<T, TResult>(value, _matcher, _predicateAndResultManager);

        public WhereForOptionResult<T, TResult> Where(Func<T, bool> predicate) => 
            new WhereForOptionResult<T, TResult>(predicate, _predicateAndResultManager, _matcher);

        public ResultOptionMatcher<T, TResult> Do(Func<T, TResult> func)
        {
            _predicateAndResultManager.AddPredicateAndValueFunc(o => o.isSome, func);
            return _matcher;
        }

        public ResultOptionMatcher<T, TResult> Do(TResult result)
        {
            _predicateAndResultManager.AddPredicateAndResult(o => o.isSome, result);
            return _matcher;
        }
    }
}