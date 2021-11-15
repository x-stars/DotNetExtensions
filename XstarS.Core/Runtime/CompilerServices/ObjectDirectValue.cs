using System.Runtime.CompilerServices;

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供对象的直接值相关的帮助方法。
    /// </summary>
    /// <remarks>
    /// 对于引用类型，直接值即为对象的引用；对于值类型，直接值即为对象所有字段的序列。
    /// </remarks>
    public static unsafe class ObjectDirectValue
    {
        /// <summary>
        /// 确定当前对象与指定对象的直接值是否相等。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行直接值相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的直接值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool Equals<T>(T value, T other)
        {
            var pValue = Unsafe.AsPointer(ref value);
            var pOther = Unsafe.AsPointer(ref other);
            var size = Unsafe.SizeOf<T>();
            return BinaryEqualityComparer.Equals(pValue, pOther, size);
        }

        /// <summary>
        /// 获取当前对象的基于直接值的哈希代码。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于直接值哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于的直接值的哈希代码。</returns>
        public static int GetHashCode<T>(T value)
        {
            var pValue = Unsafe.AsPointer(ref value);
            var size = Unsafe.SizeOf<T>();
            return BinaryEqualityComparer.GetHashCode(pValue, size);
        }

        /// <summary>
        /// 将当前对象的直接值填充到对应长度的字节数组并返回。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要将直接值填充到字节数组的对象。</param>
        /// <returns>以 <paramref name="value"/> 的直接值填充的字节数组。</returns>
        public static byte[] ToByteArray<T>(T value)
        {
            var pValue = Unsafe.AsPointer(ref value);
            var size = Unsafe.SizeOf<T>();
            var bytes = new byte[size];
            fixed (byte* pBytes = bytes)
            {
                Unsafe.CopyBlock(pBytes, pValue, (uint)size);
            }
            return bytes;
        }
    }
}
