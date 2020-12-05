using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供结构化对象基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    public abstract class StructureEqualityComparer<T> : AcyclicEqualityComparer<T>
    {
        /// <summary>
        /// 表示 <see cref="StructureEqualityComparer{T}.Default"/> 的延迟初始化对象。 
        /// </summary>
        private static readonly Lazy<StructureEqualityComparer<T>> LazyDefault =
            new Lazy<StructureEqualityComparer<T>>(StructureEqualityComparer<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructureEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        protected StructureEqualityComparer() { }

        /// <summary>
        /// 获取用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        public static new StructureEqualityComparer<T> Default =>
            StructureEqualityComparer<T>.LazyDefault.Value;

        /// <summary>
        /// 创建用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        private static StructureEqualityComparer<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if ((itemType.MakeArrayType() == type) && !itemType.IsPointer)
                {
                    return (StructureEqualityComparer<T>)Activator.CreateInstance(
                        typeof(SZArrayEqualityComparer<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayEqualityComparer<T>();
                }
            }
            else if (Array.IndexOf(type.GetInterfaces(), typeof(IEnumerable)) != -1)
            {
                return new EnumerableEqualityComparer<T>();
            }
            else if (type == typeof(DictionaryEntry))
            {
                return (StructureEqualityComparer<T>)(object)new DictionaryEntryEqualityComaprer();
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (StructureEqualityComparer<T>)Activator.CreateInstance(
                        typeof(KeyValuePairEqualityComparer<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return new PlainObjectEqualityComparer<T>();
                }
            }
            else
            {
                return new PlainObjectEqualityComparer<T>();
            }
        }

        /// <summary>
        /// 组合当前哈希代码和新的哈希代码。
        /// </summary>
        /// <param name="hashCode">当前哈希代码。</param>
        /// <param name="nextHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
        protected int CombineHashCode(int hashCode, int nextHashCode)
        {
            return hashCode * -1521134295 + nextHashCode;
        }
    }

    /// <summary>
    /// 提供获取 <see cref="StructureEqualityComparer{T}"/> 类的默认实例的方法。
    /// </summary>
    internal static class StructureEqualityComparer
    {
        /// <summary>
        /// 表示指定类型的 <see cref="StructureEqualityComparer{T}"/> 的默认实例。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IAcyclicEqualityComparer> Defualts =
            new ConcurrentDictionary<Type, IAcyclicEqualityComparer>();

        /// <summary>
        /// 获取指定类型的 <see cref="StructureEqualityComparer{T}"/> 的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructureEqualityComparer{T}"/> 的默认实例。</returns>
        public static IAcyclicEqualityComparer OfType(Type type)
        {
            return (type is null) ? StructureEqualityComparer<object>.Default :
                StructureEqualityComparer.Defualts.GetOrAdd(
                    type, StructureEqualityComparer.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="StructureEqualityComparer{T}"/> 的默认实例。
        /// </summary>
        /// <param name="type">结构化对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructureEqualityComparer{T}"/> 的默认实例。</returns>
        private static IAcyclicEqualityComparer GetDefault(Type type)
        {
            var typeComparer = typeof(StructureEqualityComparer<>).MakeGenericType(type);
            var nameDefualt = nameof(StructureEqualityComparer<object>.Default);
            var propertyDefault = typeComparer.GetProperty(nameDefualt);
            return (IAcyclicEqualityComparer)propertyDefault.GetValue(null);
        }
    }
}
