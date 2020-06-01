using System;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.Domain.Internal
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> createFunc)
            where TValue : new()
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = createFunc();
                dict.Add(key, val);
            }

            return val;
        }
    }
}
