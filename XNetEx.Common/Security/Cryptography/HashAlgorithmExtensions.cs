using System;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace XNetEx.Security.Cryptography;

/// <summary>
/// 提供 <see cref="HashAlgorithm"/> 的扩展方法。
/// </summary>
public static class HashAlgorithmExtensions
{
    private abstract class MethodBridge : HashAlgorithm
    {
        public void AppendData(byte[] buffer, int offset, int count)
        {
            this.ValidateInput(buffer, offset, count);
            this.State = 1;
            this.HashCore(buffer, offset, count);
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public void AppendData(ReadOnlySpan<byte> source)
        {
            this.State = 1;
            this.HashCore(source);
        }
#endif

        public byte[] GetFinalHash()
        {
            var hash = this.HashFinal();
            this.HashValue = hash;
            this.State = 0;
            return (byte[])hash.Clone();
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public bool TryGetFinalHash(Span<byte> destination, out int bytesWritten)
        {
            var result = this.TryHashFinal(destination, out bytesWritten);
            this.HashValue = null;
            this.State = 0;
            return result;
        }
#endif

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        private void ValidateInput(byte[] buffer, int offset, int count) =>
            Validator.Instance.TransformBlock(buffer, offset, count, null, 0);

        private sealed class Validator : MethodBridge
        {
            internal static readonly Validator Instance = new Validator();

            public override void Initialize() => throw new NotImplementedException();

            protected override void HashCore(byte[] array, int ibStart, int cbSize) { }

            protected override byte[] HashFinal() => throw new NotImplementedException();
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe MethodBridge AsBridge(this HashAlgorithm hashing)
    {
#if NETCOREAPP3_0_OR_GREATER
        return Unsafe.As<HashAlgorithm, MethodBridge>(ref hashing);
#else
        static object Identity(object value) => value;
        return ((delegate*<HashAlgorithm, MethodBridge>)
                (delegate*<object, object>)&Identity)(hashing);
#endif
    }

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
    public static void AppendData(this HashAlgorithm hashing, byte[] buffer)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(buffer, 0, buffer?.Length ?? 0);
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
    public static void AppendData(
        this HashAlgorithm hashing, byte[] buffer, int offset, int count)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(buffer, offset, count);
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 计算输入只读字节范围的哈希值。
    /// </summary>
    /// <param name="hashing">当前 <see cref="HashAlgorithm"/> 对象。</param>
    /// <param name="source">要计算其哈希代码的输入。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> 对象已释放。</exception>
    public static void AppendData(this HashAlgorithm hashing, ReadOnlySpan<byte> source)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        hashing.AsBridge().AppendData(source);
    }
#endif

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

        return hashing.AsBridge().GetFinalHash();
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 完成哈希计算并尝试获取计算所得的哈希代码的值。
    /// </summary>
    /// <param name="hashing">当前 <see cref="HashAlgorithm"/> 对象。</param>
    /// <param name="destination">用于接收哈希值的字节范围。</param>
    /// <param name="bytesWritten">
    /// 此方法返回时，为写入 <paramref name="destination"/> 的字节总数。</param>
    /// <returns>如果 <paramref name="destination"/> 的长度足以接收哈希值，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="hashing"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ObjectDisposedException">
    /// <paramref name="hashing"/> 对象已释放。</exception>
    public static bool TryGetFinalHash(
        this HashAlgorithm hashing, Span<byte> destination, out int bytesWritten)
    {
        if (hashing is null)
        {
            throw new ArgumentNullException(nameof(hashing));
        }

        return hashing.AsBridge().TryGetFinalHash(destination, out bytesWritten);
    }
#endif
}
