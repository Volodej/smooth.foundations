using System;
using Smooth.Algebraics;
using Smooth.Delegates;
using Action = System.Action;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class NoneMatcher<T>
    {
        private readonly Action<Func<Option<T>, bool>, Action<Option<T>>> _addPredicateAndAction;
        private readonly OptionMatcher<T> _matcher; 

        public NoneMatcher(OptionMatcher<T> matcher, Action<Func<Option<T>, bool>, Action<Option<T>>> addPredicateAndAction)
        {
            _addPredicateAndAction = addPredicateAndAction;
            _matcher = matcher;
        }

        public OptionMatcher<T> Do(Action action)
        {
            _addPredicateAndAction(o => o.isNone, o => action());
            return _matcher;
        } 
    }
}