using System;
using Smooth.Algebraics;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class OptionMatcher<T>
    {
        private readonly ActionSelectorForOption<T> _actionSelector;
        private readonly Option<T> _item;

        internal OptionMatcher(Option<T> item)
        {
            _item = item;
            _actionSelector = new ActionSelectorForOption<T>(x =>
                { throw new NoMatchException($"No match action exists for value of {x}"); });
        }

        public ResultOptionMatcher<T, TResult> To<TResult>() => new ResultOptionMatcher<T, TResult>(_item);
        public SomeMatcher<T> Some() => new SomeMatcher<T>(this, _actionSelector.AddPredicateAndAction);
        public NoneMatcher<T> None() => new NoneMatcher<T>(this, _actionSelector.AddPredicateAndAction); 
        public OptionMatcherAfterElse<T> Else(Action<Option<T>> elseAction) => 
            new OptionMatcherAfterElse<T>(_actionSelector, elseAction, _item);
        public OptionMatcherAfterElse<T> IgnoreElse() =>
            new OptionMatcherAfterElse<T>(_actionSelector, _ => { }, _item);

        public void Exec() => _actionSelector.InvokeMatchedOrDefaultAction(_item);
    }
}