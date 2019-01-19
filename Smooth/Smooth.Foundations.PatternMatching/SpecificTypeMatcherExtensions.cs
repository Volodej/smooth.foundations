using System;
using Smooth.Algebraics;
using Smooth.Foundations.PatternMatching.Options;

namespace Smooth.Foundations.PatternMatching
{
    /// <summary>
    /// Defines extension methods for supplying Match() to specific types. Due to the way extension methods are resolved
    /// by the compiler, these are placed "closer" to the calling code (ie with a shorter namespace) than the general 
    /// type extension method to ensure these are chosen in preference to the general one.
    /// </summary>
    public static class SpecificTypeMatcherExtensions
    {
        public static ExecMatcher<T> Match<T>(this ValueTuple<T> item) => new ExecMatcher<T>(item.Item1);

        public static ExecMatcher<T1, T2> Match<T1, T2>(this ValueTuple<T1, T2> item) =>
            new ExecMatcher<T1, T2>(item.Item1, item.Item2);

        public static ExecMatcher<T1, T2, T3> Match<T1, T2, T3>(this ValueTuple<T1, T2, T3> item) =>
            new ExecMatcher<T1, T2, T3>(item.Item1, item.Item2, item.Item3);

        public static ExecMatcher<T1, T2, T3, T4> Match<T1, T2, T3, T4>(this ValueTuple<T1, T2, T3, T4> item) =>
            new ExecMatcher<T1, T2, T3, T4>(item.Item1, item.Item2, item.Item3, item.Item4);

        public static OptionMatcher<T> Match<T>(this Option<T> item) => new OptionMatcher<T>(item);

        public static ResultOptionMatcher<T, TResult> MatchTo<T, TResult>(this Option<T> item) =>
            new ResultOptionMatcher<T, TResult>(item);
    }
}