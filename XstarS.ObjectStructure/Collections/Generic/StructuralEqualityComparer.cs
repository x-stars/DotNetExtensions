using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供结构化对象中的元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    public sealed class StructuralEqualityComparer<T> : SimpleEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="StructuralEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public StructuralEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        public static new StructuralEqualityComparer<T> Default { get; } =
            new StructuralEqualityComparer<T>();

        /// <summary>
        /// 确定两个指定的结构化对象中的元素是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个结构化对象。</param>
        /// <param name="y">要比较的第二个结构化对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals([AllowNull] T x, [AllowNull] T y) =>
            StructuralEqualityComparer.Equals(x, y);

        /// <summary>
        /// 获取指定的结构化对象中的元素的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的结构化对象。</param>
        /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
        public override int GetHashCode([DisallowNull] T obj) =>
            StructuralEqualityComparer.GetHashCode(obj);
    }

    /// <summary>
    /// 提供结构化对象中的元素的相等比较的方法。
    /// </summary>
    public static class StructuralEqualityComparer
    {
        /// <summary>
        /// 表示指定类型的 <see cref="InternalStructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IAcyclicEqualityComparer> Defaults =
            new ConcurrentDictionary<Type, IAcyclicEqualityComparer>();

        /// <summary>
        /// 确定两个指定的结构化对象中的元素是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个结构化对象。</param>
        /// <param name="y">要比较的第二个结构化对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static new bool Equals(object? x, object? y)
        {
            if (x?.GetType() != y?.GetType()) { return false; }

            var comparer = StructuralEqualityComparer.OfType(x?.GetType());
            return comparer.Equals(x, y);
        }

        /// <summary>
        /// 获取指定的结构化对象中的元素的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的结构化对象。</param>
        /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
        public static int GetHashCode(object obj)
        {
            var comparer = StructuralEqualityComparer.OfType(obj?.GetType());
            return comparer.GetHashCode(obj!);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="InternalStructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="InternalStructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        internal static IAcyclicEqualityComparer OfType(Type? type)
        {
            return (type is null) ?
                InternalStructuralEqualityComparer<object>.Default :
                StructuralEqualityComparer.Defaults.GetOrAdd(
                    type, StructuralEqualityComparer.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="InternalStructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="InternalStructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        private static IAcyclicEqualityComparer GetDefault(Type type)
        {
            var typeComparer = typeof(InternalStructuralEqualityComparer<>).MakeGenericType(type);
            var nameDefualt = nameof(InternalStructuralEqualityComparer<object>.Default);
            var propertyDefault = typeComparer.GetProperty(nameDefualt);
            return (IAcyclicEqualityComparer)propertyDefault!.GetValue(null)!;
        }
    }
}
