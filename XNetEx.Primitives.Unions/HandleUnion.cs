﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace XNetEx.Unions;

/// <summary>
/// 表示指针或句柄类型的联合。
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public unsafe struct HandleUnion : IEquatable<HandleUnion>, ISerializable
{
    /// <summary>
    /// 表示 <see cref="HandleUnion"/> 实例的大小。
    /// </summary>
    public static readonly int Size = sizeof(void*);

    /// <summary>
    /// 表示已经初始化为零的 <see cref="HandleUnion"/>。
    /// </summary>
    public static readonly HandleUnion Zero;

    /// <summary>
    /// 表示 <see cref="HandleUnion.ToString()"/> 方法使用的格式。
    /// </summary>
    private static readonly string Format = "X" + (2 * Size);

    /// <summary>
    /// 表示当前 <see cref="HandleUnion"/> 的有符号指针或句柄。
    /// </summary>
    [FieldOffset(0)] public nint IntPtr;

    /// <summary>
    /// 表示当前 <see cref="HandleUnion"/> 的无符号指针或句柄。
    /// </summary>
    [CLSCompliant(false)]
    [FieldOffset(0)] public nuint UIntPtr;

    /// <summary>
    /// 表示当前 <see cref="HandleUnion"/> 的指向未指定类型的指针。
    /// </summary>
    [CLSCompliant(false)]
    [FieldOffset(0)] public void* Pointer;

    /// <summary>
    /// 表示当前 <see cref="HandleUnion"/> 的字节缓冲区。
    /// </summary>
    [CLSCompliant(false)]
    [FieldOffset(0)] public fixed byte Bytes[sizeof(uint)];

    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定的有符号指针或句柄。
    /// </summary>
    /// <param name="value">一个有符号指针或句柄。</param>
    public HandleUnion(nint value) : this() { this.IntPtr = value; }

    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定的无符号指针或句柄。
    /// </summary>
    /// <param name="value">一个无符号指针或句柄。</param>
    [CLSCompliant(false)]
    public HandleUnion(nuint value) : this() { this.UIntPtr = value; }

    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定的指向未指定类型的指针。
    /// </summary>
    /// <param name="value">一个指向未指定类型的指针。</param>
    [CLSCompliant(false)]
    public HandleUnion(void* value) : this() { this.Pointer = value; }

    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定字节数组表示的值，并指定数组的偏移量。
    /// </summary>
    /// <param name="bytes">包含值的字节数组。</param>
    /// <param name="offset">字节数组的偏移量。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="offset"/> 超出了 <paramref name="bytes"/> 中元素的范围。</exception>
    public HandleUnion(byte[] bytes, int offset = 0) : this() { this.LoadFrom(bytes, offset); }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定只读字节跨度表示的值。
    /// </summary>
    /// <param name="bytes">包含值的只读字节跨度。</param>
    public HandleUnion(ReadOnlySpan<byte> bytes) : this() { this.LoadFrom(bytes); }
#endif

    /// <summary>
    /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定序列化信息和上下文表示的值。
    /// </summary>
    /// <param name="info">包含序列化所需信息的对象。</param>
    /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidCastException">
    /// 名为 <c>Value</c> 的值无法转换为无符号指针或句柄。</exception>
    /// <exception cref="OverflowException">
    /// 在字大小为 4 字节的计算机上对值为 8 字节的指针或句柄进行反序列化。</exception>
    /// <exception cref="SerializationException">
    /// 在 <paramref name="info"/> 中未找到名为 <c>Value</c> 的值。</exception>
    private HandleUnion(SerializationInfo info, StreamingContext context) : this()
    {
        if (info is null) { throw new ArgumentNullException(nameof(info)); }
        this.UIntPtr = checked((nuint)info.GetUInt64("Value"));
    }

    /// <summary>
    /// 将十六进制数字的字符串表示形式转换为它的等效 <see cref="HandleUnion"/> 表示形式。
    /// </summary>
    /// <param name="text">包含要转换的十六进制数字的字符串。</param>
    /// <returns>与 <paramref name="text"/> 中十六进制数字等效的 <see cref="HandleUnion"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示一个大于本机无符号整数的最大值的十六进制数字。</exception>
    public static HandleUnion Parse(string text)
    {
        if (text is null) { throw new ArgumentNullException(nameof(text)); }
        return new HandleUnion(checked((nuint)Convert.ToUInt64(text, 16)));
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 在当前 <see cref="HandleUnion"/> 上创建字节跨度。
    /// </summary>
    /// <returns>当前 <see cref="HandleUnion"/> 的字节跨度表示形式。</returns>
    public Span<byte> AsByteSpan() => MemoryMarshal.CreateSpan(ref this.Bytes[0], Size);
#endif

    /// <summary>
    /// 将当前 <see cref="HandleUnion"/> 的值复制到指定字节数组中，并指定数组的偏移量。
    /// </summary>
    /// <param name="bytes">作为复制目标的字节数组。</param>
    /// <param name="offset">字节数组的偏移量。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="offset"/> 超出了 <paramref name="bytes"/> 中元素的范围。</exception>
    public void CopyTo(byte[] bytes, int offset = 0)
    {
        if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
        fixed (byte* pBytes = &bytes[offset]) { *(nuint*)pBytes = this.UIntPtr; }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将当前 <see cref="HandleUnion"/> 的值复制到指定字节跨度中。
    /// </summary>
    /// <param name="bytes">作为复制目标的字节跨度。</param>
#if NET8_0_OR_GREATER
    public void CopyTo(Span<byte> bytes) => MemoryMarshal.Write(bytes, in this.UIntPtr);
#else
    public void CopyTo(Span<byte> bytes) => MemoryMarshal.Write(bytes, ref this.UIntPtr);
#endif
#endif

    /// <summary>
    /// 确定当前 <see cref="HandleUnion"/> 与指定的 <see cref="HandleUnion"/> 是否相等。
    /// </summary>
    /// <param name="other">要与当前实例进行比较的 <see cref="HandleUnion"/>。</param>
    /// <returns>若当前 <see cref="HandleUnion"/> 的值与 <paramref name="other"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public bool Equals(HandleUnion other) => this.UIntPtr == other.UIntPtr;

    /// <summary>
    /// 确定当前 <see cref="HandleUnion"/> 与指定的对象是否相等。
    /// </summary>
    /// <param name="obj">要与当前实例进行比较的对象。</param>
    /// <returns>若 <paramref name="obj"/> 为 <see cref="HandleUnion"/>，
    /// 且当前 <see cref="HandleUnion"/> 的值与 <paramref name="obj"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(object? obj) => (obj is HandleUnion other) && this.Equals(other);

    /// <summary>
    /// 获取当前 <see cref="HandleUnion"/> 的哈希代码。
    /// </summary>
    /// <returns>当前 <see cref="HandleUnion"/> 哈希代码。</returns>
    public override int GetHashCode() => this.UIntPtr.GetHashCode();

    /// <summary>
    /// 将指定字节数组的值复制到当前 <see cref="HandleUnion"/> 中，并指定数组偏移量。
    /// </summary>
    /// <param name="bytes">作为复制源的字节数组。</param>
    /// <param name="offset">字节数组的偏移量。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="offset"/> 超出了 <paramref name="bytes"/> 中元素的范围。</exception>
    public void LoadFrom(byte[] bytes, int offset = 0)
    {
        if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
        fixed (byte* pBytes = &bytes[offset]) { this.UIntPtr = *(nuint*)pBytes; }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将指定只读字节跨度的值复制到当前 <see cref="HandleUnion"/> 中。
    /// </summary>
    /// <param name="bytes">作为复制源的只读字节跨度。</param>
    public void LoadFrom(ReadOnlySpan<byte> bytes) => this.UIntPtr = MemoryMarshal.Read<nuint>(bytes);
#endif

    /// <summary>
    /// 将当前 <see cref="HandleUnion"/> 的值转换为字节数组。
    /// </summary>
    /// <returns>包含当前 <see cref="HandleUnion"/> 的值的字节数组。</returns>
    public byte[] ToByteArray() { byte[] bytes = new byte[Size]; this.CopyTo(bytes); return bytes; }

    /// <summary>
    /// 将当前 <see cref="HandleUnion"/> 转换为其等效的十六进制数字的字符串表达形式。
    /// </summary>
    /// <returns>表示当前 <see cref="HandleUnion"/> 的十六进制数字的字符串。</returns>
    public override string ToString() => "0x" + ((ulong)this.UIntPtr).ToString(Format);

    /// <summary>
    /// 获取序列化当前 <see cref="HandleUnion"/> 所需的数据。
    /// </summary>
    /// <param name="info">包含序列化所需信息的对象。</param>
    /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) { throw new ArgumentNullException(nameof(info)); }
        info.AddValue("Value", (ulong)this.UIntPtr);
    }

    /// <summary>
    /// 确定两个指定的 <see cref="HandleUnion"/> 是否相等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="HandleUnion"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="HandleUnion"/>。</param>
    /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator ==(HandleUnion left, HandleUnion right) => left.UIntPtr == right.UIntPtr;

    /// <summary>
    /// 确定两个指定的 <see cref="HandleUnion"/> 是否不等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="HandleUnion"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="HandleUnion"/>。</param>
    /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值不等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator !=(HandleUnion left, HandleUnion right) => left.UIntPtr != right.UIntPtr;

    /// <summary>
    /// 将指定的有符号指针或句柄隐式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="value">要转换的有符号指针或句柄。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    public static implicit operator HandleUnion(nint value) => new HandleUnion(value);

    /// <summary>
    /// 将指定的无符号指针或句柄隐式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="value">要转换的无符号指针或句柄。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    [CLSCompliant(false)]
    public static implicit operator HandleUnion(nuint value) => new HandleUnion(value);

    /// <summary>
    /// 将指定的指向未指定类型的指针隐式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="value">要转换的指向未指定类型的指针。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    [CLSCompliant(false)]
    public static implicit operator HandleUnion(void* value) => new HandleUnion(value);

    /// <summary>
    /// 将指定的 <see cref="HandleUnion"/> 隐式转换为有符号指针或句柄。
    /// </summary>
    /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
    /// <returns>转换得到的有符号指针或句柄。</returns>
    public static implicit operator nint(HandleUnion union) => union.IntPtr;

    /// <summary>
    /// 将指定的 <see cref="HandleUnion"/> 隐式转换为无符号指针或句柄。
    /// </summary>
    /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
    /// <returns>转换得到的无符号指针或句柄。</returns>
    [CLSCompliant(false)]
    public static implicit operator nuint(HandleUnion union) => union.UIntPtr;

    /// <summary>
    /// 将指定的 <see cref="HandleUnion"/> 隐式转换为指向未指定类型的指针。
    /// </summary>
    /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
    /// <returns>转换得到的指向未指定类型的指针。</returns>
    [CLSCompliant(false)]
    public static implicit operator void*(HandleUnion union) => union.Pointer;

    /// <summary>
    /// 将指定的 <see cref="ByteUnion"/> 显式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="ByteUnion"/>。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    public static explicit operator HandleUnion(ByteUnion union) => new HandleUnion((nuint)union.Byte);

    /// <summary>
    /// 将指定的 <see cref="WordUnion"/> 显式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    public static explicit operator HandleUnion(WordUnion union) => new HandleUnion((nuint)union.UInt16);

    /// <summary>
    /// 将指定的 <see cref="DWordUnion"/> 显式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    public static explicit operator HandleUnion(DWordUnion union) => new HandleUnion((nuint)union.UInt32);

    /// <summary>
    /// 将指定的 <see cref="QWordUnion"/> 显式转换为 <see cref="HandleUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
    /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
    public static explicit operator HandleUnion(QWordUnion union) => new HandleUnion((nuint)union.UInt64);
}
