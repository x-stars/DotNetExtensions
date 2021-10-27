using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace XstarS.Unions
{
    /// <summary>
    /// 表示 32 位数据类型的联合。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public unsafe struct DWordUnion : IEquatable<DWordUnion>, ISerializable
    {
        /// <summary>
        /// 表示 <see cref="DWordUnion"/> 实例的大小。
        /// </summary>
        public const int Size = 4;

        /// <summary>
        /// 表示已经初始化为零的 <see cref="DWordUnion"/>。
        /// </summary>
        public static readonly DWordUnion Zero;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 的 32 位有符号整数。
        /// </summary>
        [FieldOffset(0)] public int Int32;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 的 32 位无符号整数。
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)] public uint UInt32;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 的单精度浮点数。
        /// </summary>
        [FieldOffset(0)] public float Single;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 低 16 位的 <see cref="WordUnion"/>。
        /// </summary>
        [FieldOffset(0)] public WordUnion LowWord;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 高 16 位的 <see cref="WordUnion"/>。
        /// </summary>
        [FieldOffset(2)] public WordUnion HighWord;

        /// <summary>
        /// 表示当前 <see cref="DWordUnion"/> 的字节缓冲区。
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)] public fixed byte Bytes[4];

        /// <summary>
        /// 将 <see cref="DWordUnion"/> 结构的新实例初始化为指定的 32 位有符号整数。
        /// </summary>
        /// <param name="value">作为值的 32 位有符号整数。</param>
        public DWordUnion(int value) : this() { this.Int32 = value; }

        /// <summary>
        /// 将 <see cref="DWordUnion"/> 结构的新实例初始化为指定的 32 位无符号整数。
        /// </summary>
        /// <param name="value">作为值的 32 位无符号整数。</param>
        [CLSCompliant(false)]
        public DWordUnion(uint value) : this() { this.UInt32 = value; }

        /// <summary>
        /// 将 <see cref="DWordUnion"/> 结构的新实例初始化为指定的单精度浮点数。
        /// </summary>
        /// <param name="value">作为值的单精度浮点数。</param>
        public DWordUnion(float value) : this() { this.Single = value; }

        /// <summary>
        /// 将 <see cref="DWordUnion"/> 结构的新实例初始化为指定字节数组表示的值，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">包含值的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public DWordUnion(byte[] bytes, int offset = 0) : this() { this.LoadFrom(bytes, offset); }

        /// <summary>
        /// 将 <see cref="DWordUnion"/> 结构的新实例初始化为指定序列化信息和上下文表示的值。
        /// </summary>
        /// <param name="info">包含序列化所需信息的对象。</param>
        /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="InvalidCastException">
        /// 名为 <c>Value</c> 的值无法转换为 32 位无符号整数。</exception>
        /// <exception cref="SerializationException">
        /// 在 <paramref name="info"/> 中未找到名为 <c>Value</c> 的值。</exception>
        private DWordUnion(SerializationInfo info, StreamingContext context) : this()
        {
            if (info is null) { throw new ArgumentNullException(nameof(info)); }
            this.UInt32 = info.GetUInt32("Value");
        }

        /// <summary>
        /// 将十六进制数字的字符串表示形式转换为它的等效 <see cref="DWordUnion"/> 表示形式。
        /// </summary>
        /// <param name="text">包含要转换的十六进制数字的字符串。</param>
        /// <returns>与 <paramref name="text"/> 中十六进制数字等效的 <see cref="DWordUnion"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示一个大于 <see cref="uint.MaxValue"/> 的十六进制数字。</exception>
        public static DWordUnion Parse(string text)
        {
            if (text is null) { throw new ArgumentNullException(nameof(text)); }
            return new DWordUnion(Convert.ToUInt32(text, 16));
        }

        /// <summary>
        /// 将当前 <see cref="DWordUnion"/> 的值复制到指定字节数组中，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">作为复制目标的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public void CopyTo(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { *(uint*)(pBytes + offset) = this.UInt32; }
        }

        /// <summary>
        /// 确定当前 <see cref="DWordUnion"/> 与指定的 <see cref="DWordUnion"/> 是否相等。
        /// </summary>
        /// <param name="other">要与当前实例进行比较的 <see cref="DWordUnion"/>。</param>
        /// <returns>若当前 <see cref="DWordUnion"/> 的值与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(DWordUnion other) => this.UInt32 == other.UInt32;

        /// <summary>
        /// 确定当前 <see cref="DWordUnion"/> 与指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="DWordUnion"/>，
        /// 且当前 <see cref="DWordUnion"/> 的值与 <paramref name="obj"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object? obj) => (obj is DWordUnion other) && this.Equals(other);

        /// <summary>
        /// 获取当前 <see cref="DWordUnion"/> 的哈希代码。
        /// </summary>
        /// <returns>当前 <see cref="DWordUnion"/> 哈希代码。</returns>
        public override int GetHashCode() => this.UInt32.GetHashCode();

        /// <summary>
        /// 将指定字节数组的值复制到当前 <see cref="DWordUnion"/> 中，并指定数组偏移量。
        /// </summary>
        /// <param name="bytes">作为复制源的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public void LoadFrom(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { this.UInt32 = *(uint*)(pBytes + offset); }
        }

        /// <summary>
        /// 将当前 <see cref="DWordUnion"/> 的值转换为字节数组。
        /// </summary>
        /// <returns>包含当前 <see cref="DWordUnion"/> 的值的字节数组。</returns>
        public byte[] ToByteArray() { byte[] bytes = new byte[4]; this.CopyTo(bytes); return bytes; }

        /// <summary>
        /// 将当前 <see cref="DWordUnion"/> 转换为其等效的十六进制数字的字符串表示形式。
        /// </summary>
        /// <returns>表示当前 <see cref="DWordUnion"/> 的十六进制数字的字符串。</returns>
        public override string ToString() => "0x" + this.UInt32.ToString("X8");

        /// <summary>
        /// 获取序列化当前 <see cref="DWordUnion"/> 所需的数据。
        /// </summary>
        /// <param name="info">包含序列化所需信息的对象。</param>
        /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null) { throw new ArgumentNullException(nameof(info)); }
            info.AddValue("Value", this.UInt32);
        }

        /// <summary>
        /// 确定两个指定的 <see cref="DWordUnion"/> 是否相等。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="DWordUnion"/>。</param>
        /// <param name="right">要比较的第二个 <see cref="DWordUnion"/>。</param>
        /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(DWordUnion left, DWordUnion right) => left.UInt32 == right.UInt32;

        /// <summary>
        /// 确定两个指定的 <see cref="DWordUnion"/> 是否不等。
        /// </summary>
        /// <param name="left">要比较的第一个 <see cref="DWordUnion"/>。</param>
        /// <param name="right">要比较的第二个 <see cref="DWordUnion"/>。</param>
        /// <returns>若 <paramref name="left"/> 的值与 <paramref name="right"/> 的值不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(DWordUnion left, DWordUnion right) => left.UInt32 != right.UInt32;

        /// <summary>
        /// 将指定的 32 位有符号整数隐式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的 32 位有符号整数。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static implicit operator DWordUnion(int value) => new DWordUnion(value);

        /// <summary>
        /// 将指定的 32 位无符号整数隐式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的 32 位无符号整数。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        [CLSCompliant(false)]
        public static implicit operator DWordUnion(uint value) => new DWordUnion(value);

        /// <summary>
        /// 将指定的单精度浮点数隐式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的单精度浮点数。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static implicit operator DWordUnion(float value) => new DWordUnion(value);

        /// <summary>
        /// 将指定的 <see cref="DWordUnion"/> 隐式转换为 32 位有符号整数。
        /// </summary>
        /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
        /// <returns>转换得到的 32 位有符号整数。</returns>
        public static implicit operator int(DWordUnion union) => union.Int32;

        /// <summary>
        /// 将指定的 <see cref="DWordUnion"/> 隐式转换为 32 位无符号整数。
        /// </summary>
        /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
        /// <returns>转换得到的 32 位无符号整数。</returns>
        [CLSCompliant(false)]
        public static implicit operator uint(DWordUnion union) => union.UInt32;

        /// <summary>
        /// 将指定的 <see cref="DWordUnion"/> 隐式转换为单精度浮点数。
        /// </summary>
        /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
        /// <returns>转换得到的单精度浮点数。</returns>
        public static implicit operator float(DWordUnion union) => union.Single;

        /// <summary>
        /// 将指定的 <see cref="ByteUnion"/> 隐式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="ByteUnion"/>。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static implicit operator DWordUnion(ByteUnion union) => new DWordUnion((uint)union.Byte);

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 隐式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static implicit operator DWordUnion(WordUnion union) => new DWordUnion((uint)union.UInt16);

        /// <summary>
        /// 将指定的 <see cref="QWordUnion"/> 显式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static explicit operator DWordUnion(QWordUnion union) => new DWordUnion((uint)union.UInt64);

        /// <summary>
        /// 将指定的 <see cref="HandleUnion"/> 显式转换为 <see cref="DWordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
        /// <returns>转换得到的 <see cref="DWordUnion"/>。</returns>
        public static explicit operator DWordUnion(HandleUnion union) => new DWordUnion((uint)union.Pointer);
    }
}
