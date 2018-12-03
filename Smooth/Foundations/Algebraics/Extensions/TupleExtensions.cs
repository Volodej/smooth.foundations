using System;

namespace Smooth.Algebraics.Extensions
{
    public static class TupleExtensions
    {
        public static bool ItemsEquals<T1>(this ValueTuple<T1> left, ValueTuple<T1> right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1);

        public static bool ItemsEquals<T1, T2>(this (T1, T2) left, (T1, T2) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2);

        public static bool ItemsEquals<T1, T2, T3>(this (T1, T2, T3) left, (T1, T2, T3) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3);

        public static bool ItemsEquals<T1, T2, T3, T4>(this (T1, T2, T3, T4) left, (T1, T2, T3, T4) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3) &&
            Collections.EqualityComparer<T4>.Default.Equals(left.Item4, right.Item4);

        public static bool ItemsEquals<T1, T2, T3, T4, T5>(this (T1, T2, T3, T4, T5) left, (T1, T2, T3, T4, T5) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3) &&
            Collections.EqualityComparer<T4>.Default.Equals(left.Item4, right.Item4) &&
            Collections.EqualityComparer<T5>.Default.Equals(left.Item5, right.Item5);

        public static bool ItemsEquals<T1, T2, T3, T4, T5, T6>(this (T1, T2, T3, T4, T5, T6) left, (T1, T2, T3, T4, T5, T6) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3) &&
            Collections.EqualityComparer<T4>.Default.Equals(left.Item4, right.Item4) &&
            Collections.EqualityComparer<T5>.Default.Equals(left.Item5, right.Item5) &&
            Collections.EqualityComparer<T6>.Default.Equals(left.Item6, right.Item6);

        public static bool ItemsEquals<T1, T2, T3, T4, T5, T6, T7>(this (T1, T2, T3, T4, T5, T6, T7) left,
            (T1, T2, T3, T4, T5, T6, T7) right) =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3) &&
            Collections.EqualityComparer<T4>.Default.Equals(left.Item4, right.Item4) &&
            Collections.EqualityComparer<T5>.Default.Equals(left.Item5, right.Item5) &&
            Collections.EqualityComparer<T6>.Default.Equals(left.Item6, right.Item6) &&
            Collections.EqualityComparer<T7>.Default.Equals(left.Item7, right.Item7);

        public static bool ItemsEquals<T1, T2, T3, T4, T5, T6, T7, TRest>(this (T1, T2, T3, T4, T5, T6, T7, TRest) left,
            (T1, T2, T3, T4, T5, T6, T7, TRest) right) where TRest : struct =>
            Collections.EqualityComparer<T1>.Default.Equals(left.Item1, right.Item1) &&
            Collections.EqualityComparer<T2>.Default.Equals(left.Item2, right.Item2) &&
            Collections.EqualityComparer<T3>.Default.Equals(left.Item3, right.Item3) &&
            Collections.EqualityComparer<T4>.Default.Equals(left.Item4, right.Item4) &&
            Collections.EqualityComparer<T5>.Default.Equals(left.Item5, right.Item5) &&
            Collections.EqualityComparer<T6>.Default.Equals(left.Item6, right.Item6) &&
            Collections.EqualityComparer<T7>.Default.Equals(left.Item7, right.Item7) &&
            left.Rest.ItemsEquals(right.Rest);
    }
}