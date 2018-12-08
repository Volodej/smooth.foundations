using System;
using System.Collections.Generic;

namespace Smooth.Tests.Utils
{
    public class Equals_1<T1, T2> : IEquatable<Equals_1<T1, T2>>, IEqualityComparer<ValueTuple<T1, T2>> {
        public readonly IEqualityComparer<T1> EqualityComparer;

        public Equals_1()
        {
            EqualityComparer = Collections.EqualityComparer<T1>.Default;
        }

        public Equals_1(IEqualityComparer<T1> equalityComparer) {
            this.EqualityComparer = equalityComparer;
        }
			
        public override bool Equals(object o) {
            return o is Equals_1<T1, T2> && Equals((Equals_1<T1, T2>) o);
        }
			
        public bool Equals(Equals_1<T1, T2> other) {
            return EqualityComparer == other.EqualityComparer;
        }
			
        public override int GetHashCode() {
            return EqualityComparer.GetHashCode();
        }
			
        public static bool operator == (Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs) {
            return lhs.Equals(rhs);
        }
			
        public static bool operator != (Equals_1<T1, T2> lhs, Equals_1<T1, T2> rhs) {
            return !lhs.Equals(rhs);
        }

        public bool Equals(ValueTuple<T1, T2> a, ValueTuple<T1, T2> b) {
            return EqualityComparer.Equals(a.Item1, b.Item1);
        }

        public int GetHashCode(ValueTuple<T1, T2> a) {
            return EqualityComparer.GetHashCode(a.Item1);
        }
    }
}