using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;

namespace Smooth.Foundations.PatternMatching.Options
{
    internal sealed class FuncSelectorForOption<T, TResult>
    {
        private readonly Func<Option<T>, TResult> _defaultFunc;
        private readonly List<ValueTuple<Func<Option<T>, bool>, 
            Union<Func<T, TResult>, Func<Option<T>, TResult>, TResult>>> _predicatesAndResults =
                new List<ValueTuple<Func<Option<T>, bool>, 
                    Union<Func<T, TResult>, Func<Option<T>, TResult>, TResult>>>();

        public FuncSelectorForOption(Func<Option<T>, TResult> defaultFunc)
        {
            _defaultFunc = defaultFunc;
        }

        public void AddPredicateAndOptionFunc(Func<Option<T>, bool> predicate, Func<Option<T>, TResult> func)
        {
            _predicatesAndResults.Add((predicate,
                Union<Func<T, TResult>, Func<Option<T>, TResult>, TResult>.CreateSecond(func)));
        }

        public void AddPredicateAndValueFunc(Func<Option<T>, bool> predicate, Func<T, TResult> func)
        {
            _predicatesAndResults.Add((predicate,
                Union<Func<T, TResult>, Func<Option<T>, TResult>, TResult>.CreateFirst(func)));
        }

        public void AddPredicateAndResult(Func<Option<T>, bool> predicate, TResult result)
        {
            _predicatesAndResults.Add((predicate, 
                Union<Func<T, TResult>, Func<Option<T>, TResult>, TResult>.CreateThird(result)));
        }

        public TResult GetMatchedOrDefaultResult(Option<T> item)
        {
            return GetMatchedOrProvidedResult(item, _defaultFunc);
        }

        public TResult GetMatchedOrProvidedResult(Option<T> item, Func<Option<T>, TResult> elseFunc)
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