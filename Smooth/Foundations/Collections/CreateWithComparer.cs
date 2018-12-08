using System.Collections.Generic;

namespace Smooth.Collections {

	#region Dictionary

	/// <summary>
	/// Helper class for instantiating Dictionary<>s without specifying a comparer.
	/// </summary>
	public static class Dictionary {
		
		/// <summary>
		/// Creates a new dictionary with the default comparer.
		/// </summary>
		public static Dictionary<K, V> Create<K, V>() {
			return new Dictionary<K, V>(EqualityComparer<K>.Default);
		}
		
		/// <summary>
		/// Creates a new dictionary with the default comparer and the specified initial capacity.
		/// </summary>
		public static Dictionary<K, V> Create<K, V>(int capacity) {
			return new Dictionary<K, V>(capacity, EqualityComparer<K>.Default);
		}
		
		/// <summary>
		/// Creates a new dictionary with the default comparer and elements copied from the specified dictionary.
		/// </summary>
		public static Dictionary<K, V> Create<K, V>(IDictionary<K, V> dictionary) {
			return new Dictionary<K, V>(dictionary, EqualityComparer<K>.Default);
		}
		
	}

	#endregion

	#region HashSet
	
	/// <summary>
	/// Helper class for instantiating HashSet<>s without specifying a comparer.
	/// </summary>
	public static class HashSet {
		
		/// <summary>
		/// Creates a new hash set with the default comparer.
		/// </summary>
		public static HashSet<T> Create<T>() {
			return new HashSet<T>(EqualityComparer<T>.Default);
		}
		
		/// <summary>
		/// Creates a new hash set with the default comparer and elements copied from the specified collection.
		/// </summary>
		public static HashSet<T> Create<T>(IEnumerable<T> collection) {
			return new HashSet<T>(collection, EqualityComparer<T>.Default);
		}

	}
	
	#endregion

	#region SortedDictionary
	
	/// <summary>
	/// Helper class for instantiating SortedDictionary<>s without specifying a comparer.
	/// </summary>
	public static class SortedDictionary {
		
		/// <summary>
		/// Creates a new sorted dictionary with the default comparer.
		/// </summary>
		public static SortedDictionary<K, V> Create<K, V>() {
			return new SortedDictionary<K, V>(Comparer<K>.Default);
		}
		
		/// <summary>
		/// Creates a new sorted dictionary with the default comparer and elements copied from the specified dictionary.
		/// </summary>
		public static SortedDictionary<K, V> Create<K, V>(IDictionary<K, V> dictionary) {
			return new SortedDictionary<K, V>(dictionary, Comparer<K>.Default);
		}

	}
	
	#endregion
	
	#region SortedList

	/// <summary>
	/// Helper class for instantiating SortedList<>s without specifying a comparer.
	/// </summary>
	public static class SortedList {

		/// <summary>
		/// Creates a new sorted list with the default comparer.
		/// </summary>
		public static SortedList<K, V> Create<K, V>() {
			return new SortedList<K, V>(Comparer<K>.Default);
		}
		
		/// <summary>
		/// Creates a new sorted list with the default comparer and the specified initial capacity.
		/// </summary>
		public static SortedList<K, V> Create<K, V>(int capacity) {
			return new SortedList<K, V>(capacity, Comparer<K>.Default);
		}
		
		/// <summary>
		/// Creates a new sorted list with the default comparer and elements copied from the specified dictionary.
		/// </summary>
		public static SortedList<K, V> Create<K, V>(IDictionary<K, V> dictionary) {
			return new SortedList<K, V>(dictionary, Comparer<K>.Default);
		}
	}
	
	#endregion

}
