using System;
using System.Collections;
using System.Collections.Generic;
using Smooth.Algebraics;
using Smooth.Slinq;

namespace Smooth.Collections
{
    public struct Slice<T> : IEnumerable<T>
    {
        public static Slice<T> Empty => new Slice<T>(null, 0, 0);

        public int Length { get; }

        private readonly Either<IList<T>, T> _obj;
        private readonly int _offset;

        public Slice(IList<T> array, int start, int length)
        {
            _obj = Either<IList<T>, T>.Left(array);
            _offset = start;
            Length = length - start;
        }

        public Slice(IList<T> array, int start = 0)
        {
            _obj = Either<IList<T>, T>.Left(array);
            _offset = start;
            Length = array.Count - start;
        }

        public Slice(T item)
        {
            _obj = Either<IList<T>, T>.Right(item);
            _offset = 0;
            Length = 1;
        }

        public T this[int index]
        {
            get
            {
                var realIndex = _offset + index;
                if (_obj.isLeft)
                    return _obj.leftValue[realIndex];
                if (realIndex == 0)
                    return _obj.rightValue;
                throw new IndexOutOfRangeException();
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        public Slinq<T, SliceContext<T>> Slinq()
        {
            return SliceContext<T>.Slinq(this);
        }

        public Slinq<(T, int), SliceContext<T>> SlinqWithIndex()
        {
            return SliceContext<T>.SlinqWithIndex(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public struct Enumerator : IEnumerator<T>
        {
            private Slice<T> _slice;
            private int _position;

            public Enumerator(Slice<T> slice)
            {
                _slice = slice;
                _position = -1;
            }

            public T Current => _slice[_position];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                _slice = default;
                _position = -1;
            }

            public bool MoveNext()
            {
                return ++_position < _slice.Length;
            }

            public void Reset()
            {
                _position = -1;
            }
        }
    }
}