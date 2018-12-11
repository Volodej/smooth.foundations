using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smooth.Algebraics;
using Smooth.Delegates;
using Smooth.Dispose;
using Smooth.Foundations.Algebraics;
using Smooth.Slinq;

namespace Utils.Collections
{
    public static class DictionaryExtensions
    {
        public static V GetOrDefault<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default(V);
        }

        public static V GetOr<K, V>(this Dictionary<K, V> dictionary, K key, V other)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : other;
        }


        public static V GetOr<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> other)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : other();
        }

        public static V GetOrAdd<K, V>(this Dictionary<K, V> dictionary, K key, V other)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            return dictionary[key] = other;
        }


        public static V GetOrAdd<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> other)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            value = other();
            dictionary[key] = value;
            return value;
        }

        public static V GetOrAdd<K, V, P>(this Dictionary<K, V> dictionary, K key, Func<P, V> other, P param)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            value = other(param);
            dictionary[key] = value;
            return value;
        }

        public static Option<V> TryGet<K, V>(this Disposable<Dictionary<K, V>> dictionary, K key)
        {
            V value;
            return dictionary.value.TryGetValue(key, out value) ? Option.Some(value) : Option.None(value);
        }

        public static Option<V> TryGet<K, V>(this Dictionary<K, V> dictionary, K key)
        {
            V value;
            return dictionary.TryGetValue(key, out value) ? Option.Some(value) : Option.None(value);
        }
        
        // TODO
        //public static Option<V> TryGet<K, V>(this SerializableDictionary<K, V> dictionary, K key)
        //{
        //    V value;
        //    return dictionary.TryGetValue(key, out value) ? Option.Some(value) : Option.None(value);
        //}

        public static Option<V> TryGet<K, V>(this Dictionary<K, V> dictionary, Option<K> key)
        {
            return key.Select((k, dic) =>
            {
                V value;
                return dic.TryGetValue(k, out value) ? Option.Some(value) : Option.None(value);
            }, dictionary).Flatten();
        } 

        public static Dictionary<K, V> LeftMergedWith<K, V>(this IDictionary<K, V> left, IDictionary<K, V> right)
        {
            var merged = new Dictionary<K, V>(left);
            right.Slinq()
                .Where((pair, m) => !m.ContainsKey(pair.Key), merged)
                .ForEach((pair, m) => m.Add(pair.Key, pair.Value), merged);
            return merged;
        }

        public static IDictionary<string, string> AsStringDictionary(this IDictionary dictionary)
        {
            var result = new Dictionary<string, string>();

            foreach (var key in dictionary.Keys)
            {
                result[key.ToString()] = dictionary[key].ToString();
            }

            return result;
        }

        public static string AsString<K, V>(this IDictionary<K, V> dictionary)
        {
            return dictionary.Aggregate(new StringBuilder(),
                (sb, kvp) => sb.Append($"{kvp.Key}={kvp.Value}{Environment.NewLine}"),
                sb => sb.ToString());
        }

        public static ValueOrError<V> GetValueOrError<K, V>(this IDictionary<K, V> dictionary, K key)
        {
            if (dictionary.ContainsKey(key))
                return ValueOrError.FromValue(dictionary[key]);
            return ValueOrError<V>.FromError($"Can't find value in dictionary with key \"{key}\"");
        }

        public static Dictionary<TKey, TValue> SetKeyAndValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
    }
}