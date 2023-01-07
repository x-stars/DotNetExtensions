using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace XNetEx.Unions;

/// <summary>
/// 表示 64 位数据类型的联合。
/// </summary>
[Serializable]
[StructLayout(LayoutKind.Explicit)]
public unsafe struct QWordUnion : IEquatable<QWordUnion>, ISerializable
{
    /// <summary>
    /// 表示 <see cref="QWordUnion"/> 实例的大小。
    /// </summary>
    public const int Size = 8;

    /// <summary>
    /// 表示已经初始化为零的 <see cref="QWordUnion"/>。
    /// </summary>
    public static readonly QWordUnion Zero;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 的 64 位有符号整数。
    /// </summary>
    [FieldOffset(0)] public long Int64;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 的 64 位无符号整数。
    /// </summary>
    [CLSCompliant(false)]
    [FieldOffset(0)] public ulong UInt64;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 的双精度浮点数。
    /// </summary>
    [FieldOffset(0)] public double Double;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 低 32 位的 <see cref="DWordUnion"/>。
    /// </summary>
    [FieldOffset(0)] public DWordUnion LowDWord;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 高 32 位的 <see cref="DWordUnion"/>。
    /// </summary>
    [FieldOffset(4)] public DWordUnion HighDWord;

    /// <summary>
    /// 表示当前 <see cref="QWordUnion"/> 的字节缓冲区。
    /// </summary>
    [CLSCompliant(false)]
    [FieldOffset(0)] public fixed byte Bytes[8];

    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定的 64 位有符号整数。
    /// </summary>
    /// <param name="value">作为值的 64 位有符号整数。</param>
    public QWordUnion(long value) : this() { this.Int64 = value; }

    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定的 64 位无符号整数。
    /// </summary>
    /// <param name="value">作为值的 64 位无符号整数。</param>
    [CLSCompliant(false)]
    public QWordUnion(ulong value) : this() { this.UInt64 = value; }

    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定的双精度浮点数。
    /// </summary>
    /// <param name="value">作为值的双精度浮点数。</param>
    public QWordUnion(double value) : this() { this.Double = value; }

    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定字节数组表示的值，并指定数组的偏移量。
    /// </summary>
    /// <param name="bytes">包含值的字节数组。</param>
    /// <param name="offset">字节数组的偏移量。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="offset"/> 超出了 <paramref name="bytes"/> 中元素的范围。</exception>
    public QWordUnion(byte[] bytes, int offset = 0) : this() { this.LoadFrom(bytes, offset); }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定只读字节跨度表示的值。
    /// </summary>
    /// <param name="bytes">包含值的只读字节跨度。</param>
    public QWordUnion(ReadOnlySpan<byte> bytes) : this() { this.LoadFrom(bytes); }
#endif

    /// <summary>
    /// 将 <see cref="QWordUnion"/> 结构的新实例初始化为指定序列化信息和上下文表示的值。
    /// </summary>
    /// <param name="info">包含序列化所需信息的对象。</param>
    /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidCastException">
    /// 名为 <c>Value</c> 的值无法转换为 64 位无符号整数。</exception>
    /// <exception cref="SerializationException">
    /// 在 <paramref name="info"/> 中未找到名为 <c>Value</c> 的值。</exception>
    private QWordUnion(SerializationInfo info, StreamingContext context) : this()
    {
        if (info is null) { throw new ArgumentNullException(nameof(info)); }
        this.UInt64 = info.GetUInt64("Value");
    }

    /// <summary>
    /// 将十六进制数字的字符串表示形式转换为它的等效 <see cref="QWordUnion"/> 表示形式。
    /// </summary>
    /// <param name="text">包含要转换的十六进制数字的字符串。</param>
    /// <returns>与 <paramref name="text"/> 中十六进制数字等效的 <see cref="QWordUnion"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示一个大于 <see cref="ulong.MaxValue"/> 的十六进制数字。</exception>
    public static QWordUnion Parse(string text)
    {
        if (text is null) { throw new ArgumentNullException(nameof(text)); }
        return new QWordUnion(Convert.ToUInt64(text, 16));
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 在当前 <see cref="QWordUnion"/> 上创建字节跨度。
    /// </summary>
    /// <returns>当前 <see cref="QWordUnion"/> 的字节跨度表示形式。</returns>
    public Span<byte> AsByteSpan() => MemoryMarshal.CreateSpan(ref this.Bytes[0], 8);
#endif

    /// <summary>
    /// 将当前 <see cref="QWordUnion"/> 的值复制到指定字节数组中，并指定数组的偏移量。
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
        fixed (byte* pBytes = &bytes[offset]) { *(ulong*)pBytes = this.UInt64; }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将当前 <see cref="QWordUnion"/> 的值复制到指定字节跨度中。
    /// </summary>
    /// <param name="bytes">作为复制目标的字节跨度。</param>
    public void CopyTo(Span<byte> bytes) => MemoryMarshal.Write(bytes, ref this.UInt64);
#endif

    /// <summary>
    /// 确定当前 <see cref="QWordUnion"/> 与指定的 <see cref="QWordUnion"/> 是否相等。
    /// </summary>
    /// <param name="other">要与当前实例进行比较的 <see cref="QWordUnion"/>。</param>
    /// <returns>若当前 <see cref="QWordUnion"/> 的值与 <paramref name="other"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public bool Equals(QWordUnion other) => this.UInt64 == other.UInt64;

    /// <summary>
    /// 确定当前 <see cref="QWordUnion"/> 与指定的对象是否相等。
    /// </summary>
    /// <param name="obj">要与当前实例进行比较的对象。</param>
    /// <returns>若 <paramref name="obj"/> 为 <see cref="QWordUnion"/>，
    /// 且当前 <see cref="QWordUnion"/> 的值与 <paramref name="obj"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(object? obj) => (obj is QWordUnion other) && this.Equals(other);

    /// <summary>
    /// 获取当前 <see cref="QWordUnion"/> 的哈希代码。
    /// </summary>
    /// <returns>当前 <see cref="QWordUnion"/> 哈希代码。</returns>
    public override int GetHashCode() => this.UInt64.GetHashCode();

    /// <summary>
    /// 将指定字节数组的值复制到当前 <see cref="QWordUnion"/> 中，并指定数组偏移量。
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
        fixed (byte* pBytes = &bytes[offset]) { this.UInt64 = *(ulong*)pBytes; }
    }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// 将指定只读字节跨度的值复制到当前 <see cref="QWordUnion"/> 中。
    /// </summary>
    /// <param name="bytes">作为复制源的只读字节跨度。</param>
    public void LoadFrom(ReadOnlySpan<byte> bytes) => this.UInt64 = MemoryMarshal.Read<ulong>(bytes);
#endif

