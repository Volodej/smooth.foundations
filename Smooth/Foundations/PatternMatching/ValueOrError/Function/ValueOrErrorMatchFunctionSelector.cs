using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.ValueOrError.Function
{
    public sealed class ValueOrErrorMatchFunctionSelector<T, TResult>
    {
        private readonly DelegateFunc<T, TResult> _defaultFunction;

        private readonly List<ValueTuple<DelegateFunc<T, bool>, DelegateFunc<T, TResult>>> _predicatesAndFuncs;
        private readonly bool _isError;

        public ValueOrErrorMatchFunctionSelector(DelegateFunc<T, TResult> defaultFunction, bool isError)
        {
            _defaultFunction = defaultFunction;
            _predicatesAndFuncs = new List<ValueTuple<DelegateFunc<T, bool>, DelegateFunc<T, TResult>>>();
            _isError = isError;
        }

        public void AddPredicateAndAction(DelegateFunc<T, bool> test, DelegateFunc<T, TResult> action) =>
            _predicatesAndFuncs.Add(new ValueTuple<DelegateFunc<T, bool>, DelegateFunc<T, TResult>>(test, action));

        public Option<ValueOrError<TResult>> DetermineResult(ValueOrError<T> value)
        {
            return _predicatesAndFuncs
                .Slinq()
                .FirstOrNone((tuple, p) => tuple.Item1(p.Value), value)
                .Select((pair, p) => ValueOrError<TResult>.FromValue(pair.Item2(p.Value)), value);
        }

        public ValueOrError<TResult> DetermineResultUsingDefaultIfRequired(ValueOrError<T> value)
        {
            if (_isError) return ValueOrError<TResult>.FromError(value.Error);

            var funcAndArgsPair =
                _predicatesAndFuncs
                    .Slinq()
                    .FirstOrNone((predicateAndTransformationTuple, concreteValue)
                        => predicateAndTransformationTuple.Item1(concreteValue.Value), value);

            return funcAndArgsPair.isSome
                ? ValueOrError<TResult>.FromValue(funcAndArgsPair.value.Item2(value.Value))
                : ValueOrError<TResult>.FromValue(_defaultFunction(value.Value));
        }
    }
}