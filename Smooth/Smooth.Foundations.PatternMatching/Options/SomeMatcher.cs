using System;
using Smooth.Algebraics;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class SomeMatcher<T>
    {
        private readonly OptionMatcher<T> _matcher;
        private readonly Action<Func<Option<T>, bool>, Action<T>> _addPredicateAndAction;

        public SomeMatcher(OptionMatcher<T> matcher, Action<Func<Option<T>, bool>, Action<T>> addPredicateAndAction)
        {
            _matcher = matcher;
            _addPredicateAndAction = addPredicateAndAction;
        }

        public OfMatcher<T> Of(T value) => new OfMatcher<T>(value, _matcher, _addPredicateAndAction);

        public WhereForOption<T> Where(Func<T, bool> predicate) => 
            new WhereForOption<T>(predicate, _addPredicateAndAction, _matcher);

        public OptionMatcher<T> Do(Action<T> action)
        {
            _addPredicateAndAction(o => o.isSome, action);
            return _matcher;
        } 
    }
}