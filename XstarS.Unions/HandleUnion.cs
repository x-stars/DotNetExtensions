using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

using nint = System.IntPtr;
using nuint = System.UIntPtr;

namespace XstarS.Unions
{
    /// <summary>
    /// 表示指针或句柄类型的联合。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Explicit)]
    public struct HandleUnion : IEquatable<HandleUnion>, ISerializable
    {
        /// <summary>
        /// 表示已经初始化为零的 <see cref="HandleUnion"/>。
        /// </summary>
        public static readonly HandleUnion Zero;

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
        [FieldOffset(0)] public unsafe void* Pointer;

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
        public unsafe HandleUnion(void* value) : this() { this.Pointer = value; }

        /// <summary>
        /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定字节数组表示的值，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">包含值的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public HandleUnion(byte[] bytes, int offset = 0) : this() { this.LoadFrom(bytes, offset); }

        /// <summary>
        /// 将 <see cref="HandleUnion"/> 结构的新实例初始化为指定序列化信息和上下文表示的值。
        /// </summary>
        /// <param name="info">包含序列化所需信息的对象。</param>
        /// <param name="context">包含序列化流的源和目标的上下文对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="info"/> 为 <see langword="null"/>。</exception>
        private HandleUnion(SerializationInfo info, StreamingContext context) : this()
        {
            if (info is null) { throw new ArgumentNullException(nameof(info)); }
            this.IntPtr = (nint)info.GetValue("Value", typeof(nint));
        }

        /// <summary>
        /// 将当前 <see cref="HandleUnion"/> 的值复制到指定字节数组中，并指定数组的偏移量。
        /// </summary>
        /// <param name="bytes">作为复制目标的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public unsafe void CopyTo(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { *(nint*)(pBytes + offset) = this.IntPtr; }
        }

        /// <summary>
        /// 确定当前 <see cref="HandleUnion"/> 与指定的 <see cref="HandleUnion"/> 是否相等。
        /// </summary>
        /// <param name="other">要与当前实例进行比较的 <see cref="HandleUnion"/>。</param>
        /// <returns>若当前 <see cref="HandleUnion"/> 的值与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(HandleUnion other) => this.IntPtr == other.IntPtr;

        /// <summary>
        /// 确定当前 <see cref="HandleUnion"/> 与指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <returns>若 <paramref name="obj"/> 为 <see cref="HandleUnion"/>，
        /// 且当前 <see cref="HandleUnion"/> 的值与 <paramref name="obj"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) => (obj is HandleUnion other) && this.Equals(other);

        /// <summary>
        /// 获取当前 <see cref="HandleUnion"/> 的哈希代码。
        /// </summary>
        /// <returns>当前 <see cref="HandleUnion"/> 哈希代码。</returns>
        public override int GetHashCode() => this.IntPtr.GetHashCode();

        /// <summary>
        /// 将指定字节数组的值复制到当前 <see cref="HandleUnion"/> 中，并指定数组偏移量。
        /// </summary>
        /// <param name="bytes">作为复制源的字节数组。</param>
        /// <param name="offset">字节数组的偏移量。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public unsafe void LoadFrom(byte[] bytes, int offset = 0)
        {
            if (bytes is null) { throw new ArgumentNullException(nameof(bytes)); }
            fixed (byte* pBytes = bytes) { this.IntPtr = *(nint*)(pBytes + offset); }
        }

        /// <summary>
        /// 将当前 <see cref="HandleUnion"/> 的值转换为字节数组。
        /// </summary>
        /// <returns>包含当前 <see cref="HandleUnion"/> 的值的字节数组。</returns>
        public byte[] ToByteArray() { byte[] bytes = new byte[nint.Size]; this.CopyTo(bytes); return bytes; }

