using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.Pools;
using Smooth.Slinq;
using Smooth.Slinq.Collections;
using Smooth.Slinq.Context;

namespace Smooth.Foundations.AsyncExtensions.Collections
{
    // TODO: create correct solution for .ConfigureAwait(false)
    public static class SlinqAsyncExtensions
    {
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
    }
}