    /// <summary>
    /// 将当前 <see cref="QWordUnion"/> 的值转换为字节数组。
    /// </summary>
    /// <returns>包含当前 <see cref="QWordUnion"/> 的值的字节数组。</returns>
    public byte[] ToByteArray() { byte[] bytes = new byte[8]; this.CopyTo(bytes); return bytes; }

    /// <summary>
    /// 将当前 <see cref="QWordUnion"/> 转换为其等效的十六进制数字的字符串表示形式。
    /// </summary>
    /// <returns>表示当前 <see cref="QWordUnion"/> 对象的十六进制数字的字符串。</returns>
    public override string ToString() => "0x" + this.UInt64.ToString("X16");

    /// <summary>
    /// 获取序列化当前 <see cref="QWordUnion"/> 所需的数据。
    /// </summary>
    /// <param name="info">包含序列化所需信息的对象。</param>
    /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        if (info is null) { throw new ArgumentNullException(nameof(info)); }
        info.AddValue("Value", this.UInt64);
    }

    /// <summary>
    /// 确定两个指定的 <see cref="QWordUnion"/> 是否相等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="QWordUnion"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="QWordUnion"/>。</param>
    /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator ==(QWordUnion left, QWordUnion right) => left.UInt64 == right.UInt64;

    /// <summary>
    /// 确定两个指定的 <see cref="QWordUnion"/> 是否不等。
    /// </summary>
    /// <param name="left">要比较的第一个 <see cref="QWordUnion"/>。</param>
    /// <param name="right">要比较的第二个 <see cref="QWordUnion"/>。</param>
    /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值不等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator !=(QWordUnion left, QWordUnion right) => left.UInt64 != right.UInt64;

    /// <summary>
    /// 将指定的 64 位有符号整数隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="value">要转换的 64 位有符号整数。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static implicit operator QWordUnion(long value) => new QWordUnion(value);

    /// <summary>
    /// 将指定的 64 位无符号整数隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="value">要转换的 64 位无符号整数。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    [CLSCompliant(false)]
    public static implicit operator QWordUnion(ulong value) => new QWordUnion(value);

    /// <summary>
    /// 将指定的双精度浮点数隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="value">要转换的双精度浮点数。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static implicit operator QWordUnion(double value) => new QWordUnion(value);

    /// <summary>
    /// 将指定的 <see cref="QWordUnion"/> 隐式转换为 64 位有符号整数。
    /// </summary>
    /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
    /// <returns>转换得到的 64 位有符号整数。</returns>
    public static implicit operator long(QWordUnion union) => union.Int64;

    /// <summary>
    /// 将指定的 <see cref="QWordUnion"/> 隐式转换为 64 位无符号整数。
    /// </summary>
    /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
    /// <returns>转换得到的 64 位无符号整数。</returns>
    [CLSCompliant(false)]
    public static implicit operator ulong(QWordUnion union) => union.UInt64;

    /// <summary>
    /// 将指定的 <see cref="QWordUnion"/> 隐式转换为双精度浮点数。
    /// </summary>
    /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
    /// <returns>转换得到的双精度浮点数。</returns>
    public static implicit operator double(QWordUnion union) => union.Double;

    /// <summary>
    /// 将指定的 <see cref="ByteUnion"/> 隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="ByteUnion"/>。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static implicit operator QWordUnion(ByteUnion union) => new QWordUnion((ulong)union.Byte);

    /// <summary>
    /// 将指定的 <see cref="WordUnion"/> 隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static implicit operator QWordUnion(WordUnion union) => new QWordUnion((ulong)union.UInt16);

    /// <summary>
    /// 将指定的 <see cref="DWordUnion"/> 隐式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static implicit operator QWordUnion(DWordUnion union) => new QWordUnion((ulong)union.UInt32);

    /// <summary>
    /// 将指定的 <see cref="HandleUnion"/> 显式转换为 <see cref="QWordUnion"/>。
    /// </summary>
    /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
    /// <returns>转换得到的 <see cref="QWordUnion"/>。</returns>
    public static explicit operator QWordUnion(HandleUnion union) => new QWordUnion((ulong)union.UIntPtr);
}
