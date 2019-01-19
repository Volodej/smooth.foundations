using System;
using Smooth.Algebraics;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class OptionMatcherAfterElse<T>
    {
        private readonly Option<T> _item;
        private readonly Action<Option<T>> _elseAction;
        private readonly ActionSelectorForOption<T> _actionSelector;

        internal OptionMatcherAfterElse(ActionSelectorForOption<T> actionSelector, Action<Option<T>> elseAction, Option<T> item)
        {
            _actionSelector = actionSelector;
            _elseAction = elseAction;
            _item = item;
        }

        public void Exec()
        {
            _actionSelector.InvokeMatchedOrProvidedAction(_item, _elseAction);
        }
    }
}