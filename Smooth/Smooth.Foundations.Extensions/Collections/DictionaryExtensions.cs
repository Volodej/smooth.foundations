using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smooth.Algebraics;
using Smooth.Algebraics.Results;
using Smooth.Dispose;
using Smooth.Slinq;

namespace Smooth.Extensions.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : default(TValue);
        }

        public static TValue GetOr<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue other)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : other;
        }


        public static TValue GetOr<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> other)
        {
            return dictionary.ContainsKey(key) ? dictionary[key] : other();
        }

        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue other)
        {
            if (dictionary.TryGetValue(key, out var value)) return value;
            return dictionary[key] = other;
        }


        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> other)
        {
            if (dictionary.TryGetValue(key, out var value)) return value;
            value = other();
            dictionary[key] = value;
            return value;
        }

        public static TValue GetOrAdd<TKey, TValue, TP>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TP, TValue> other,
            TP param)
        {
            if (dictionary.TryGetValue(key, out var value)) return value;
            value = other(param);
            dictionary[key] = value;
            return value;
        }

        public static Option<TValue> TryGet<TKey, TValue>(this Disposable<Dictionary<TKey, TValue>> dictionary, TKey key)
        {
            return dictionary.value.TryGetValue(key, out var value) ? Option.Some(value) : Option.None;
        }

        public static Option<TValue> TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryGetValue(key, out var value) ? Option.Some(value) : Option.None;
        }

        public static Option<TValue> TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Option<TKey> key)
        {
            return key.Select((k, dic) => { return dic.TryGetValue(k, out var value) ? Option.Some(value) : Option.None; },
                dictionary).Flatten();
        }

        public static Dictionary<TKey, TValue> LeftMergedWith<TKey, TValue>(this IDictionary<TKey, TValue> left,
            IDictionary<TKey, TValue> right)
        {
            var merged = new Dictionary<TKey, TValue>(left);
            right.Slinq()
                .Where((pair, m) => !m.ContainsKey(pair.Key), merged)
                .ForEach((pair, m) => m.Add(pair.Key, pair.Value), merged);
            return merged;
        }

        public static IDictionary<string, string> AsStringDictionary(this IDictionary dictionary)
        {
            var result = new Dictionary<string, string>();

            foreach (var key in dictionary.Keys) result[key.ToString()] = dictionary[key].ToString();

            return result;
        }

        public static string AsString<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary.Aggregate(new StringBuilder(),
                (sb, kvp) => sb.Append($"{kvp.Key}={kvp.Value}{Environment.NewLine}"),
                sb => sb.ToString());
        }

        public static Result<TValue> GetValueOrError<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
                return Result.FromValue(dictionary[key]);
            return Result<TValue>.FromError($"Can't find value in dictionary with key \"{key}\"");
        }

        public static Dictionary<TKey, TValue> SetKeyAndValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict[key] = value;
            return dict;
        }
    }
}