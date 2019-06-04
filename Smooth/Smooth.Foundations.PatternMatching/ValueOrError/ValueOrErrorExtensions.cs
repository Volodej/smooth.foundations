using Smooth.Foundations.Algebraics;
using Smooth.Foundations.PatternMatching.ValueOrError.Function;

namespace Smooth.Foundations.PatternMatching.ValueOrError
{
    public static class ValueOrErrorExtensions
    {
        public static ValueOrErrorMatcher<T> Match<T>(this ValueOrError<T> valueOrError)
        {
            return new ValueOrErrorMatcher<T>(valueOrError);
        }

        public static ValueOrErrorResultMatcher<T, TResult> MatchTo<T, TResult>(this ValueOrError<T> valueOrError)
        {
            return new ValueOrErrorResultMatcher<T, TResult>(valueOrError);
        }
    }
}
