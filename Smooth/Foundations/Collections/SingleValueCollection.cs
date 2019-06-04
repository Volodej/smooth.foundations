using System;
using System.Collections;
using System.Collections.Generic;

namespace Smooth.Collections
{
    public sealed class SingleValueCollection<T> : IReadOnlyList<T>
    {
        private readonly T _value;

        public SingleValueCollection(T value)
        {
            _value = value;
        }

        public int Count => 1;

        public T this[int index] => index == 0
            ? _value
            : throw new IndexOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection.");

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new SingleEnumerator(_value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new SingleEnumerator(_value);
        }

        // Created for duck typing in foreach statement
        public SingleEnumerator GetEnumerator()
        {
            return new SingleEnumerator(_value);
        }

        public struct SingleEnumerator : IEnumerator<T>
        {
            private MoveStatus _moveStatus;
            private readonly T _value;

            public SingleEnumerator(T value)
            {
                _value = value;
                _moveStatus = MoveStatus.NotMoved;
            }

            public void Dispose()
            {
                _moveStatus = MoveStatus.OutOfValue;
            }

            public bool MoveNext()
            {
                _moveStatus = _moveStatus == MoveStatus.NotMoved ? MoveStatus.OnValue : MoveStatus.OutOfValue;
                return _moveStatus == MoveStatus.OnValue;
            }

            public void Reset()
            {
                _moveStatus = MoveStatus.NotMoved;
            }

            public T Current => _moveStatus == MoveStatus.OnValue ? _value : default;

            object IEnumerator.Current => Current;
        }

        private enum MoveStatus : byte
        {
            NotMoved,
            OnValue,
            OutOfValue
        }
    }

    public static class SingleValueEnumerable
    {
        public static SingleValueCollection<T> FromValue<T>(T value)
        {
            return new SingleValueCollection<T>(value);
        }
    }
}