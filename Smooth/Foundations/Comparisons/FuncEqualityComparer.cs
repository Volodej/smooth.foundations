using System.Collections.Generic;
using Smooth.Delegates;

namespace Smooth.Comparisons {
	/// <summary>
	/// Performs type-specific equality comparisons and hashCode generation using the functions supplied to the constructor.
	/// </summary>
	public class FuncEqualityComparer<T> : Smooth.Collections.EqualityComparer<T> {
		private readonly DelegateFunc<T, T, bool> equals;
		private readonly DelegateFunc<T, int> hashCode;

		/// <summary>
		/// Instantiate an equality comparer for type T using the specified equality function and T.GetHashCode()
		/// </summary>
		public FuncEqualityComparer(DelegateFunc<T, T, bool> equals) {
			this.equals = equals;
			this.hashCode = typeof(T).IsClass ? (DelegateFunc<T, int>) (t => t == null ? 0 : t.GetHashCode()) : (DelegateFunc<T, int>) (t => t.GetHashCode());
		}

		/// <summary>
		/// Instantiate an equality comparer for type T with the specified equality and hashCode functions
		/// </summary>
		public FuncEqualityComparer(DelegateFunc<T, T, bool> equals, DelegateFunc<T, int> hashCode) {
			this.equals = equals;
			this.hashCode = hashCode;
		}

		public FuncEqualityComparer(IEqualityComparer<T> equalityComparer) {
			this.equals = equalityComparer.Equals;
			this.hashCode = equalityComparer.GetHashCode;
		}
		
		public override bool Equals(T t1, T t2) {
			return equals(t1, t2);
		}
		
		public override int GetHashCode(T t) {
			return hashCode(t);
		}
	}
}
