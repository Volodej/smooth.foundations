using System.Collections.Generic;
using System.Linq;
using Smooth.Collections;

namespace Utils.Collections
{
    public static class SliceExtensions
    {
        public static Slice<T> ToSlice<T>(this IEnumerable<T> item)
        {
            var list = item as IList<T> ?? item.ToList();
            return new Slice<T>(list);
        }
    }
}

// TODO: fix
// SliceGeneralExtensions class has been moved to another namespace because its method extension 'ToSlice' overlaps other methods.
namespace Utils.Collections.Other
{
    public static class SliceGeneralExtensions
    {
        public static Slice<T> ToSlice<T>(this T item)
        {
            return new Slice<T>(item);
        }
    }
}