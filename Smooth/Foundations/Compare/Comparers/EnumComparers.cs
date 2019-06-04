using System;
using Smooth.Collections;
using Smooth.Conversion;

namespace Smooth.Compare.Comparers
{
    /// <summary>
    /// Fast, allocation free equality comparer for enums with an underlying size of 32 bits or less.
    /// </summary>
    public class Enum32EqualityComparer<T> : EqualityComparer<T>
    {
        internal Enum32EqualityComparer()
        {
        }

        public override bool Equals(T t1, T t2)
        {
            return EnumConverter.ToInt32RuntimeCheck(t1) == EnumConverter.ToInt32RuntimeCheck(t2);
        }

        public override int GetHashCode(T t)
        {
            return EnumConverter.ToInt32RuntimeCheck(t);
        }
    }

    /// <summary>
    /// Fast, allocation free equality comparer for enums with an underlying size of 64 bits or less.
    /// </summary>
    public class Enum64EqualityComparer<T> : EqualityComparer<T>
    {
        internal Enum64EqualityComparer()
        {
        }

        public override bool Equals(T t1, T t2)
        {
            return EnumConverter.ToInt64RuntimeCheck(t1) == EnumConverter.ToInt64RuntimeCheck(t2);
        }

        public override int GetHashCode(T t)
        {
            return EnumConverter.ToInt64RuntimeCheck(t).GetHashCode();
        }
    }

    public static class Enum32EqualityComparer
    {
        /// <summary>
        /// Create equality comparer for enum with an underlying size of 32 or less with runtime check for enum
        /// </summary>
        public static Enum32EqualityComparer<T> CreateUnsafe<T>()
        {
            EnumEqualityComparer.CheckIsEnum<T, Enum32EqualityComparer<T>>();
            return new Enum32EqualityComparer<T>();
        }

        public static Enum32EqualityComparer<T> Create<T>() where T : struct, IConvertible
        {
            EnumEqualityComparer.CheckIsEnum<T, Enum32EqualityComparer<T>>();
            return new Enum32EqualityComparer<T>();
        }
    }

    public static class Enum64EqualityComparer
    {
        /// <summary>
        /// Create equality comparer for enum with an underlying size of 64 or less with runtime check for enum
        /// </summary>
        public static Enum64EqualityComparer<T> CreateUnsafe<T>()
        {
            EnumEqualityComparer.CheckIsEnum<T, Enum64EqualityComparer<T>>();
            return new Enum64EqualityComparer<T>();
        }

        public static Enum64EqualityComparer<T> Create<T>() where T : struct, IConvertible
        {
            EnumEqualityComparer.CheckIsEnum<T, Enum64EqualityComparer<T>>();
            return new Enum64EqualityComparer<T>();
        }
    }

    internal static class EnumEqualityComparer
    {
        public static void CheckIsEnum<T, TConverter>() where TConverter : EqualityComparer<T>
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException(
                    $"Can't create converter of type '{typeof(TConverter).Name}' because {typeof(T).Name} is not an enum");
        }
    }

//	/// <summary>
//	/// Fast, allocation free IEqualityComparer<T> for Enums that uses System.Reflection.Emit to create JIT complied equality and hashCode functions.
//	/// 
//	/// Note: This class isn't any faster than Blittable32EqualityComparer or Blittable64EqualityComparer and doesn't work on platforms without JIT complilation.
//	/// 
//	/// It is provided simply as example code.
//	/// </summary>
//	public class EnumEmitEqualityComparer<T> : Smooth.Collections.EqualityComparer<T> {
//		private readonly Func<T, T, bool> equals;
//		private readonly Func<T, int> hashCode;
//		
//		public EnumEmitEqualityComparer() {
//			var type = typeof(T);
//			
//			if (type.IsEnum) {
//				var l = Expression.Parameter(type, "l");
//				var r = Expression.Parameter(type, "r");
//				
//				this.equals = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(l, r), l, r).Compile();
//				
//				switch (Type.GetTypeCode(type)) {
//				case TypeCode.Int64:
//				case TypeCode.UInt64:
//					this.hashCode = Expression.Lambda<Func<T, int>>(Expression.Call(Expression.Convert(l, typeof(Int64)), typeof(Int64).GetMethod("GetHashCode")), l).Compile();
//					break;
//				default:
//					this.hashCode = Expression.Lambda<Func<T, int>>(Expression.Convert(l, typeof(Int32)), l).Compile();
//					break;
//				}
//			} else {
//				throw new ArgumentException(GetType().Name + " can only be used with enum types.");
//			}
//		}
//		
//		public override bool Equals(T t1, T t2) {
//			return equals(t1, t2);
//		}
//		
//		public override int GetHashCode(T t) {
//			return hashCode(t);
//		}
//	}
}