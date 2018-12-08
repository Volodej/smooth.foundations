using System;
using Smooth.Foundations.Algebraics;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Function
{
    public class ValueOrErrorResultMatcherWithElse<T1, TResult>
    {
        private readonly ValueOrErrorMatchFunctionSelector<T1, TResult> _selector;
        private readonly Func<T1, TResult> _elseAction;
        private readonly ValueOrError<T1> _value;

        internal ValueOrErrorResultMatcherWithElse(ValueOrErrorMatchFunctionSelector<T1, TResult> selector,
            Func<T1, TResult> elseAction, ValueOrError<T1> value)
        {
            _selector = selector;
            _elseAction = elseAction;
            _value = value;
        }

        public ValueOrError<TResult> Result()
        {
            if (_value.IsError) return ValueOrError<TResult>.FromError(_value.Error);
            var result = _selector.DetermineResult(_value);
            return result.isSome 
                ? result.value 
                : ValueOrError<TResult>.FromValue(_elseAction(_value.Value));
        }
    }
}