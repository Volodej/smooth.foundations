using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Dispose;
using Smooth.Pools;
using Smooth.Slinq;
using Smooth.Slinq.Context;

namespace Smooth.Extensions.Collections
{
    public static class SlinqExtensions
    {
        public static Slinq<(T, int), IListContext<T>> SlinqWithIndex<T>(this Disposable<List<T>> disposableList,
            int startIndex = 0)
        {
            return disposableList.value.SlinqWithIndex(startIndex);
        }

        public static Slinq<(T, int), IListContext<T>> SlinqWithIndex<T, TList>(
            this Disposable<TList> disposableList, int startIndex = 0)
            where TList : IList<T>
        {
            return disposableList.value.SlinqWithIndex(startIndex);
        }

        public static T[] ToArray<T, C>(this Slinq<T, C> slinq)
        {
            using (var list = ListPool<T>.Instance.BorrowDisposable())
            {
                slinq.AddTo(list);
                return list.value.ToArray();
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<T, C, TKey, TValue>(this Slinq<T, C> slinq, Func<T, TKey> keyFunc,
            Func<T, TValue> valueFunc)
        {
            return slinq.Select((item, tuple) => new KeyValuePair<TKey, TValue>(tuple.Item1(item), tuple.Item2(item)),
                    Tuple.Create(keyFunc, valueFunc))
                .AddTo(DictionaryPool<TKey, TValue>.Instance.Borrow());
        }

        public static Dictionary<K, V> ToDictionary<K, V, C>(this Slinq<(K, V), C> slinq)
        {
            return slinq.AddTo(DictionaryPool<K, V>.Instance.Borrow(), tuple => new KeyValuePair<K, V>(tuple.Item1, tuple.Item2));
        }

        public static List<List<T>> ToBatches<T, C>(this Slinq<T, C> slinq, int batchSize)
        {
            if (batchSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(batchSize), "Can't be negative o zero.");

            var startBatch = ListPool<T>.Instance.Borrow();
            var aggregationList = ListPool<List<T>>.Instance.Borrow();
            aggregationList.Add(startBatch);
            var result = slinq.Aggregate(Tuple.Create(aggregationList, startBatch), (t, value, maxSize) =>
            {
                var aggregation = t.Item1;
                var currentList = t.Item2;
                if (currentList.Count == maxSize)
                {
                    var newCurrent = ListPool<T>.Instance.Borrow();
                    newCurrent.Add(value);
                    aggregation.Add(newCurrent);
                    return Tuple.Create(aggregation, newCurrent);
                }

                currentList.Add(value);
                return t;
            }, batchSize).Item1;
            return result;
        }

        public static Slinq<T, ConcatContext<OptionContext<T>, T, OptionContext<T>>> With<T>(this T item1, T item2)
        {
            return item1.ToSome().Slinq().Concat(item2.ToSome().Slinq());
        }

        public static Slinq<T, ConcatContext<OptionContext<T>, T, C>> With<T, C>(this Slinq<T, C> slinq, T item2)
        {
            return slinq.Concat(item2.ToSome().Slinq());
        }
    }
}