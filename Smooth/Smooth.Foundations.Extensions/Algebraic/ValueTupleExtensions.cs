using System;
using Smooth.Algebraics;

namespace Smooth.Extensions.Algebraic
{
    public static class ValueTupleExtensions
    {
        public static (T1, T2) PipeAndReturn<T1, T2>(this (T1, T2) tuple, Action<T1, T2> action)
        {
            action(tuple.Item1, tuple.Item2);
            return tuple;
        }

        public static (T1, T2) Convert<T1, T2, T3, T4>(this (T3, T4) tuple, 
            Func<T3, T1> func1, Func<T4, T2> func2)
        {
            return (func1(tuple.Item1), func2(tuple.Item2));
        }

        public static Option<(T1, T2)> Strict<T1, T2>(this (Option<T1>, Option<T2>) tuple)
        {
            if (tuple.Item1.isSome && tuple.Item2.isSome)
            {
                return Option.Some((tuple.Item1.value, tuple.Item2.value));
            }
            return Option<(T1, T2)>.None;
        }

        public static Option<(T1, T2, T3)> Strict<T1, T2, T3>(this (Option<T1>, Option<T2>, Option<T3>) tuple)
        {
            if (tuple.Item1.isSome && tuple.Item2.isSome && tuple.Item3.isSome)
            {
                return Option.Some((tuple.Item1.value, tuple.Item2.value, tuple.Item3.value));
            }
            return Option<(T1, T2, T3)>.None;
        }

        public static Option<(T1, T2, T3, T4)> Strict<T1, T2, T3, T4>(this (Option<T1>, Option<T2>, Option<T3>, Option<T4>) tuple)
        {
            if (tuple.Item1.isSome && tuple.Item2.isSome && tuple.Item3.isSome && tuple.Item4.isSome)
            {
                return Option.Some((tuple.Item1.value, tuple.Item2.value, tuple.Item3.value, tuple.Item4.value));
            }
            return Option<(T1, T2, T3, T4)>.None;
        }

        public static TResult Select<T1, T2, TParam, TResult>(this (T1, T2) tuple,
            Func<T1, T2,  TParam, TResult> func, TParam param)
        {
            return func(tuple.Item1, tuple.Item2, param);
        }

        public static TResult Select<T1, T2, T3, TParam, TResult>(this (T1, T2, T3) tuple,
            Func<T1, T2, T3, TParam, TResult> func, TParam param)
        {
            return func(tuple.Item1, tuple.Item2, tuple.Item3, param);
        }

        public static TResult Select<T1, T2, T3, T4, TParam, TResult>(this (T1, T2, T3, T4) tuple,
            Func<T1, T2, T3, T4, TParam, TResult> func, TParam param)
        {
            return func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, param);
        }

        public static TResult Select<T1, T2, TResult>(this (T1, T2) tuple,
            Func<T1, T2, TResult> func)
        {
            return func(tuple.Item1, tuple.Item2);
        }

        public static TResult Select<T1, T2, T3, TResult>(this (T1, T2, T3) tuple,
            Func<T1, T2, T3, TResult> func)
        {
            return func(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public static TResult Select<T1, T2, T3, T4, TResult>(this (T1, T2, T3, T4) tuple,
            Func<T1, T2, T3, T4, TResult> func)
        {
            return func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        public static TResult Select<T1, T2, T3, T4, T5, TResult>(this (T1, T2, T3, T4, T5) tuple,
       Func<T1, T2, T3, T4, T5, TResult> func)
        {
            return func(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
        }

        public static void Call<T1, T2>(this (T1, T2) tuple, Action<T1, T2> action)
        {
            action(tuple.Item1, tuple.Item2);
        }

        public static void Call<T1, T2, T3>(this (T1, T2, T3) tuple, Action<T1, T2, T3> action)
        {
            action(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public static void Call<T1, T2, T3, T4>(this (T1, T2, T3, T4) tuple, Action<T1, T2, T3, T4> action)
        {
            action(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4);
        }

        public static void Call<T1, T2, TParam>(this (T1, T2) tuple, Action<T1, T2, TParam> action, TParam param)
        {
            action(tuple.Item1, tuple.Item2, param);
        }

        public static void Call<T1, T2, T3, TParam>(this (T1, T2, T3) tuple, Action<T1, T2, T3, TParam> action, TParam param)
        {
            action(tuple.Item1, tuple.Item2, tuple.Item3, param);
        }

        public static void Call<T1, T2, T3, T4, TParam>(this (T1, T2, T3, T4) tuple, Action<T1, T2, T3, T4, TParam> action, TParam param)
        {
            action(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, param);
        }

        public static (T1, T2, T3) Flatten<T1, T2, T3>(this ((T1, T2), T3) tuple)
        {
            return (tuple.Item1.Item1, tuple.Item1.Item2, tuple.Item2);
        }

        public static (T1, T2, T3, T4) Flatten<T1, T2, T3, T4>(this ((T1, T2), (T3, T4)) tuple)
        {
            return (tuple.Item1.Item1, tuple.Item1.Item2, tuple.Item2.Item1, tuple.Item2.Item2);
        }
    }
}
