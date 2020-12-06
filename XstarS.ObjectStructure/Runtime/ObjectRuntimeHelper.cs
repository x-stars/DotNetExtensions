using System;
using System.Reflection;

namespace XstarS.Runtime
{
    /// <summary>
    /// 提供对象运行时相关的帮助方法。
    /// </summary>
    internal static partial class ObjectRuntimeHelper
    {
        /// <summary>
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="value">要进行相等比较的第一个包装的指针。</param>
        /// <param name="other">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        internal static unsafe bool BoxedPointerEquals(object value, object other)
        {
            return Pointer.Unbox(value) == Pointer.Unbox(other);
        }

        /// <summary>
        /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
        /// </summary>
        /// <param name="value">要获取哈希代码的包装的指针。</param>
        /// <returns><paramref name="value"/> 中包装的指针的哈希代码。</returns>
        internal static unsafe int GetBoxedPointerHashCode(object value)
        {
            return ((IntPtr)Pointer.Unbox(value)).GetHashCode();
        }
    }
}
