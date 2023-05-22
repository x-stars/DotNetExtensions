using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Concurrent;

/// <summary>
/// Provides extension methods for <see cref="ConcurrentDictionary{TKey, TValue}"/>.
/// </summary>
internal static class ConcurrentDictionary
{
    /// <summary>
    /// Updates the value associated with the specified key and returns the original value
    /// if the key already exists,
    /// or adds a key/value pair to the <see cref="ConcurrentDictionary{TKey, TValue}"/>
    /// if the key does not already exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
    /// <param name="key">The key whose value should be updated or to be added.</param>
    /// <param name="value">The value to be updated or added for the key.</param>
    /// <param name="oldValue">When this method returns <see langword="true"/>,
    /// contains the original value associated with <paramref name="key"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="key"/> already exists
    /// in <paramref name="dictionary"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
    /// or <paramref name="key"/> is <see langword="null"/>.</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="dictionary"/> contains too many elements.</exception>
    public static bool ExchangeOrAdd<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary,
        TKey key, TValue value, [MaybeNullWhen(false)] out TValue oldValue)
        where TKey : notnull
    {
        if (dictionary is null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        while (true)
        {
            var hasValue = dictionary.TryGetValue(key, out oldValue);
            var exchanged = hasValue ?
                dictionary.TryUpdate(key, value, oldValue!) :
                dictionary.TryAdd(key, value);
            if (exchanged) { return hasValue; }
        }
    }

    /// <summary>
    /// Attempts to update the value associated with the specified key in the
    /// <see cref="ConcurrentDictionary{TKey, TValue}"/> and returns the original value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The <see cref="ConcurrentDictionary{TKey, TValue}"/>.</param>
    /// <param name="key">The key whose value should be updated.</param>
    /// <param name="value">The value to be updated for the key.</param>
    /// <param name="oldValue">When this method returns <see langword="true"/>,
    /// contains the original value associated with <paramref name="key"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="key"/> already exists
    /// in <paramref name="dictionary"/>; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
    /// or <paramref name="key"/> is <see langword="null"/>.</exception>
    public static bool TryExchange<TKey, TValue>(
        this ConcurrentDictionary<TKey, TValue> dictionary,
        TKey key, TValue value, [MaybeNullWhen(false)] out TValue oldValue)
        where TKey : notnull
    {
        if (dictionary is null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

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
