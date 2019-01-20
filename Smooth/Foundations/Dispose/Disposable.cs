using System;
using Smooth.Pools;

namespace Smooth.Dispose
{
#if UNITY_PS3 || UNITY_XBOX360 || UNITY_WII
	/// <summary>
	/// Wrapper around a value that uses the IDisposable interface to dispose of the value.
	/// 
	/// On AOT, this is a struct to avoid compute_class_bitmap errors.
	/// 
	/// On other platforms, it is a pooled object to avoid boxing when disposed by a using block with the Unity compiler.
	/// </summary>
	public struct Disposable<T> : IDisposable {
		/// <summary>
		/// Borrows a wrapper for the specified value and disposal delegate.
		/// </summary>
		public static Disposable<T> Borrow(T value, Action<T> dispose) {
			return new Disposable<T>(value, dispose);
		}
		
		private readonly Action<T> dispose;

		/// <summary>
		/// The wrapped value.
		/// </summary>
		public readonly T value;

		public Disposable(T value, Action<T> dispose) {
			this.value = value;
			this.dispose = dispose;
		}
		
		/// <summary>
		/// Relinquishes ownership of the wrapper and disposes the wrapped value.
		/// </summary>
		public void Dispose() {
			dispose(value);
		}
		
		/// <summary>
		/// Relinquishes ownership of the wrapper and adds it to the disposal queue.
		/// </summary>
		public void DisposeInBackground() {
			DisposalQueue.Enqueue(this);
		}
	}

#else

	/// <summary>
	///     Wrapper around a value that uses the IDisposable interface to dispose of the value.
	///     On AOT, this is a value type to avoid compute_class_bitmap errors.
	///     On other platforms, it is a pooled object to avoid boxing when disposed by a using block with the Unity compiler.
	/// </summary>
	public class Disposable<T> : IDisposable
    {
        private static readonly Pool<Disposable<T>> _pool = new Pool<Disposable<T>>(
            () => new Disposable<T>(),
            wrapper =>
            {
                wrapper._dispose(wrapper.value);
                wrapper._dispose = t => { };
                wrapper.value = default;
            }
        );

        private Action<T> _dispose;

        private Disposable()
        {
        }

        /// <summary>
        ///     The wrapped value.
        /// </summary>
        public T value { get; private set; }

        /// <summary>
        ///     Relinquishes ownership of the wrapper, disposes the wrapped value, and returns the wrapper to the pool.
        /// </summary>
        public void Dispose()
        {
            _pool.Release(this);
        }

        /// <summary>
        ///     Borrows a wrapper for the specified value and disposal delegate.
        /// </summary>
        public static Disposable<T> Borrow(T value, Action<T> dispose)
        {
            var wrapper = _pool.Borrow();
            wrapper.value = value;
            wrapper._dispose = dispose;
            return wrapper;
        }

        /// <summary>
        ///     Relinquishes ownership of the wrapper and adds it to the disposal queue.
        /// </summary>
        public void DisposeInBackground()
        {
            DisposalQueue.Enqueue(this);
        }
    }

#endif
}