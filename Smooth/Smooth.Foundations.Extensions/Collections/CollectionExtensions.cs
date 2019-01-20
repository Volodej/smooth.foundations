using System.Collections.Generic;

namespace Smooth.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static TCollection WithItem<TItem, TCollection>(this TCollection collection, TItem item)
            where TCollection : ICollection<TItem>
        {
            collection.Add(item);
            return collection;
        }

        public static TItem AddTo<TItem, TCollection>(this TItem item, TCollection collection)
            where TCollection : ICollection<TItem>
        {
            collection.Add(item);
            return item;
        }
    }
}