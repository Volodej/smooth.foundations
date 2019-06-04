using System;

namespace Smooth.Foundations.PatternMatching
{
    /// <summary>
    /// Fluent class created by an invocation of Else() when handling a pattern definition for T that ends in Exec().
    /// Whilst this is a public class (as the user needs access to Exec()), it has an internal constructor as it's
    /// intended for pattern matching internal usage only.
    /// </summary>
    public sealed class ExecMatcherAfterElse<T1>
    {
        private readonly MatchActionSelector<T1> _selector;
        private readonly Action<T1> _elseAction;
        private readonly T1 _value;

        internal ExecMatcherAfterElse(MatchActionSelector<T1> selector, Action<T1> elseAction, T1 value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public void Exec()
        {
            _selector.FindMatchedActionOrNone(_value).ValueOr(_elseAction)(_value);
        }
    }

    public sealed class ExecMatcherAfterElse<T1, T2>
    {
        private readonly MatchActionSelector<T1, T2> _selector;
        private readonly Action<T1, T2> _elseAction;
        private readonly ValueTuple<T1, T2> _value;

        internal ExecMatcherAfterElse(MatchActionSelector<T1, T2> selector,
                                     Action<T1, T2> elseAction,
                                     ValueTuple<T1, T2> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public void Exec()
        {
            var action = _selector.FindMatchedActionOrNone(_value.Item1, _value.Item2)
                .ValueOr(_elseAction);
            action(_value.Item1, _value.Item2);
        }
    }

    public sealed class ExecMatcherAfterElse<T1, T2, T3>
    {
        private readonly MatchActionSelector<T1, T2, T3> _selector;
        private readonly Action<T1, T2, T3> _elseAction;
        private readonly ValueTuple<T1, T2, T3> _value;

        internal ExecMatcherAfterElse(MatchActionSelector<T1, T2, T3> selector,
                                      Action<T1, T2, T3> elseAction,
                                      ValueTuple<T1, T2, T3> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public void Exec()
        {
            var action = _selector.FindMatchedActionOrNone(_value.Item1, _value.Item2, _value.Item3)
                .ValueOr(_elseAction);
            action(_value.Item1, _value.Item2, _value.Item3);
        }
    }

    public sealed class ExecMatcherAfterElse<T1, T2, T3, T4>
    {
        private readonly MatchActionSelector<T1, T2, T3, T4> _selector;
        private readonly Action<T1, T2, T3, T4> _elseAction;
        private readonly ValueTuple<T1, T2, T3, T4> _value;

        internal ExecMatcherAfterElse(MatchActionSelector<T1, T2, T3, T4> selector,
                                      Action<T1, T2, T3, T4> elseAction,
                                      ValueTuple<T1, T2, T3, T4> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public void Exec()
        {
            var action = _selector.FindMatchedActionOrNone(_value.Item1, _value.Item2, _value.Item3, _value.Item4)
                .ValueOr(_elseAction);
            action(_value.Item1, _value.Item2, _value.Item3, _value.Item4);
        }
    }

}