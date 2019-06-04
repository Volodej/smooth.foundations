using System;

namespace Smooth.Foundations.PatternMatching
{
    /// <summary>
    /// An instance of this class will start a fluent function chain for defining and evaluating a pattern match against
    /// a value of T. It either ends in Exec(), or if To{TResult}() is used, switches to a ResultMatcher{T}.
    /// </summary>
    public sealed class ExecMatcher<T1>
    {
        private readonly MatchActionSelector<T1> _actionSelector;
        private readonly T1 _item;

        internal ExecMatcher(T1 item)
        {
            _item = item;
            _actionSelector = new MatchActionSelector<T1>(
                x => { throw new NoMatchException($"No match action exists for value of {_item}"); });
        }

        public ResultMatcher<T1, TResult> To<TResult>() => new ResultMatcher<T1, TResult>(_item);

        public WithForActionHandler<ExecMatcher<T1>, T1> With(T1 value) =>
            new WithForActionHandler<ExecMatcher<T1>, T1>(value, RecordAction, this);

        public WhereForActionHandler<ExecMatcher<T1>, T1> Where(Func<T1, bool> expression) =>
            new WhereForActionHandler<ExecMatcher<T1>, T1>(expression, RecordAction, this);

        public ExecMatcherAfterElse<T1> Else(Action<T1> action) => new ExecMatcherAfterElse<T1>(_actionSelector, action, _item);

        public ExecMatcherAfterElse<T1> IgnoreElse() => new ExecMatcherAfterElse<T1>(_actionSelector, x => { }, _item);

        public void Exec() => _actionSelector.InvokeMatchedActionUsingDefaultIfRequired(_item);

        private void RecordAction(Func<T1, bool> test, Action<T1> action) => _actionSelector.AddPredicateAndAction(test, action);

    }

    public sealed class ExecMatcher<T1, T2>
    {
        private readonly MatchActionSelector<T1, T2> _actionSelector;
        private readonly ValueTuple<T1, T2> _item;

        internal ExecMatcher(T1 item1, T2 item2)
        {
            _item = (item1, item2);
            _actionSelector = new MatchActionSelector<T1, T2>((x, y) =>
            {
                throw new NoMatchException($"No match action exists for value of ({_item.Item1},{_item.Item2})");
            });
        }

        public ResultMatcher<T1, T2, TResult> To<TResult>() => new ResultMatcher<T1, T2, TResult>(_item);

        public WithForActionHandler<ExecMatcher<T1, T2>, T1, T2> With(T1 value1, T2 value2) =>
            new WithForActionHandler<ExecMatcher<T1, T2>, T1, T2>((value1, value2), RecordAction, this);

        public WhereForActionHandler<ExecMatcher<T1, T2>, T1, T2> Where(Func<T1, T2, bool> expression) =>
            new WhereForActionHandler<ExecMatcher<T1, T2>, T1, T2>(expression, RecordAction, this);

        private void RecordAction(Func<T1, T2, bool> test, Action<T1, T2> action) =>
            _actionSelector.AddPredicateAndAction(test, action);

        public ExecMatcherAfterElse<T1, T2> Else(Action<T1, T2> action) =>
            new ExecMatcherAfterElse<T1, T2>(_actionSelector, action, _item);

        public ExecMatcherAfterElse<T1, T2> IgnoreElse() =>
            new ExecMatcherAfterElse<T1, T2>(_actionSelector, (x, y) => { }, _item);

        public void Exec() => _actionSelector.InvokeMatchedActionUsingDefaultIfRequired(_item.Item1, _item.Item2);
    }

    public sealed class ExecMatcher<T1, T2, T3>
    {
        private readonly MatchActionSelector<T1, T2, T3> _actionSelector;
        private readonly ValueTuple<T1, T2, T3> _item;

        internal ExecMatcher(T1 item1, T2 item2, T3 item3)
        {
            _item = (item1, item2, item3);
            _actionSelector = new MatchActionSelector<T1, T2, T3>((x, y, z) =>
            {
                throw new NoMatchException(
                    $"No match action exists for value of ({_item.Item1}, {_item.Item2}, {_item.Item3})");
            });
        }

        public ResultMatcher<T1, T2, T3, TResult> To<TResult>() => new ResultMatcher<T1, T2, T3, TResult>(_item);

        public WithForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3> With(T1 value1, T2 value2, T3 value3) =>
            new WithForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3>((value1, value2, value3),
                                                                          RecordAction,
                                                                          this);

        public WhereForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3> Where(Func<T1, T2, T3, bool> expression) =>
            new WhereForActionHandler<ExecMatcher<T1, T2, T3>, T1, T2, T3>(expression, RecordAction, this);

        private void RecordAction(Func<T1, T2, T3, bool> test, Action<T1, T2, T3> action) =>
            _actionSelector.AddPredicateAndAction(test, action);

        public ExecMatcherAfterElse<T1, T2, T3> Else(Action<T1, T2, T3> action) =>
            new ExecMatcherAfterElse<T1, T2, T3>(_actionSelector, action, _item);

        public ExecMatcherAfterElse<T1, T2, T3> IgnoreElse() =>
            new ExecMatcherAfterElse<T1, T2, T3>(_actionSelector, (x, y, z) => { }, _item);

        public void Exec() =>
            _actionSelector.InvokeMatchedActionUsingDefaultIfRequired(_item.Item1, _item.Item2, _item.Item3);
    }

    public sealed class ExecMatcher<T1, T2, T3, T4>
    {
        private readonly MatchActionSelector<T1, T2, T3, T4> _actionSelector;
        private readonly ValueTuple<T1, T2, T3, T4> _item;

        internal ExecMatcher(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            _item = (item1, item2, item3, item4);
            _actionSelector = new MatchActionSelector<T1, T2, T3, T4>(
                (w, x, y, z) =>
                {
                    throw new NoMatchException(
                        $"No match action exists for value of ({_item.Item1}, {_item.Item2}, {_item.Item3})");
                });
        }

        public ResultMatcher<T1, T2, T3, T4, TResult> To<TResult>() => new ResultMatcher<T1, T2, T3, T4, TResult>(_item);

        public WithForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4> With(T1 value1,
                                                                                      T2 value2,
                                                                                      T3 value3,
                                                                                      T4 value4)
        {
            return new WithForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>((value1,
                                                                                                      value2,
                                                                                                      value3,
                                                                                                      value4),
                                                                                         RecordAction,
                                                                                         this);
        }

        public WhereForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>
            Where(Func<T1, T2, T3, T4, bool> expression) =>
                new WhereForActionHandler<ExecMatcher<T1, T2, T3, T4>, T1, T2, T3, T4>(expression, RecordAction, this);

        private void RecordAction(Func<T1, T2, T3, T4, bool> test, Action<T1, T2, T3, T4> action) =>
            _actionSelector.AddPredicateAndAction(test, action);

        public ExecMatcherAfterElse<T1, T2, T3, T4> Else(Action<T1, T2, T3, T4> action) =>
            new ExecMatcherAfterElse<T1, T2, T3, T4>(_actionSelector, action, _item);

        public ExecMatcherAfterElse<T1, T2, T3, T4> IgnoreElse() =>
            new ExecMatcherAfterElse<T1, T2, T3, T4>(_actionSelector, (w, x, y, z) => { }, _item);

        public void Exec() =>
            _actionSelector.InvokeMatchedActionUsingDefaultIfRequired(_item.Item1,
                                                                      _item.Item2,
                                                                      _item.Item3,
                                                                      _item.Item4);
    }

}

