namespace XstarS.Runtime
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
        /// 提供指定类型的相关属性。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        private static class TypeProperties<T>
        {
            /// <summary>
            /// 表示当前类型的直接值以字节为单位的大小。
            /// </summary>
            internal static readonly int Size = TypeProperties<T>.GetSize();

            /// <summary>
            /// 获取当前类型的直接值以字节为单位的大小。
            /// </summary>
            /// <returns>当前类型以字节为单位的大小。</returns>
            private static int GetSize()
            {
                var array = new T[2];
                var ref0 = __makeref(array[0]);
                var ref1 = __makeref(array[1]);
                var ptr0 = *(byte**)&ref0;
                var ptr1 = *(byte**)&ref1;
                return (int)(ptr1 - ptr0);
            }
        }

        /// <summary>
        /// 获取当前类型的直接值以字节为单位的大小。
        /// </summary>
        /// <typeparam name="T">要获取直接值大小的类型。</typeparam>
        /// <returns>当前类型的直接值以字节为单位的大小。</returns>
        public static int SizeOf<T>() => TypeProperties<T>.Size;

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
            var refValue = __makeref(value);
            var refOther = __makeref(other);
            var ptrValue = *(void**)&refValue;
            var ptrOther = *(void**)&refOther;
            var size = ObjectDirectValue.SizeOf<T>();
            return BinaryEqualityComparer.Equals(ptrValue, ptrOther, size);
        }

        /// <summary>
        /// 获取当前对象的基于直接值的哈希代码。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于直接值哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于的直接值的哈希代码。</returns>
        public static int GetHashCode<T>(T value)
        {
            var refValue = __makeref(value);
            var ptrValue = *(void**)&refValue;
            var size = TypeProperties<T>.Size;
            return BinaryEqualityComparer.GetHashCode(ptrValue, size);
        }

        /// <summary>
        /// 将当前对象的直接值填充到对应长度的字节数组并返回。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要将直接值填充到字节数组的对象。</param>
        /// <returns>以 <paramref name="value"/> 的直接值填充的字节数组。</returns>
        public static byte[] ToByteArray<T>(T value)
        {
            var refValue = __makeref(value);
            var ptrValue = *(byte**)&refValue;
            var size = TypeProperties<T>.Size;
            var bytes = new byte[size];
            for (int offset = 0; offset < size; offset++)
            {
                bytes[offset] = ptrValue[offset];
            }
            return bytes;
        }
    }
}
