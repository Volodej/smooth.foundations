using System;
using Smooth.Algebraics;
using Smooth.Delegates;

namespace Smooth.Foundations.PatternMatching
{
    public sealed class ResultMatcherWithElse<T1, TResult>
    {
        private readonly MatchFunctionSelector<T1, TResult> _selector;
        private readonly DelegateFunc<T1, TResult> _elseAction;
        private readonly T1 _value;

        internal ResultMatcherWithElse(MatchFunctionSelector<T1, TResult> selector, DelegateFunc<T1, TResult> elseAction, T1 value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public TResult Result()
        {
            var result = _selector.DetermineResult(_value);
            return result.isSome ? result.value : _elseAction(_value);
        }
    }

    public sealed class ResultMatcherWithElse<T1, T2, TResult>
    {
        private readonly MatchFunctionSelector<T1, T2, TResult> _selector;
        private readonly DelegateFunc<T1, T2, TResult> _elseAction;
        private readonly ValueTuple<T1, T2> _value;

        internal ResultMatcherWithElse(MatchFunctionSelector<T1, T2, TResult> selector,
                                       DelegateFunc<T1, T2, TResult> elseAction,
                                       ValueTuple<T1, T2> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public TResult Result()
        {
            var result = _selector.DetermineResult(_value);
            return result.isSome ? result.value : _elseAction(_value.Item1, _value.Item2);
        }
    }

    public sealed class ResultMatcherWithElse<T1, T2, T3, TResult>
    {
        private readonly MatchFunctionSelector<T1, T2, T3, TResult> _selector;
        private readonly DelegateFunc<T1, T2, T3, TResult> _elseAction;
        private readonly ValueTuple<T1, T2, T3> _value;

        internal ResultMatcherWithElse(MatchFunctionSelector<T1, T2, T3, TResult> selector,
                                       DelegateFunc<T1, T2, T3, TResult> elseAction,
                                       ValueTuple<T1, T2, T3> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public TResult Result()
        {
            var result = _selector.DetermineResult(_value);
            return result.isSome ? result.value : _elseAction(_value.Item1, _value.Item2, _value.Item3);
        }
    }

    public sealed class ResultMatcherWithElse<T1, T2, T3, T4, TResult>
    {
        private readonly MatchFunctionSelector<T1, T2, T3, T4, TResult> _selector;
        private readonly DelegateFunc<T1, T2, T3, T4, TResult> _elseAction;
        private readonly ValueTuple<T1, T2, T3, T4> _value;

        internal ResultMatcherWithElse(MatchFunctionSelector<T1, T2, T3, T4, TResult> selector,
                                       DelegateFunc<T1, T2, T3, T4, TResult> elseAction,
                                       ValueTuple<T1, T2, T3, T4> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public TResult Result()
        {
            var result = _selector.DetermineResult(_value);
            return result.isSome 
                ? result.value 
                : _elseAction(_value.Item1, _value.Item2, _value.Item3, _value.Item4);
        }
    }

}