        /// <summary>
        /// 将当前 <see cref="HandleUnion"/> 转换为其等效的字符串表达形式。
        /// </summary>
        /// <returns>当前 <see cref="HandleUnion"/> 的字符串表达形式。</returns>
        public override string ToString() => this.IntPtr.ToString();

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
            info.AddValue("Value", this.IntPtr, typeof(nint));
        }

        /// <summary>
        /// 确定两个 <see cref="HandleUnion"/> 是否相等。
        /// </summary>
        /// <param name="union1">要比较的第一个 <see cref="HandleUnion"/>。</param>
        /// <param name="union2">要比较的第二个 <see cref="HandleUnion"/>。</param>
        /// <returns>若 <paramref name="union1"/> 的值与 <paramref name="union2"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator ==(HandleUnion union1, HandleUnion union2) => union1.IntPtr == union2.IntPtr;

        /// <summary>
        /// 确定两个 <see cref="HandleUnion"/> 是否不等。
        /// </summary>
        /// <param name="union1">要比较的第一个 <see cref="HandleUnion"/>。</param>
        /// <param name="union2">要比较的第二个 <see cref="HandleUnion"/>。</param>
        /// <returns>若 <paramref name="union1"/> 的值与 <paramref name="union2"/> 的值不等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool operator !=(HandleUnion union1, HandleUnion union2) => union1.IntPtr != union2.IntPtr;

        /// <summary>
        /// 将指定的有符号指针或句柄转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="value">要转换的有符号指针或句柄。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        public static implicit operator HandleUnion(nint value) => new HandleUnion(value);

        /// <summary>
        /// 将指定的无符号指针或句柄转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="value">要转换的无符号指针或句柄。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        [CLSCompliant(false)]
        public static implicit operator HandleUnion(nuint value) => new HandleUnion(value);

        /// <summary>
        /// 将指定的指向未指定类型的指针转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="value">要转换的指向未指定类型的指针。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        [CLSCompliant(false)]
        public static unsafe implicit operator HandleUnion(void* value) => new HandleUnion(value);

        /// <summary>
        /// 将指定的 <see cref="HandleUnion"/> 转换为有符号指针或句柄。
        /// </summary>
        /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
        /// <returns>转换得到的有符号指针或句柄。</returns>
        public static implicit operator nint(HandleUnion union) => union.IntPtr;

        /// <summary>
        /// 将指定的 <see cref="HandleUnion"/> 转换为无符号指针或句柄。
        /// </summary>
        /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
        /// <returns>转换得到的无符号指针或句柄。</returns>
        [CLSCompliant(false)]
        public static implicit operator nuint(HandleUnion union) => union.UIntPtr;

        /// <summary>
        /// 将指定的 <see cref="HandleUnion"/> 转换为指向未指定类型的指针。
        /// </summary>
        /// <param name="union">要转换的 <see cref="HandleUnion"/>。</param>
        /// <returns>转换得到的指向未指定类型的指针。</returns>
        [CLSCompliant(false)]
        public static unsafe implicit operator void*(HandleUnion union) => union.Pointer;

        /// <summary>
        /// 将指定的 <see cref="ByteUnion"/> 转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="ByteUnion"/>。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        public static explicit operator HandleUnion(ByteUnion union) => new HandleUnion((nint)union.Byte);

        /// <summary>
        /// 将指定的 <see cref="WordUnion"/> 转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="WordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        public static explicit operator HandleUnion(WordUnion union) => new HandleUnion((nint)union.Int16);

        /// <summary>
        /// 将指定的 <see cref="DWordUnion"/> 转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="DWordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        public static explicit operator HandleUnion(DWordUnion union) => new HandleUnion((nint)union.Int32);

        /// <summary>
        /// 将指定的 <see cref="QWordUnion"/> 转换为 <see cref="HandleUnion"/>。
        /// </summary>
        /// <param name="union">要转换的 <see cref="QWordUnion"/>。</param>
        /// <returns>转换得到的 <see cref="HandleUnion"/>。</returns>
        public static explicit operator HandleUnion(QWordUnion union) => new HandleUnion((nint)union.Int64);
    }
}
