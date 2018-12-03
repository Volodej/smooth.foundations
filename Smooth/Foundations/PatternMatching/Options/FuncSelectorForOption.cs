using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.Options
{
    internal sealed class FuncSelectorForOption<T, TResult>
    {
        private readonly DelegateFunc<Option<T>, TResult> _defaultFunc;
        private readonly List<ValueTuple<DelegateFunc<Option<T>, bool>, 
            Union<DelegateFunc<T, TResult>, DelegateFunc<Option<T>, TResult>, TResult>>> _predicatesAndResults =
                new List<ValueTuple<DelegateFunc<Option<T>, bool>, 
                    Union<DelegateFunc<T, TResult>, DelegateFunc<Option<T>, TResult>, TResult>>>();

        public FuncSelectorForOption(DelegateFunc<Option<T>, TResult> defaultFunc)
        {
            _defaultFunc = defaultFunc;
        }

        public void AddPredicateAndOptionFunc(DelegateFunc<Option<T>, bool> predicate, DelegateFunc<Option<T>, TResult> func)
        {
            _predicatesAndResults.Add((predicate,
                Union<DelegateFunc<T, TResult>, DelegateFunc<Option<T>, TResult>, TResult>.CreateSecond(func)));
        }

        public void AddPredicateAndValueFunc(DelegateFunc<Option<T>, bool> predicate, DelegateFunc<T, TResult> func)
        {
            _predicatesAndResults.Add((predicate,
                Union<DelegateFunc<T, TResult>, DelegateFunc<Option<T>, TResult>, TResult>.CreateFirst(func)));
        }

        public void AddPredicateAndResult(DelegateFunc<Option<T>, bool> predicate, TResult result)
        {
            _predicatesAndResults.Add((predicate, 
                Union<DelegateFunc<T, TResult>, DelegateFunc<Option<T>, TResult>, TResult>.CreateThird(result)));
        }

        public TResult GetMatchedOrDefaultResult(Option<T> item)
        {
            return GetMatchedOrProvidedResult(item, _defaultFunc);
        }

        public TResult GetMatchedOrProvidedResult(Option<T> item, DelegateFunc<Option<T>, TResult> elseFunc)
        {
            return GetMatchedResult(item).ValueOr(elseFunc, item);
        }

        public TResult GetMatchedOrProvidedResult(Option<T> item, TResult result)
        {
            return GetMatchedResult(item).ValueOr(result);
        }

        private Option<TResult> GetMatchedResult(Option<T> item)
        {
            return _predicatesAndResults.Slinq()
                .FirstOrNone((pair, v) => pair.Item1(v), item)
                .Select(pair => pair.Item2)
                .Select((resultProvider, pItem) => 
                    resultProvider.Case == Variant.First 
                    ? resultProvider.Case1(pItem.value)
                    : resultProvider.Case == Variant.Second
                    ? resultProvider.Case2(pItem)
                    : resultProvider.Case3, item);
        }

    }
}