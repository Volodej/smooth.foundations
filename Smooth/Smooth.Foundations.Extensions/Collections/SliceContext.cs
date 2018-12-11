using System;
using Smooth.Algebraics;
using Smooth.Slinq;
using Smooth.Slinq.Context;
using Utils.Collections;

namespace Smooth.Extensions.Collections
{
    public struct SliceContext<T>
    {

        public static Slinq<T, SliceContext<T>> Slinq(Slice<T> slice)
        {
            return new Slinq<T, SliceContext<T>>(
                skip,
                remove,
                dispose,
                new SliceContext<T>(slice));
        }

        public static Slinq<(T, int), SliceContext<T>> SlinqWithIndex(Slice<T> slice)
        {
            return new Slinq<(T, int), SliceContext<T>>(
                skipWithIndex,
                removeWithIndex,
                disposeWithIndex,
                new SliceContext<T>(slice));
        }

        private readonly Slice<T> _slice;
        private readonly int _size;
        private int _index;

        private SliceContext(Slice<T> slice)
        {
            _slice = slice;
            _size = slice.Length;
            _index = -1;
        } 

        private static readonly Mutator<T, SliceContext<T>> skip = Skip;
        private static readonly Mutator<T, SliceContext<T>> remove = Remove;
        private static readonly Mutator<T, SliceContext<T>> dispose = Dispose;

        private static readonly Mutator<(T, int), SliceContext<T>> skipWithIndex = SkipWithIndex;
        private static readonly Mutator<(T, int), SliceContext<T>> removeWithIndex = RemoveWithIndex;
        private static readonly Mutator<(T, int), SliceContext<T>> disposeWithIndex = DisposeWithIndex;

        private static void Skip(ref SliceContext<T> context, out Option<T> next)
        {
            var index = context._index + 1;

            if (0 <= index && index < context._size)
            {
                next = new Option<T>(context._slice[index]);
                context._index = index;
            }
            else
            {
                next = new Option<T>();
            }
        }

        private static void Remove(ref SliceContext<T> context, out Option<T> next)
        {
            throw new NotSupportedException();
        }

        private static void Dispose(ref SliceContext<T> context, out Option<T> next)
        {
            next = new Option<T>();
        }

        private static void SkipWithIndex(ref SliceContext<T> context, out Option<(T, int)> next)
        {
            var index = context._index + 1;

            if (0 <= index && index < context._size)
            {
                next = new Option<(T, int)>((context._slice[index], index)); // context._slice[index]
                context._index = index;
            }
            else
            {
                next = new Option<(T, int)>();
            }
        }

        private static void RemoveWithIndex(ref SliceContext<T> context, out Option<(T, int)> next)
        {
            throw new NotSupportedException();
        }

        private static void DisposeWithIndex(ref SliceContext<T> context, out Option<(T, int)> next)
        {
            next = new Option<(T, int)>();
        }
    }
}