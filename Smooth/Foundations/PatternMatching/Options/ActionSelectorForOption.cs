using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.Options
{
    internal sealed class ActionSelectorForOption<T>
    {
        private readonly Action<Option<T>> _defaultAction;
        private readonly List<ValueTuple<Func<Option<T>, bool>, Either<Action<T>, Action<Option<T>>>>>  _predicatesAndActions = 
            new List<ValueTuple<Func<Option<T>, bool>, Either<Action<T>, Action<Option<T>>>>>();

        public ActionSelectorForOption(Action<Option<T>> defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public void AddPredicateAndAction(Func<Option<T>, bool> predicate, Action<T> action)
        {
            _predicatesAndActions.Add((predicate,
                Either<Action<T>, Action<Option<T>>>.Left(action)));
        }

        public void AddPredicateAndAction(Func<Option<T>, bool> predicate, Action<Option<T>> action)
        {
            _predicatesAndActions.Add((predicate,
                Either<Action<T>, Action<Option<T>>>.Right(action)));
        }


        public void InvokeMatchedOrDefaultAction(Option<T> item)
        {
            InvokeMatchedOrProvidedAction(item, _defaultAction);
        }

        public void InvokeMatchedOrProvidedAction(Option<T> item, Action<Option<T>> elseAction)
        {
            var actionOption = _predicatesAndActions.Slinq()
                .FirstOrNone((pair, v) => pair.Item1(v), item)
                .Select(pair => pair.Item2);
            if (actionOption.isNone)
            {
                elseAction(item);
                return;
            }
            var action = actionOption.value;
            if (action.isLeft)
            {
                action.leftValue(item.value);
            }
            else
            {
                action.rightValue(item);
            }
        }
    }
}