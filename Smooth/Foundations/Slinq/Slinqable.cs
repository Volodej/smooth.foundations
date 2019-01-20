using System;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq.Context;

namespace Smooth.Slinq
{
	/// <summary>
	///     Provides methods for creating basic Slinqs from various underlying collections or delegates.
	/// </summary>
	public static class Slinqable
    {
        #region Empty

        /// <summary>
        ///     Returns an empty Slinq of the specified type.
        /// </summary>
        public static Slinq<T, Unit> Empty<T>()
        {
            return new Slinq<T, Unit>();
        }

        #endregion

        #region IEnumerable

        /// <summary>
        ///     Returns a Slinq that enumerates over the specified enumerable.
        ///     The returned Slinq will be backed by an IEnumerator
        ///     <T>
        ///         , and thus will allocate an object or box.
        ///         Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, IEnumerableContext<T>> Slinq<T>(this IEnumerable<T> enumerable)
        {
            return IEnumerableContext<T>.Slinq(enumerable);
        }

        #endregion

        #region Option

        /// <summary>
        ///     Returns a Slinq that enumerates the specified option.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, OptionContext<T>> Slinq<T>(this Option<T> option)
        {
            return OptionContext<T>.Slinq(option);
        }

        #endregion

        #region Range

        /// <summary>
        ///     Returns a Slinq that enumerates the integers within a specified range.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<int, FuncOptionContext<int, int>> Range(int start, int count)
        {
            if (count > 0)
            {
                var max = start + count - 1;
                if (max >= start)
                    return FuncOptionContext<int, int>.Sequence(start,
                        (acc, last) => ++acc > last ? new Option<int>() : new Option<int>(acc), max);
                throw new ArgumentOutOfRangeException();
            }

            if (count == 0)
                return new Slinq<int, FuncOptionContext<int, int>>();
            throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region IList

        #region Explicit step

        /// <summary>
        ///     Returns a Slinq that enumerates over the elements of the specified list using the specified start index and step.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex, int step)
        {
            return IListContext<T>.Slinq(list, startIndex, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the element, index pairs of the specified list using the specified start index and step.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<ValueTuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex, int step)
        {
            return IListContext<T>.SlinqWithIndex(list, startIndex, step);
        }

        #endregion

        #region Ascending

        /// <summary>
        ///     Returns a Slinq that enumerates over the elements of the specified list in ascending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list)
        {
            return IListContext<T>.Slinq(list, 0, 1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the element, index pairs of the specified list in ascending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<ValueTuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list)
        {
            return IListContext<T>.SlinqWithIndex(list, 0, 1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the elements of the specified list in ascending order starting with the specified index.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, IListContext<T>> Slinq<T>(this IList<T> list, int startIndex)
        {
            return IListContext<T>.Slinq(list, startIndex, 1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the element, index pairs of the specified list in ascending order starting with the specified
        ///     index.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<ValueTuple<T, int>, IListContext<T>> SlinqWithIndex<T>(this IList<T> list, int startIndex)
        {
            return IListContext<T>.SlinqWithIndex(list, startIndex, 1);
        }

        #endregion

        #region Descending

        /// <summary>
        ///     Returns a Slinq that enumerates over the elements of the specified list in descending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list)
        {
            return IListContext<T>.Slinq(list, list.Count - 1, -1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the element, index pairs of the specified list in descending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<ValueTuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list)
        {
            return IListContext<T>.SlinqWithIndex(list, list.Count - 1, -1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the elements of the specified list in decending order starting with the specified index.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, IListContext<T>> SlinqDescending<T>(this IList<T> list, int startIndex)
        {
            return IListContext<T>.Slinq(list, startIndex, -1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the element, index pairs of the specified list in decending order starting with the specified
        ///     index.
        ///     If startIndex is outside the element range of the list, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<ValueTuple<T, int>, IListContext<T>> SlinqWithIndexDescending<T>(this IList<T> list, int startIndex)
        {
            return IListContext<T>.SlinqWithIndex(list, startIndex, -1);
        }

        #endregion

        #endregion

        #region LinkedList

        #region Explicit step

        /// <summary>
        ///     Returns a Slinq that starts with the value of the specified node and proceeds along node links.
        ///     If step is positive, the Slinq will move along Next links, if step is negative the Slinq will move along Previous links.  If step is
        ///     zero the Slinq will stay in place.
        ///     If the specified node is null, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedListNode<T> node, int step)
        {
            return LinkedListContext<T>.Slinq(node, step);
        }

        /// <summary>
        ///     Returns a Slinq that starts with the specified node and proceeds along node links.
        ///     If step is positive, the Slinq will move along Next links, if step is negative the Slinq will move along Previous links.  If step is
        ///     zero the Slinq will stay in place.
        ///     If the specified node is null, the resulting Slinq will be empty.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedListNode<T> node, int step)
        {
            return LinkedListContext<T>.SlinqNodes(node, step);
        }

        #endregion

        #region Ascending / Descending

        /// <summary>
        ///     Returns a Slinq that enumerates over the values of the specified linked list in ascending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, LinkedListContext<T>> Slinq<T>(this LinkedList<T> list)
        {
            return LinkedListContext<T>.Slinq(list.First, 1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the values of the specified linked list in decending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<T, LinkedListContext<T>> SlinqDescending<T>(this LinkedList<T> list)
        {
            return LinkedListContext<T>.Slinq(list.Last, -1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the nodes of the specified linked list in ascending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodes<T>(this LinkedList<T> list)
        {
            return LinkedListContext<T>.SlinqNodes(list.First, 1);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates over the nodes of the specified linked list in decending order.
        ///     Slinqs created by this method will chain removal operations to the underlying list.
        /// </summary>
        public static Slinq<LinkedListNode<T>, LinkedListContext<T>> SlinqNodesDescending<T>(this LinkedList<T> list)
        {
            return LinkedListContext<T>.SlinqNodes(list.Last, -1);
        }

        #endregion

        #endregion

        #region Repeat

        /// <summary>
        ///     Returns a Slinq that repeats the specified value.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, OptionContext<T>> Repeat<T>(T value)
        {
            return OptionContext<T>.Repeat(value);
        }

        /// <summary>
        ///     Returns a Slinq that repeats the specified value the specified number of times.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, IntContext<T, OptionContext<T>>> Repeat<T>(T value, int count)
        {
            if (count > 0)
                return OptionContext<T>.Repeat(value).Take(count);
            if (count == 0)
                return new Slinq<T, IntContext<T, OptionContext<T>>>();
            throw new ArgumentOutOfRangeException();
        }

        #endregion

        #region Sequence

        #region Generic Sequences

        /// <summary>
        ///     Returns a Slinq that enumerates the sequence generated by specified seed value and selector function.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, FuncContext<T>> Sequence<T>(T seed, Func<T, T> selector)
        {
            return FuncContext<T>.Sequence(seed, selector);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the sequence generated by specified seed value and selector function.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, FuncContext<T, P>> Sequence<T, P>(T seed, Func<T, P, T> selector, P parameter)
        {
            return FuncContext<T, P>.Sequence(seed, selector, parameter);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the sequence generated by specified seed value and selector function.
        ///     The enumeration will end when the selector returns a None option.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, FuncOptionContext<T>> Sequence<T>(T seed, Func<T, Option<T>> selector)
        {
            return FuncOptionContext<T>.Sequence(seed, selector);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the sequence generated by specified seed value and selector function.
        ///     The enumeration will end when the selector returns a None option.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<T, FuncOptionContext<T, P>> Sequence<T, P>(T seed, Func<T, P, Option<T>> selector, P parameter)
        {
            return FuncOptionContext<T, P>.Sequence(seed, selector, parameter);
        }

        #endregion

        #region Type specific sequences

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<byte, FuncContext<byte, byte>> Sequence(byte start, byte step)
        {
            return FuncContext<byte, byte>.Sequence(start, (x, s) => (byte) (x + s), step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<sbyte, FuncContext<sbyte, sbyte>> Sequence(sbyte start, sbyte step)
        {
            return FuncContext<sbyte, sbyte>.Sequence(start, (x, s) => (sbyte) (x + s), step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<short, FuncContext<short, short>> Sequence(short start, short step)
        {
            return FuncContext<short, short>.Sequence(start, (x, s) => (short) (x + s), step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<ushort, FuncContext<ushort, ushort>> Sequence(ushort start, ushort step)
        {
            return FuncContext<ushort, ushort>.Sequence(start, (x, s) => (ushort) (x + s), step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<int, FuncContext<int, int>> Sequence(int start, int step)
        {
            return FuncContext<int, int>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<uint, FuncContext<uint, uint>> Sequence(uint start, uint step)
        {
            return FuncContext<uint, uint>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<long, FuncContext<long, long>> Sequence(long start, long step)
        {
            return FuncContext<long, long>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<ulong, FuncContext<ulong, ulong>> Sequence(ulong start, ulong step)
        {
            return FuncContext<ulong, ulong>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<float, FuncContext<float, float>> Sequence(float start, float step)
        {
            return FuncContext<float, float>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<double, FuncContext<double, double>> Sequence(double start, double step)
        {
            return FuncContext<double, double>.Sequence(start, (x, s) => x + s, step);
        }

        /// <summary>
        ///     Returns a Slinq that enumerates the arithmetic sequence generated by the specified start and step values.
        ///     Slinqs created by this method do not support element removal.
        /// </summary>
        public static Slinq<decimal, FuncContext<decimal, decimal>> Sequence(decimal start, decimal step)
        {
            return FuncContext<decimal, decimal>.Sequence(start, (x, s) => x + s, step);
        }

        #endregion

        #endregion
    }
}