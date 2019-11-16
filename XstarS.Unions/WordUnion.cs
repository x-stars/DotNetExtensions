using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace XstarS.Unions
{
    /// <summary>
    /// 表示 16 位数据类型的联合。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    public struct WordUnion : IEquatable<WordUnion>, ISerializable
    {
        /// <summary>
        /// 当前 <see cref="WordUnion"/> 的 16 位有符号整数。
        /// </summary>
        [FieldOffset(0)] public short Int16;

        /// <summary>
        /// 当前 <see cref="WordUnion"/> 的 16 位无符号整数。
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)] public ushort UInt16;

        /// <summary>
        /// 当前 <see cref="WordUnion"/> 的 UTF-16 字符。
        /// </summary>
        [FieldOffset(0)] public char Char;

        /// <summary>
        /// 当前 <see cref="WordUnion"/> 低 8 位的 <see cref="ByteUnion"/>。
        /// </summary>
        [FieldOffset(0)] public ByteUnion LowByte;

        /// <summary>
        /// 当前 <see cref="WordUnion"/> 的高 8 位的 <see cref="ByteUnion"/>。
        /// </summary>
        [FieldOffset(1)] public ByteUnion HighByte;

        /// <summary>
        /// 当前 <see cref="WordUnion"/> 的固定字节缓冲区。
        /// </summary>
        [CLSCompliant(false)]
        [FieldOffset(0)] public unsafe fixed byte Bytes[2];

        /// <summary>
        /// 将 <see cref="WordUnion"/> 结构的新实例初始化为指定的 16 位有符号整数。
        /// </summary>
        /// <param name="value">一个 16 位有符号整数。</param>
        public WordUnion(short value) : this() { this.Int16 = value; }

        /// <summary>
        /// 将 <see cref="WordUnion"/> 结构的新实例初始化为指定的 16 位无符号整数。
        /// </summary>
        /// <param name="value">一个 16 位无符号整数。</param>
        [CLSCompliant(false)]
        public WordUnion(ushort value) : this() { this.UInt16 = value; }

        /// <summary>
        /// 将 <see cref="WordUnion"/> 结构的新实例初始化为指定的 UTF-16 字符。
        /// </summary>
        /// <param name="value">一个 UTF-16 字符。</param>
        public WordUnion(char value) : this() { this.Char = value; }

        /// <summary>
        /// 将 <see cref="WordUnion"/> 结构的新实例初始化为指定字节数组表示的值，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">一个字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public WordUnion(byte[] bytes, int offset = 0) : this() { this.LoadFrom(bytes, offset); }

        /// <summary>
        /// 将 <see cref="WordUnion"/> 结构的新实例初始化为指定序列化信息和上下文表示的值。
        /// </summary>
        /// <param name="info">包含序列化所需信息的对象。</param>
        /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
        private WordUnion(SerializationInfo info, StreamingContext context) : this()
        {
            if (info is null) { throw new ArgumentNullException(nameof(info)); }
            this.Int16 = info.GetInt16("Value");
        }

        /// <summary>
        /// 将当前 <see cref="WordUnion"/> 的值复制到指定字节数组中，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">一个字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public unsafe void CopyTo(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { *(short*)(pBytes + offset) = this.Int16; }
        }

        /// <summary>
        /// 确定当前 <see cref="WordUnion"/> 与指定的 <see cref="WordUnion"/> 是否相等。
        /// </summary>
        /// <param name="other">要与当前实例进行比较的 <see cref="WordUnion"/>。</param>
        /// <returns>若当前 <see cref="WordUnion"/> 的值与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(WordUnion other) => this.Int16 == other.Int16;

        /// <summary>
        /// 确定当前 <see cref="WordUnion"/> 与指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="WordUnion"/>，
        /// 且当前 <see cref="WordUnion"/> 的值与 <paramref name="obj"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) => (obj is WordUnion other) && this.Equals(other);

        /// <summary>
        /// 获取当前 <see cref="WordUnion"/> 的哈希代码。
        /// </summary>
        /// <returns>当前 <see cref="WordUnion"/> 哈希代码。</returns>
        public override int GetHashCode() => this.Int16.GetHashCode();

        /// <summary>
        /// 将指定字节数组的值复制到当前 <see cref="WordUnion"/> 中，并指定数组偏移量。
        /// </summary>
        /// <param name="bytes">一个字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public unsafe void LoadFrom(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { this.Int16 = *(short*)(pBytes + offset); }
        }

        /// <summary>
        /// 将当前 <see cref="WordUnion"/> 的值转换为字节数组。
        /// </summary>
        /// <returns>包含当前 <see cref="WordUnion"/> 的值的字节数组。</returns>
        public byte[] ToByteArray() { byte[] bytes = new byte[2]; this.CopyTo(bytes); return bytes; }

        /// <summary>
        /// 将当前 <see cref="WordUnion"/> 转换为其等效的字符串表达形式。
        /// </summary>
        /// <returns>当前 <see cref="WordUnion"/> 的字符串表达形式。</returns>
        public override string ToString() => this.Int16.ToString();

        /// <summary>
        /// 获取序列化当前 <see cref="WordUnion"/> 所需的数据。
        /// </summary>
        /// <param name="info">包含序列化所需信息的对象。</param>
        /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info is null) { throw new ArgumentNullException(nameof(info)); }
            info.AddValue("Value", this.Int16);
        }

        /// <summary>
        /// 确定两个 <see cref="WordUnion"/> 是否相等。
        /// </summary>
        /// <param name="union1">要比较的第一个 <see cref="WordUnion"/>。</param>
        /// <param name="union2">要比较的第二个 <see cref="WordUnion"/>。</param>
        /// <returns>若 <paramref name="union1"/> 的值与 <paramref name="union2"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(WordUnion union1, WordUnion union2) => union1.Int16 == union2.Int16;

        /// <summary>
        /// 确定两个 <see cref="WordUnion"/> 是否不等。
        /// </summary>
        /// <param name="union1">要比较的第一个 <see cref="WordUnion"/>。</param>
        /// <param name="union2">要比较的第二个 <see cref="WordUnion"/>。</param>
        /// <returns>若 <paramref name="union1"/> 的值与 <paramref name="union2"/> 的值不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(WordUnion union1, WordUnion union2) => union1.Int16 != union2.Int16;

        /// <summary>
        /// 将指定的 16 位有符号整数转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的 16 位有符号整数。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static implicit operator WordUnion(short value) => new WordUnion(value);

        /// <summary>
        /// 将指定的 16 位无符号整数转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的 16 位无符号整数。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        [CLSCompliant(false)]
        public static implicit operator WordUnion(ushort value) => new WordUnion(value);

        /// <summary>
        /// 将指定的 UTF-16 字符转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="value">要转换的 UTF-16 字符。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static implicit operator WordUnion(char value) => new WordUnion(value);

        /// <summary>
        /// 将指定的字节数组转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="bytes">要转换的字节数组。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public static implicit operator WordUnion(byte[] bytes) => new WordUnion(bytes);

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 转换为 16 位有符号整数。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的 16 位有符号整数。</returns>
        public static implicit operator short(WordUnion union) => union.Int16;

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 转换为 16 位无符号整数。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的 16 位无符号整数。</returns>
        [CLSCompliant(false)]
        public static implicit operator ushort(WordUnion union) => union.UInt16;

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 转换为 UTF-16 字符。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的 UTF-16 字符。</returns>
        public static implicit operator char(WordUnion union) => union.Char;

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 转换为字节数组。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的字节数组。</returns>
        public static implicit operator byte[](WordUnion union) => union.ToByteArray();

        /// <summary>
        /// 将指定的 <see cref="ByteUnion"/> 转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="ByteUnion"/>。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static implicit operator WordUnion(ByteUnion union) => new WordUnion((short)union.Byte);

        /// <summary>
        /// 将指定的 <see cref="DWordUnion"/> 对象转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static explicit operator WordUnion(DWordUnion union) => new WordUnion((short)union.Int32);

        /// <summary>
        /// 将指定的 <see cref="QWordUnion"/> 转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static explicit operator WordUnion(QWordUnion union) => new WordUnion((short)union.Int64);

        /// <summary>
        /// 将指定的 <see cref="HandleUnion"/> 转换为 <see cref="WordUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
        /// <returns>转换得到的 <see cref="WordUnion"/>。</returns>
        public static explicit operator WordUnion(HandleUnion union) => new WordUnion((short)union.IntPtr);
    }
}
