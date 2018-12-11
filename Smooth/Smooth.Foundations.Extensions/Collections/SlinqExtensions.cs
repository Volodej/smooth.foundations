using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.Algebraics;
using Smooth.Dispose;
using Smooth.Pools;
using Smooth.Slinq;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;

namespace Smooth.Extensions.Collections
{
    public static class SlinqExtensions
    {
        public static Slinq<(T, int), IListContext<T>> SlinqWithIndex<T>(this Disposable<List<T>> disposableList,
            int startIndex = 0)
            => disposableList.value.SlinqWithIndex(startIndex);

        public static Slinq<(T, int), IListContext<T>> SlinqWithIndex<T, TList>(
            this Disposable<TList> disposableList, int startIndex = 0)
            where TList : IList<T>
            => disposableList.value.SlinqWithIndex(startIndex);

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

        public static async Task<Slinq<T, IListContext<T>>> ToAsync<T, C>(this Slinq<Task<T>, C> slinq)
        {
            var list = slinq.ToList(); //ToList borrows from pool.
            var results = await Task.WhenAll(list);
            ListPool<Task<T>>.Instance.Release(list);
            return results.Slinq();
        }

        public static async Task<List<T>> ToListAsync<T, C>(this Task<Slinq<T, C>> slinq)
        {
            return (await slinq).ToList(); //ToList borrows from pool.
        }

        public static async Task<LinkedHeadTail<T>> ToLinkedAsync<T, C>(this Task<Slinq<T, C>> slinq)
        {
            return (await slinq).ToLinked();
        }

        public static async Task<Slinq<TResult, SelectContext<TResult, T, C>>> SelectAsync<T, C, TResult>(this Task<Slinq<T, C>> slinqTask,
            Func<T, TResult> selector)
        {
            var slinq = await slinqTask;
            return slinq.Select(selector);
        }

        public static async Task<Slinq<TResult, IListContext<TResult>>> SelectAsync<T, C, TResult>(this Task<Slinq<T, C>> slinqTask,
            Func<T, Task<TResult>> selector)
        {
            var slinq = await slinqTask;
            return await slinq.Select(selector).ToAsync();
        }

        public static async Task<Slinq<TResult, SelectSlinqContext<TResult, C2, T, C>>> SelectManyAsync<T, C, C2, TResult>(
            this Task<Slinq<T, C>> slinqTask, Func<T, Slinq<TResult, C2>> selector)
        {
            var slinq = await slinqTask;
            return slinq.SelectMany(selector);
        }

        public static async Task<Slinq<T, PredicateContext<T, C>>> WhereAsync<T, C>(this Task<Slinq<T, C>> slinqTask,
            Func<T, bool> predicate)
        {
            var slinq = await slinqTask;
            return slinq.Where(predicate);
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
                else
                {
                    currentList.Add(value);
                    return t;
                }
            }, batchSize).Item1;
            return result;
        }

#if UNITY_EDITOR
        public static Slinq<T, IListContext<T>> Debug<T, C>(this Slinq<T, C> slinq, Func<T, string> toString = null)
        {
            var list = slinq.ToList();
            toString = toString ?? (t => t.ToString());
            var debugString = list.Slinq().Aggregate(new StringBuilder().Append($"########## Slinq of {list.Count} items"),
                (s, t) => s.Append($"\n {toString(t)}"));
            debugString.Append("\n ########## end of Slinq \n\n");
            UnityEngine.Debug.Log(debugString.ToString());
            return list.Slinq();
        }
#endif


#if !UNITY_EDITOR
        public static Slinq<T, C> Debug<T, C>(this Slinq<T, C> slinq, Func<T, string> toString = null)
        {
            return slinq;
        }
#endif
    }

    public static class SlinqableEx
    {
        public static Slinq<T, ConcatContext<OptionContext<T>, T, OptionContext<T>>> Two<T>(this T item, T item2)
        {
            return item.ToSome().Slinq().Concat(item2.ToOption().Slinq());
        }
    }
}