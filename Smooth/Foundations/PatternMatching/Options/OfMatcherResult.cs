using System;
using System.Collections.Generic;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class OfMatcherResult<T, TResult>
    {
        private readonly ResultOptionMatcher<T, TResult> _matcher;
        private readonly FuncSelectorForOption<T, TResult> _predicateAndResultManager;
        private readonly List<T> _values = new List<T>(5);

        internal OfMatcherResult(T value, 
                           ResultOptionMatcher<T, TResult> matcher,
                           FuncSelectorForOption<T, TResult> predicateAndResultManager)
        {
            _matcher = matcher;
            _predicateAndResultManager = predicateAndResultManager;
            _values.Add(value);
        }
        public OfMatcherResult<T, TResult> Or(T value)
        {
            _values.Add(value);
            return this;
        }

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(TResult result) => Return(result);

        [Obsolete("Please use return")]
        public ResultOptionMatcher<T, TResult> Do(DelegateFunc<T, TResult> func) => Return(func);

        public ResultOptionMatcher<T, TResult> Return(TResult result)
        {
            _predicateAndResultManager.AddPredicateAndResult(o => o.isSome &&
                _values.Slinq()
                .Any((v, p) => Collections.EqualityComparer<T>.Default.Equals(v, p), o.value), result);
            return _matcher;
        }

        public ResultOptionMatcher<T, TResult> Return(DelegateFunc<T, TResult> func)
        {
            _predicateAndResultManager.AddPredicateAndValueFunc(o => o.isSome &&
                _values.Slinq()
                .Any((v, p) => Collections.EqualityComparer<T>.Default.Equals(v, p), o.value), func);
            return _matcher;
        }
    }
}