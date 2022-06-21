#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XNetEx.Collections.Generic;

/// <summary>
/// 提供 <see cref="IEnumerable{T}"/> 的 <see cref="IAsyncEnumerable{T}"/> 包装。
/// </summary>
/// <typeparam name="T">要枚举的对象的类型。</typeparam>
internal sealed class AsyncedEnumerable<T> : IAsyncEnumerable<T>
{
    /// <summary>
    /// 表示当前包装的 <see cref="IEnumerable{T}"/> 对象。
    /// </summary>
    private readonly IEnumerable<T> Enumerable;

    /// <summary>
    /// 以要包装为 <see cref="IAsyncEnumerable{T}"/> 的 <see cref="IEnumerable{T}"/>
    /// 初始化 <see cref="AsyncedEnumerable{T}"/> 类的新实例。
    /// </summary>
    /// <param name="enumerable">要包装的 <see cref="IEnumerable{T}"/> 对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
    public AsyncedEnumerable(IEnumerable<T> enumerable)
    {
        this.Enumerable = enumerable ??
            throw new ArgumentNullException(nameof(enumerable));
    }

    /// <inheritdoc/>
    public IAsyncEnumerator<T> GetAsyncEnumerator(
        CancellationToken cancellationToken = default)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            throw new TaskCanceledException();
        }

        var enumerator = this.Enumerable.GetEnumerator();
        return new AsyncedEnumerator<T>(enumerator, cancellationToken);
    }
}

/// <summary>
/// 提供 <see cref="IEnumerator{T}"/> 的 <see cref="IAsyncEnumerator{T}"/> 包装。
/// </summary>
/// <typeparam name="T">要枚举的对象的类型。</typeparam>
internal sealed class AsyncedEnumerator<T> : IAsyncEnumerator<T>
{
    /// <summary>
    /// 表示当前包装的 <see cref="IEnumerator{T}"/> 对象。
    /// </summary>
    private readonly IEnumerator<T> Enumerator;

    /// <summary>
    /// 表示 <see cref="IEnumerator.MoveNext()"/> 方法的委托。
    /// </summary>
    private readonly Func<bool> MoveNextDelegate;

    /// <summary>
    /// 表示用于取消异步迭代的 <see cref="CancellationToken"/>。
    /// </summary>
    private readonly CancellationToken CancellationSource;

    /// <summary>
    /// 以要包装为 <see cref="IAsyncEnumerator{T}"/> 的 <see cref="IEnumerator{T}"/>
    /// 初始化 <see cref="AsyncedEnumerator{T}"/> 类的新实例。
    /// </summary>
    /// <param name="enumerator">要包装的 <see cref="IEnumerator{T}"/> 对象。</param>
    /// <param name="cancellationToken">
    /// 一个 <see cref="CancellationToken"/>，可用于取消异步迭代。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
    public AsyncedEnumerator(IEnumerator<T> enumerator,
        CancellationToken cancellationToken = default)
    {
        this.Enumerator = enumerator ??
            throw new ArgumentNullException(nameof(enumerator));
        this.MoveNextDelegate = this.Enumerator.MoveNext;
        this.CancellationSource = cancellationToken;
    }

    /// <inheritdoc/>
    public T Current => this.Enumerator.Current;

    /// <inheritdoc/>
    public ValueTask DisposeAsync()
    {
        try { this.Enumerator.Dispose(); return new ValueTask(); }
        catch (Exception e) { return new ValueTask(Task.FromException(e)); }
    }

    /// <inheritdoc/>
    public ValueTask<bool> MoveNextAsync()
    {
        if (this.CancellationSource.IsCancellationRequested)
        {
            return new ValueTask<bool>(
                Task.FromCanceled<bool>(this.CancellationSource));
        }

        return new ValueTask<bool>(
            Task.Run(this.MoveNextDelegate, this.CancellationSource));
    }
}
#endif
