using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Smooth.Algebraics;
using Smooth.Conversion;

namespace Smooth.Extensions
{
    public static class EnumExtensions
    {
        private static readonly ReaderWriterLockSlim ParseLock = new ReaderWriterLockSlim();
        private static readonly ReaderWriterLockSlim ValuesLock = new ReaderWriterLockSlim();

        // TODO: check performance
        public static Option<T> GetEnumOrNone<T>(this string rawValue) where T : struct, IConvertible
        {
            ParseLock.EnterReadLock();
            if (EnumFromStringMapping<T>.Values.ContainsKey(rawValue))
            {
                var value = EnumFromStringMapping<T>.Values[rawValue];
                ParseLock.ExitReadLock();
                return value;
            }

            try
            {
                ParseLock.EnterWriteLock();

                if (string.IsNullOrEmpty(rawValue))
                    return EnumFromStringMapping<T>.Values[rawValue] = Option<T>.None;

                var enumValue = GetEnum<T>(rawValue).ToSome();
                EnumFromStringMapping<T>.Values[rawValue] = enumValue;
                return enumValue;
            }
            catch (ArgumentException)
            {
                return EnumFromStringMapping<T>.Values[rawValue] = Option<T>.None;
            }
            finally
            {
                ParseLock.ExitWriteLock();
            }
        }

        public static T GetEnum<T>(this string rawValue) where T : struct, IConvertible
        {
            return (T) Enum.Parse(typeof(T), rawValue);
        }

        // TODO: check performance
        public static IEnumerable<T> GetValues<T>() where T : struct, IConvertible
        {
            ValuesLock.EnterReadLock();
            if (EnumValues<T>.Values != null)
            {
                ValuesLock.ExitReadLock();
                return EnumValues<T>.Values;
            }

            try
            {
                ValuesLock.EnterWriteLock();
                EnumValues<T>.Values = Enum.GetValues(typeof(T)).Cast<T>().ToList();
                return EnumValues<T>.Values;
            }
            finally
            {
                ValuesLock.ExitWriteLock();
            }
        }

        public static IEnumerable<T> GetValuesInternal<T>() where T : struct, IConvertible
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static bool HasFlag<T>(this T e, T value) where T : struct, IConvertible
        {
            if (typeof(T).IsEnum)
            {
                return (EnumConverter.ToUInt32RuntimeCheck(e) & EnumConverter.ToUInt32RuntimeCheck(value)) == EnumConverter.ToUInt32RuntimeCheck(value);
            }

            throw new ArgumentException($"Expected enum, got {value.GetType()}");
        }

        private static class EnumFromStringMapping<T> where T : struct, IConvertible
        {
            public static readonly Dictionary<string, Option<T>> Values = new Dictionary<string, Option<T>>();
        }

        private static class EnumValues<T> where T : struct, IConvertible
        {
            public static List<T> Values;
        }
    }
}