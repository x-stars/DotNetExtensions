using System;
using System.Security.Cryptography;

namespace XNetEx.Security.Cryptography;

/// <summary>
/// 提供 <see cref="HashAlgorithm"/> 的扩展方法。
/// </summary>
public static class HashAlgorithmExtensions
{
    /// <summary>
    /// 计算输入字节数组的哈希值。
    /// </summary>
    /// <param name="hashing">当前 <see cref="HashAlgorithm"/> 对象。</param>
    /// <param name="buffer">要计算其哈希代码的输入。</param>
    /// <exception cref="ArgumentException">
    /// <paramref name="buffer"/> 有一个无效的长度。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="hashing"/>
    /// 或 <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> 对象已释放。</exception>
    public static void AppendInput(this HashAlgorithm hashing, byte[] buffer)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.TransformBlock(buffer, 0, buffer.Length, null, 0);
    }

    /// <summary>
    /// 计算输入字节数组指定区域的哈希值。
    /// </summary>
    /// <param name="hashing">当前 <see cref="HashAlgorithm"/> 对象。</param>
    /// <param name="buffer">要计算其哈希代码的输入。</param>
    /// <param name="offset">输入字节数组中的偏移量，从该位置开始使用数据。</param>
    /// <param name="count">输入字节数组中用作数据的字节数。</param>
    /// <exception cref="ArgumentException"><paramref name="count"/>
    /// 使用了无效值；或 <paramref name="buffer"/> 有一个无效的长度。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="hashing"/>
    /// 或 <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="offset"/> 超出范围。此参数需要非负数。</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> 对象已释放。</exception>
    public static void AppendInput(
        this HashAlgorithm hashing, byte[] buffer, int offset, int count)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.TransformBlock(buffer, offset, count, null, 0);
    }

    /// <summary>
    /// 完成哈希计算并获取计算所得的哈希代码的值。
    /// </summary>
    /// <param name="hashing">当前 <see cref="HashAlgorithm"/> 对象。</param>
    /// <returns>计算所得的哈希代码的当前值。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> 对象已释放。</exception>
    public static byte[] GetFinalHash(this HashAlgorithm hashing)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.TransformFinalBlock(Array.Empty<byte>(), 0, 0);
        return hashing.Hash!;
    }
}
