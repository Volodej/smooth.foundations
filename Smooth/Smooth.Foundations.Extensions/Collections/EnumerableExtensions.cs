using System;
using System.Collections.Generic;
using System.Linq;
using Smooth.Algebraics;
using Smooth.Extensions.Algebraic;
using Smooth.Pools;
using Smooth.Slinq;

namespace Smooth.Extensions.Collections
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> One<T>(this T obj)
        {
            yield return obj;
        }
        
        public static IEnumerable<R> Zip<T1, T2, R>(this IEnumerable<T1> first, IEnumerable<T2> second,
            Func<T1, T2, R> resultSelector)
        {
            if (first == null) throw new NullReferenceException(nameof(first));
            if (second == null) throw new NullReferenceException(nameof(second));
            if (resultSelector == null) throw new NullReferenceException(nameof(second));

            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
                while (e1.MoveNext() && e2.MoveNext())
                    yield return resultSelector(e1.Current, e2.Current);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
        {
            return new HashSet<T>(enumerable);
        }

        public static T FirstOr<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate, Func<T> generator)
        {
            var firstOrDefault = enumerable.Slinq().FirstOrDefault(predicate);
            return (System.Collections.Generic.EqualityComparer<T>.Default.Equals(firstOrDefault, default(T)))
                ? generator()
                : firstOrDefault;
        }

        public static string AsString<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null ? "NULL" : enumerable.Aggregate("[", (s, o) => s + (o+ "\n")) + "]";
        }

        public static List<T> OuterJoin<T, TKey>(this IEnumerable<T> leftCollection, IEnumerable<T> rightCollection,
            Func<T, TKey> keySelector, Func<T, T, T> joinSelector)
        {
            return OuterJoin(leftCollection, rightCollection, keySelector, keySelector, joinSelector, left => left, right => right);
        }

        public static List<TResult> OuterJoin<TResult, TLeft, TRight, TKey>(this IEnumerable<TLeft> leftCollection, IEnumerable<TRight> rightCollection, 
            Func<TLeft,TKey> leftKeySelector, Func<TRight, TKey> rightKeySelector,
            Func<TLeft, TRight, TResult> joinSelector, Func<TLeft, TResult> leftSelector, Func<TRight, TResult> rightSelector)
        {
            var dictionary = DictionaryPool<TKey, (Option<TLeft>, Option<TRight>)>.Instance.Borrow();
            var list = ListPool<TResult>.Instance.Borrow();

            foreach (var left in leftCollection)
            {
                dictionary.Add(leftKeySelector(left), (left.ToSome(), Option<TRight>.None));
            }

            foreach (var right in rightCollection)
            {
                var key = rightKeySelector(right);
                dictionary[key] = dictionary.TryGet(key)
                    .Cata((leftAndRight, r) => (leftAndRight.Item1, r.ToSome()), right,
                        r => (Option<TLeft>.None, r.ToSome()), right);
            }

            dictionary.Slinq()
                .Select(pair => pair.Value)
                .Select((leftAndRight, selectors) => selectors.Select(SelectOuterJoinValue, leftAndRight),
                    (joinSelector, leftSelector, rightSelector))
                .AddTo(list);

            DictionaryPool<TKey, (Option<TLeft>, Option<TRight>)>.Instance.Release(dictionary);

            return list;
        }

        private static TResult SelectOuterJoinValue<TResult, TLeft, TRight>(
            Func<TLeft, TRight, TResult> joinSelector, Func<TLeft, TResult> leftSelector,
            Func<TRight, TResult> rightSelector,
            (Option<TLeft>, Option<TRight>) leftAndRight)
            => leftAndRight.Strict()
                .Select((lAndR, selector) => selector(lAndR.Item1, lAndR.Item2), joinSelector)
                .Or(leftAndSelector => leftAndSelector.Item1.Select((left, selector) => selector(left), leftAndSelector.Item2),
                    (leftAndRight.Item1, leftSelector))
                .ValueOr(rightAndSelector => rightAndSelector.Item2(rightAndSelector.Item1.value),
                    (leftAndRight.Item2, rightSelector));
    }
}