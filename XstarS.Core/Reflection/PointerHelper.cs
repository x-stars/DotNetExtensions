using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供以 <see cref="Pointer"/> 包装的指针的帮助方法。
    /// </summary>
    [CLSCompliant(false)]
    public static unsafe class PointerHelper
    {
        /// <summary>
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="value">要进行相等比较的第一个包装的指针。</param>
        /// <param name="other">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException"><paramref name="value"/>
        /// 或 <paramref name="other"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        public static new bool Equals(object value, object other)
        {
            return Pointer.Unbox(value) == Pointer.Unbox(other);
        }

        /// <summary>
        /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
        /// </summary>
        /// <param name="value">要获取哈希代码的包装的指针。</param>
        /// <returns><paramref name="value"/> 中包装的指针的哈希代码。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        public static int GetHashCode(object value)
        {
            return ((IntPtr)Pointer.Unbox(value)).GetHashCode();
        }
    }
}
