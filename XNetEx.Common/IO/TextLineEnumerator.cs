using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace XNetEx.IO;

/// <summary>
/// 为 <see cref="TextReader"/> 提供按行迭代的枚举器。
/// </summary>
internal sealed class TextLineEnumerator : IEnumerable<string>, IEnumerator<string>
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    , IAsyncEnumerable<string>, IAsyncEnumerator<string>
#endif
{
    /// <summary>
    /// 表示要按行进行迭代的文本读取器。
    /// </summary>
    private readonly TextReader Reader;

    /// <summary>
    /// 表示是否在迭代完成后释放输入的文本读取器。
    /// </summary>
    private readonly bool Disposing;

    /// <summary>
    /// 表示文本读取器在当前位置读取到的文本行。
    /// </summary>
    private volatile string? CurrentLine;

    /// <summary>
    /// 以指定的文本读取器初始化 <see cref="TextLineEnumerator"/> 类的新实例，
    /// 并指定是否在迭代完成后释放输入的文本读取器。
    /// </summary>
    /// <param name="reader">要按行进行迭代的文本读取器。</param>
    /// <param name="disposing">指定是否在迭代完成后释放输入的文本读取器。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
    internal TextLineEnumerator(TextReader reader, bool disposing = false)
    {
        this.Reader = reader ??
            throw new ArgumentNullException(nameof(reader));
        this.Disposing = disposing;
    }

    /// <summary>
    /// 获取文本读取器在当前位置读取到的文本行。
    /// </summary>
    /// <returns>文本读取器在当前位置读取到的文本行。</returns>
    public string Current => this.CurrentLine!;

    /// <summary>
    /// 获取文本读取器在当前位置读取到的文本行。
    /// </summary>
    /// <returns>文本读取器在当前位置读取到的文本行。</returns>
    object IEnumerator.Current => this.CurrentLine!;

    /// <summary>
    /// 根据初始化参数的指示释放输入的文本读取器。
    /// </summary>
    public void Dispose() { if (this.Disposing) { this.Reader.Dispose(); } }

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 根据初始化参数的指示同步释放输入的文本读取器。
    /// </summary>
    public ValueTask DisposeAsync()
    {
        try { this.Dispose(); return new ValueTask(); }
        catch (Exception e) { return new ValueTask(Task.FromException(e)); }
    }
#endif

    /// <summary>
    /// 返回当前按行迭代读取文本的枚举器。
    /// </summary>
    /// <returns>当前按行迭代读取文本的枚举器。</returns>
    public IEnumerator<string> GetEnumerator() => this;

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 返回当前按行异步迭代读取文本的枚举器。
    /// </summary>
    /// <param name="cancellationToken">不使用此参数。</param>
    /// <returns>当前按行异步迭代读取文本的枚举器。</returns>
    public IAsyncEnumerator<string> GetAsyncEnumerator(
        CancellationToken cancellationToken = default) => this;
#endif

    /// <summary>
    /// 将文本读取器的位置移动到下一行。
    /// </summary>
    /// <returns>如果成功移动到下一行，则为 <see langword="true"/>；
    /// 如果已读取所有字符，则为 <see langword="false"/>。</returns>
    public bool MoveNext() => (this.CurrentLine = this.Reader.ReadLine()) != null;

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将文本读取器的位置异步移动到下一行。
    /// </summary>
    /// <returns>一个 <see cref="ValueTask{TResult}"/>，
    /// 如果成功移动到下一行，则结果为 <see langword="true"/>；
    /// 如果已读取所有字符，则结果为 <see langword="false"/>。</returns>
    public async ValueTask<bool> MoveNextAsync() =>
        (this.CurrentLine = await this.Reader.ReadLineAsync()) != null;
#endif

    /// <summary>
    /// 返回当前按行迭代读取文本的枚举器。
    /// </summary>
    /// <returns>当前按行迭代读取文本的枚举器。</returns>
    IEnumerator IEnumerable.GetEnumerator() => this;

    /// <summary>
    /// 不支持此方法，永远抛出 <see cref="NotSupportedException"/> 异常。
    /// </summary>
    /// <exception cref="NotSupportedException">不支持此方法。</exception>
    void IEnumerator.Reset() => throw new NotSupportedException();
}
