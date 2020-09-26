using System;
using System.Runtime.CompilerServices;

namespace XstarS
{
    /// <summary>
    /// 提供类型无关的通用泛型扩展方法。
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建浅表副本的对象。</param>
        /// <returns><paramref name="value"/> 的浅表副本。</returns>
        public static T ShallowClone<T>(this T value) =>
            (T)new CloneableObject(value).ShallowClone();

        /// <summary>
        /// 创建当前对象的深度副本。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        public static T DeepClone<T>(this T value) =>
            (T)new CloneableObject(value).DeepClone();

        /// <summary>
        /// 确定当前对象与指定对象的引用是否相等。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行引用相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的引用相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ReferenceEquals<T>(this T value, T other) =>
            object.ReferenceEquals(value, other);

        /// <summary>
        /// 确定当前对象与指定对象的值是否相等。
        /// 将递归比较至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行值相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ValueEquals<T>(this T value, T other) =>
            new ValueEquatablePair(value, other).ValueEquals();

        /// <summary>
        /// 获取当前对象基于引用的哈希代码。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于引用的哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于引用的哈希代码。</returns>
        public static int GetReferenceHashCode<T>(this T value) =>
            RuntimeHelpers.GetHashCode(value);

        /// <summary>
        /// 获取当前对象基于值的哈希代码。
        /// 将递归计算至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        /// <returns>由 <paramref name="value"/> 基于值的哈希代码。</returns>
        public static int GetValueHashCode<T>(this T value) =>
            new ValueHashableObject(value).GetValueHashCode();
    }
}
