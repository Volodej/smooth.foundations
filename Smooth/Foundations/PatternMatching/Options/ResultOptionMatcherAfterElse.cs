using System;
using Smooth.Algebraics;

namespace Smooth.Foundations.PatternMatching.Options
{
    public sealed class ResultOptionMatcherAfterElse<T, TResult>
    {
        private readonly Option<T> _item;
        private readonly Either<TResult, Func<Option<T>, TResult>> _result; 
        private readonly FuncSelectorForOption<T, TResult> _funcSelector;

        internal ResultOptionMatcherAfterElse(FuncSelectorForOption<T, TResult> funcSelector,
                                              Func<Option<T>, TResult> elseResult, 
                                              Option<T> item)
        {
            _funcSelector = funcSelector;
            _result = Either<TResult, Func<Option<T>, TResult>>.Right(elseResult);
            _item = item;
        }

        internal ResultOptionMatcherAfterElse(FuncSelectorForOption<T, TResult> funcSelector,
                                              TResult elseResult,
                                              Option<T> item)
        {
            _funcSelector = funcSelector;
            _result = Either<TResult, Func<Option<T>, TResult>>.Left(elseResult);
            _item = item;
        }

        public TResult Result()
        {
            return _result.isLeft
                ? _funcSelector.GetMatchedOrProvidedResult(_item, _result.leftValue)
                : _funcSelector.GetMatchedOrProvidedResult(_item, _result.rightValue);
        }
    }
}