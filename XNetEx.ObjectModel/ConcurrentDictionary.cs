using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

internal static class ConcurrentDictionary
{
    public static bool AddOrExchange<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary,
        TKey key, TValue value, [MaybeNullWhen(false)] out TValue oldValue)
        where TKey : notnull
    {
        while (true)
        {
            var hasValue = dictionary.TryGetValue(key, out oldValue);
            var exchanged = hasValue ?
                dictionary.TryUpdate(key, value, oldValue!) :
                dictionary.TryAdd(key, value);
            if (exchanged) { return hasValue; }
        }
    }

    public static TValue Exchange<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TKey : notnull
    {
        while (true)
        {
            var oldValue = dictionary[key];
            if (dictionary.TryUpdate(key, value, oldValue))
            {
                return oldValue;
            }
        }
    }

    public static bool TryExchange<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary,
        TKey key, TValue value, [MaybeNullWhen(false)] out TValue oldValue)
        where TKey : notnull
    {
        while (true)
        {
            if (!dictionary.TryGetValue(key, out oldValue))
            {
                return false;
            }
            if (dictionary.TryUpdate(key, value, oldValue))
            {
                return true;
            }
        }
    }
}
