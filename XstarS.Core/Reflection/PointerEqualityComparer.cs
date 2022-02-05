using System;
using System.Reflection;
using XstarS.Collections.Generic;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供以 <see cref="Pointer"/> 包装的指针的帮助方法。
    /// </summary>
    [Serializable]
    [CLSCompliant(false)]
    public sealed unsafe class PointerEqualityComparer : SimpleEqualityComparer<Pointer>
    {
        /// <summary>
        /// 初始化 <see cref="PointerEqualityComparer"/> 类的新实例。
        /// </summary>
        public PointerEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="PointerEqualityComparer"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="PointerEqualityComparer"/> 类的默认实例。</returns>
        public static new PointerEqualityComparer Default { get; } = new PointerEqualityComparer();

        /// <summary>
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="x">要进行相等比较的第一个包装的指针。</param>
        /// <param name="y">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException"><paramref name="x"/>
        /// 或 <paramref name="y"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public static new bool Equals(object? x, object? y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) || (y is null)) { return false; }
            return Pointer.Unbox(x) == Pointer.Unbox(y);
        }

        /// <summary>
        /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的包装的指针。</param>
        /// <returns><paramref name="obj"/> 中包装的指针的哈希代码。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1061:DoNotHideBaseClassMethods")]
        public static int GetHashCode(object? obj) =>
            (obj is null) ? 0 : ((nint)Pointer.Unbox(obj)).GetHashCode();

        /// <summary>
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="x">要进行相等比较的第一个包装的指针。</param>
        /// <param name="y">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException"><paramref name="x"/>
        /// 或 <paramref name="y"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        public override bool Equals(Pointer? x, Pointer? y) => PointerEqualityComparer.Equals(x, y);

        /// <summary>
        /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的包装的指针。</param>
        /// <returns><paramref name="obj"/> 中包装的指针的哈希代码。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> 不为以 <see cref="Pointer"/> 包装的指针。</exception>
        public override int GetHashCode(Pointer? obj) => PointerEqualityComparer.GetHashCode(obj);
    }
}
