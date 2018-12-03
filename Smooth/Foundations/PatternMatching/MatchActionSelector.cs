using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching
{
    internal sealed class MatchActionSelector<T1>
    {
        private readonly Action<T1> _defaultAction;

        private readonly List<ValueTuple<DelegateFunc<T1, bool>, Action<T1>>> _testsAndActions =
            new List<ValueTuple<DelegateFunc<T1, bool>, Action<T1>>>();

        public MatchActionSelector(Action<T1> defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public void AddPredicateAndAction(DelegateFunc<T1, bool> test, Action<T1> action) =>
            _testsAndActions.Add(new ValueTuple<DelegateFunc<T1, bool>, Action<T1>>(test, action));

        public void InvokeMatchedActionUsingDefaultIfRequired(T1 value)
        {
            var action = _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, p) => matcher.Item1(p), value);

            if (action.isSome)
            {
                action.value.Item2(value);
            }
            else
            {
                _defaultAction(value);
            }
        }

        public Option<Action<T1>> FindMatchedActionOrNone(T1 value)
        {
            return _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, v) => matcher.Item1(v), value)
                .Select(t => t.Item2);
        }
    }

    internal sealed class MatchActionSelector<T1, T2>
    {
        private readonly Action<T1, T2> _defaultAction;

        private readonly List<ValueTuple<DelegateFunc<T1, T2, bool>, Action<T1, T2>>> _testsAndActions =
            new List<ValueTuple<DelegateFunc<T1, T2, bool>, Action<T1, T2>>>();

        public MatchActionSelector(Action<T1, T2> defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public void AddPredicateAndAction(DelegateFunc<T1, T2, bool> test, Action<T1, T2> action) =>
            _testsAndActions.Add(new ValueTuple<DelegateFunc<T1, T2, bool>, Action<T1, T2>>(test, action));

        public void InvokeMatchedActionUsingDefaultIfRequired(T1 value1, T2 value2)
        {
            var args = (value1, value2);
            var action = _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2), args);

            if (action.isSome)
            {
                action.value.Item2(value1, value2);
            }
            else
            {
                _defaultAction(value1, value2);
            }
        }

        public Option<Action<T1, T2>> FindMatchedActionOrNone(T1 value1, T2 value2)
        {
            var args = (value1, value2);
            return _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2), args)
                .Select(t => t.Item2);
        }
    }

    internal sealed class MatchActionSelector<T1, T2, T3>
    {
        private readonly Action<T1, T2, T3> _defaultAction;

        private readonly List<ValueTuple<DelegateFunc<T1, T2, T3, bool>, Action<T1, T2, T3>>> _testsAndActions =
            new List<ValueTuple<DelegateFunc<T1, T2, T3, bool>, Action<T1, T2, T3>>>();

        public MatchActionSelector(Action<T1, T2, T3> defaultAction)
        {
            _defaultAction = defaultAction;
        }

        public void AddPredicateAndAction(DelegateFunc<T1, T2, T3, bool> test, Action<T1, T2, T3> action) =>
            _testsAndActions.Add(new ValueTuple<DelegateFunc<T1, T2, T3, bool>, Action<T1, T2, T3>>(test, action));

        public void InvokeMatchedActionUsingDefaultIfRequired(T1 value1, T2 value2, T3 value3)
        {
            var args = (value1, value2, value3);
            var action = _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2, a.Item3), args);
            if (action.isSome)
            {
                action.value.Item2(value1, value2, value3);
            }
            else
            {
                _defaultAction(value1, value2, value3);
            }
        }

        public Option<Action<T1, T2, T3>> FindMatchedActionOrNone(T1 value1, T2 value2, T3 value3)
        {
            var args = (value1, value2, value3);
            return _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2, a.Item3), args)
                .Select(t => t.Item2);
        }
    }

    internal sealed class MatchActionSelector<T1, T2, T3, T4>
    {
        private readonly Action<T1, T2, T3, T4> _defaultAction;

        private readonly List<ValueTuple<DelegateFunc<T1, T2, T3, T4, bool>, Action<T1, T2, T3, T4>>> _testsAndActions =
            new List<ValueTuple<DelegateFunc<T1, T2, T3, T4, bool>, Action<T1, T2, T3, T4>>>();

        public MatchActionSelector(Action<T1, T2, T3, T4> defaultAction) { _defaultAction = defaultAction; }

        public void AddPredicateAndAction(DelegateFunc<T1, T2, T3, T4, bool> test, Action<T1, T2, T3, T4> action) =>
            _testsAndActions.Add(new ValueTuple<DelegateFunc<T1, T2, T3, T4, bool>, Action<T1, T2, T3, T4>>(test, action));

        public void InvokeMatchedActionUsingDefaultIfRequired(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var args = (value1, value2, value3, value4);
            var action = _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2, a.Item3, a.Item4), args);

            if (action.isSome)
            {
                action.value.Item2(value1, value2, value3, value4);
            }
            else
            {
                _defaultAction(value1, value2, value3, value4);
            }
        }

        public Option<Action<T1, T2, T3, T4>> FindMatchedActionOrNone(T1 value1, T2 value2, T3 value3, T4 value4)
        {
            var args = (value1, value2, value3, value4);
            return _testsAndActions
                .Slinq()
                .FirstOrNone((matcher, a) => matcher.Item1(a.Item1, a.Item2, a.Item3, a.Item4), args)
                .Select(t => t.Item2);
        }
    }
}