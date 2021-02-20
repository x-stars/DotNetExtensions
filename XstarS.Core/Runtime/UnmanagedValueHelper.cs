namespace XstarS.Runtime
{
    /// <summary>
    /// 提供非托管类型的值相关的帮助方法。
    /// </summary>
    public static class UnmanagedValueHelper
    {
        /// <summary>
        /// 提供指定非托管类型的相关常数值。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        private static class Constants<T> where T : unmanaged
        {
            /// <summary>
            /// 表示当前非托管类型以字节为单位的大小。
            /// </summary>
            internal static readonly unsafe int Size = sizeof(T);
        }

        /// <summary>
        /// 获取当前非托管类型以字节为单位的大小。
        /// </summary>
        /// <typeparam name="T">要获取大小的非托管类型。</typeparam>
        /// <returns>当前非托管类型以字节为单位的大小。</returns>
        public static int SizeOf<T>() where T : unmanaged => Constants<T>.Size;

        /// <summary>
        /// 确定当前非托管类型的值与指定非托管类型的值是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要进行二进制相等比较的非托管类型的值。</param>
        /// <param name="other">要与当前值进行比较的非托管类型的值。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static unsafe bool BinaryEquals<T>(this T value, T other)
            where T : unmanaged
        {
            var size = UnmanagedValueHelper.SizeOf<T>();
            return UnmanagedValueHelper.BinaryEquals(&value, &other, size);
        }

        /// <summary>
        /// 获取当前非托管类型的值基于二进制值的哈希代码。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要获取基于二进制值的哈希代码的非托管类型的值。</param>
        /// <returns><paramref name="value"/> 基于二进制值的哈希代码。</returns>
        public static unsafe int GetBinaryHashCode<T>(this T value)
            where T : unmanaged
        {
            var size = UnmanagedValueHelper.SizeOf<T>();
            return UnmanagedValueHelper.GetBinaryHashCode(&value, size);
        }

        /// <summary>
        /// 确定当前非托管类型的数组与指定非托管类型的数组是否二进制相等。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要进行二进制相等比较的非托管类型的数组。</param>
        /// <param name="other">要与当前值进行比较的非托管类型的数组。</param>
        /// <returns>若 <paramref name="array"/> 与 <paramref name="other"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static unsafe bool BinaryEquals<T>(this T[] array, T[] other)
            where T : unmanaged
        {
            if (object.ReferenceEquals(array, other)) { return true; }
            if ((array is null) ^ (other is null)) { return false; }
            if (array.Length != other.Length) { return false; }

            fixed (T* pArray = array, pOther = other)
            {
                var size = UnmanagedValueHelper.SizeOf<T>() * array.Length;
                return UnmanagedValueHelper.BinaryEquals(pArray, pOther, size);
            }
        }

        /// <summary>
        /// 获取当前非托管类型的数组基于二进制值的哈希代码。
        /// </summary>
        /// <typeparam name="T">非托管类型的数组中的元素的类型。</typeparam>
        /// <param name="array">要获取基于二进制值的哈希代码的非托管类型的数组。</param>
        /// <returns><paramref name="array"/> 基于二进制值的哈希代码。</returns>
        public static unsafe int GetBinaryHashCode<T>(this T[] array)
            where T : unmanaged
        {
            if (array is null) { return 0; }

            fixed (T* pArray = array)
            {
                var size = UnmanagedValueHelper.SizeOf<T>() * array.Length;
                return UnmanagedValueHelper.GetBinaryHashCode(pArray, size);
            }
        }

        /// <summary>
        /// 将当前非托管类型的二进制值填充到对应长度的字节数组并返回。
        /// </summary>
        /// <typeparam name="T">非托管值的类型。</typeparam>
        /// <param name="value">要填充到字节数组的非托管类型的值。</param>
        /// <returns>以 <paramref name="value"/> 的二进制值填充的字节数组。</returns>
        public static unsafe byte[] BinaryToByteArray<T>(this T value)
            where T : unmanaged
        {
            var size = UnmanagedValueHelper.SizeOf<T>();
            var bytes = new byte[size];
            fixed (byte* pBytes = bytes)
            {
                *(T*)pBytes = value;
            }
            return bytes;
        }

        /// <summary>
        /// 确定指定的两个指针指向的值是否二进制相等。
        /// </summary>
        /// <param name="value">指向要进行二进制相等比较的值的指针。</param>
        /// <param name="other">指向要与当前值进行比较的值的指针。</param>
        /// <param name="size">指针指向的值以字节为单位的大小。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 指向的值二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        internal static unsafe bool BinaryEquals(void* value, void* other, int size)
        {
            switch (size)
            {
                case 0:
                    return true;
                case 1:
                    return *(byte*)value == *(byte*)other;
                case 2:
                    return *(ushort*)value == *(ushort*)other;
                case 3:
                    var sValue = (ushort*)value;
                    var sOther = (ushort*)other;
                    return (*sValue++ == *sOther++) &&
                        UnmanagedValueHelper.BinaryEquals(sValue, sOther, size - 2);
                case 4:
                    return *(uint*)value == *(uint*)other;
                case 5:
                case 6:
                case 7:
                    var iValue = (uint*)value;
                    var iOther = (uint*)other;
                    return (*iValue++ == *iOther++) &&
                        UnmanagedValueHelper.BinaryEquals(iValue, iOther, size - 4);
                case 8:
                    return *(ulong*)value == *(ulong*)other;
                default:
                    var lValue = (ulong*)value;
                    var lOther = (ulong*)other;
                    var pEnd = (void*)((byte*)value + size);
                    while (lValue < pEnd)
                    {
                        if (*lValue++ != *lOther++)
                        {
                            return false;
                        }
                    }
                    return (lValue == pEnd) ||
                        UnmanagedValueHelper.BinaryEquals(lValue, lOther, size % 8);
            }
        }

        /// <summary>
        /// 获取指定的指针指向的值基于二进制的哈希代码。
        /// </summary>
        /// <param name="value">指向要获取基于二进制的哈希代码的值的指针。</param>
        /// <param name="size">指针指向的值以字节为单位的大小。</param>
        /// <returns><paramref name="value"/> 指向的值基于二进制的哈希代码。</returns>
        internal static unsafe int GetBinaryHashCode(void* value, int size)
        {
            switch (size)
            {
                case 0:
                    return 0;
                case 1:
                    return *(sbyte*)value;
                case 2:
                    return *(short*)value;
                case 3:
                    var sValue = (short*)value;
                    return *sValue++ * -1521134295 +
                        UnmanagedValueHelper.GetBinaryHashCode(sValue, size - 2);
                case 4:
                    return *(int*)value;
                default:
                    var hashCode = 0;
                    var iValue = (int*)value;
                    var pEnd = (void*)((byte*)value + size);
                    while (iValue < pEnd)
                    {
                        hashCode = hashCode * -1521134295 + *iValue++;
                    }
                    return (iValue == pEnd) ? hashCode : (hashCode * -1521134295 +
                        UnmanagedValueHelper.GetBinaryHashCode(iValue, size % 4));
            }
        }
    }
}
