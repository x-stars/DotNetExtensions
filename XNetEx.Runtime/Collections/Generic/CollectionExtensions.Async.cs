#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections.Generic;

namespace XNetEx.Collections.Generic;

static partial class CollectionExtensions
{
    /// <summary>
    /// 返回 <see cref="IEnumerable{T}"/> 的 <see cref="IAsyncEnumerable{T}"/> 包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    /// <param name="enumerable">要包装的 <see cref="IEnumerable{T}"/> 对象。</param>
    /// <returns><paramref name="enumerable"/> 的 <see cref="IAsyncEnumerable{T}"/> 包装。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
    public static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> enumerable)
    {
        return new AsyncedEnumerable<T>(enumerable);
    }

    /// <summary>
    /// 返回 <see cref="IAsyncEnumerable{T}"/> 的 <see cref="IEnumerable{T}"/> 包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    /// <param name="asyncEnumerable">要包装的 <see cref="IAsyncEnumerable{T}"/> 对象。</param>
    /// <returns><paramref name="asyncEnumerable"/> 的 <see cref="IEnumerable{T}"/> 包装。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="asyncEnumerable"/> 为 <see langword="null"/>。</exception>
    public static IEnumerable<T> AsEnumerable<T>(this IAsyncEnumerable<T> asyncEnumerable)
    {
        return new SyncedAsyncEnumerable<T>(asyncEnumerable);
    }
}
#endif
