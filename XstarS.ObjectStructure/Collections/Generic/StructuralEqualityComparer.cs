using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 为结构化对象中的元素的相等比较器提供抽象基类。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    public abstract class StructuralEqualityComparer<T> : SimpleAcyclicEqualityComparer<T>
    {
        /// <summary>
        /// 表示 <see cref="StructuralEqualityComparer{T}.Default"/> 的延迟初始化值。 
        /// </summary>
        private static readonly Lazy<StructuralEqualityComparer<T>> LazyDefault =
            new Lazy<StructuralEqualityComparer<T>>(
                StructuralEqualityComparer<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructuralEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        protected StructuralEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        public static new StructuralEqualityComparer<T> Default =>
            StructuralEqualityComparer<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        private static StructuralEqualityComparer<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var itemType = type.GetElementType()!;
                if (itemType.IsPointer)
                {
                    return new PointerArrayEqualityComparer<T>();
                }
                else if (itemType.MakeArrayType() == type)
                {
                    return (StructuralEqualityComparer<T>)Activator.CreateInstance(
                        typeof(SZArrayEqualityComparer<>).MakeGenericType(itemType))!;
                }
                else
                {
                    return new ArrayEqualityComparer<T>();
                }
            }
            else if (type == typeof(string))
            {
                return new PlainEqualityComparer<T>();
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return (StructuralEqualityComparer<T>)Activator.CreateInstance(
                    typeof(EnumerableEqualityComparer<>).MakeGenericType(type))!;
            }
            else if (type == typeof(DictionaryEntry))
            {
                return (StructuralEqualityComparer<T>)
                    (object)new DictionaryEntryEqualityComparer();
            }
            else if (type.IsGenericType &&
                (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
            {
                var keyValueTypes = type.GetGenericArguments();
                return (StructuralEqualityComparer<T>)Activator.CreateInstance(
                    typeof(KeyValuePairEqualityComparer<,>).MakeGenericType(keyValueTypes))!;
            }
            else
            {
                return new PlainEqualityComparer<T>();
            }
        }

        /// <summary>
        /// 组合当前哈希代码和新的哈希代码。
        /// </summary>
        /// <param name="hashCode">当前哈希代码。</param>
        /// <param name="nextHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Performance", "CA1822: MarkMembersAsStatic")]
        protected int CombineHashCode(int hashCode, int nextHashCode)
        {
            return hashCode * -1521134295 + nextHashCode;
        }
    }

    /// <summary>
    /// 提供结构化对象中的元素的相等比较的方法。
    /// </summary>
    public static class StructuralEqualityComparer
    {
        /// <summary>
        /// 表示指定类型的 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
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
        public static int GetHashCode(object? obj)
        {
            var comparer = StructuralEqualityComparer.OfType(obj?.GetType());
            return comparer.GetHashCode(obj!);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        internal static IAcyclicEqualityComparer OfType(Type? type)
        {
            return (type is null) ?
                StructuralEqualityComparer<object>.Default :
                StructuralEqualityComparer.Defaults.GetOrAdd(
                    type, StructuralEqualityComparer.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructuralEqualityComparer{T}"/> 类的默认实例。</returns>
        private static IAcyclicEqualityComparer GetDefault(Type type)
        {
            var typeComparer = typeof(StructuralEqualityComparer<>).MakeGenericType(type);
            var nameDefualt = nameof(StructuralEqualityComparer<object>.Default);
            var propertyDefault = typeComparer.GetProperty(nameDefualt);
            return (IAcyclicEqualityComparer)propertyDefault!.GetValue(null)!;
        }
    }
}
