using System.Collections.Generic;
using Smooth.Dispose;

namespace Smooth.Extensions.Collections
{
    public static class DisposableExtensions
    {
        public static Disposable<TCollection> WithItem<TItem, TCollection>(this Disposable<TCollection> disposable, TItem item)
            where TCollection : ICollection<TItem>
        {
            disposable.value.Add(item);
            return disposable;
        }

        public static TItem AddTo<TItem, TCollection>(this TItem item, Disposable<TCollection> collection)
            where TCollection : ICollection<TItem>
        {
            collection.value.Add(item);
            return item;
        }
    }
}