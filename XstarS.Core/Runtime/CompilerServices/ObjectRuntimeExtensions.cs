﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供对象运行时相关的扩展方法。
    /// </summary>
    public static class ObjectRuntimeExtensions
    {
        /// <summary>
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建浅表副本的对象。</param>
        /// <returns><paramref name="value"/> 的浅表副本。</returns>
        [return: NotNullIfNotNull("value")]
        public static T? DirectClone<T>(this T? value) =>
            (T?)ObjectRuntimeValue.DirectClone(value);

        /// <summary>
        /// 创建当前对象的深度副本。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        [return: NotNullIfNotNull("value")]
        public static T? RecursiveClone<T>(this T? value) =>
            (T?)ObjectRuntimeValue.RecursiveClone(value);

        /// <summary>
        /// 创建当前对象的序列化副本。
        /// </summary>
        /// <remarks>基于对象序列化，可能存在性能问题。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建序列化副本的对象。</param>
        /// <returns><paramref name="value"/> 的序列化副本。</returns>
        /// <exception cref="SerializationException">
        /// <paramref name="value"/> 中的某个对象未标记为可序列化。</exception>
        [return: NotNullIfNotNull("value")]
        public static T? SerializationClone<T>(this T? value) =>
            (T?)ObjectRuntimeValue.SerializationClone(value);

        /// <summary>
        /// 确定当前对象与指定对象的引用是否相等。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行引用相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的引用相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ReferenceEquals<T>(this T? value, T? other) =>
            object.ReferenceEquals(value, other);

        /// <summary>
        /// 确定当前对象与指定对象的值是否相等。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。
        /// 将递归比较至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行值相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ValueEquals<T>(this T? value, T? other) =>
            ObjectRuntimeValue.RecursiveEquals(value, other);

        /// <summary>
        /// 确定当前对象与指定对象的直接值是否相等。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要进行直接值相等比较的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的直接值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool DirectEquals<T>(this T? value, T? other) =>
            ObjectDirectValue.Equals(value, other);

        /// <summary>
        /// 获取当前对象基于引用的哈希代码。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于引用的哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于引用的哈希代码。</returns>
        public static int GetReferenceHashCode<T>(this T? value) =>
            RuntimeHelpers.GetHashCode(value!);

        /// <summary>
        /// 获取当前对象基于值的哈希代码。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。
        /// 将递归计算至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。</remarks>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        /// <returns>由 <paramref name="value"/> 基于值的哈希代码。</returns>
        public static int GetValueHashCode<T>(this T? value) =>
            ObjectRuntimeValue.GetRecursiveHashCode(value);

        /// <summary>
        /// 获取当前对象的基于直接值的哈希代码。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要获取基于直接值哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于的直接值的哈希代码。</returns>
        public static int GetDirectHashCode<T>(this T? value) =>
            ObjectDirectValue.GetHashCode(value);
    }
}
