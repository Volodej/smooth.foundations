using System.Collections.Generic;
using Smooth.Algebraics;

namespace Smooth.Extensions.Collections
{
    public static class ListExtensions
    {
        public static Option<T> GetByIndex<T>(this IList<T> list, int index)
        {
            return index >= list.Count || index < 0
                ? Option<T>.None
                : list[index].ToSome();
        }

        public static void InsertOrAdd<T>(this IList<T> list, int index, T element)
        {
            if (index < 0 || index >= list.Count)
                list.Add(element);
            else
                list.Insert(index, element);
        }
    }
}
