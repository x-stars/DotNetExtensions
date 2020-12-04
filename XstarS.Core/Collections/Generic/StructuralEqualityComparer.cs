using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供结构化对象基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    public abstract class StructuralEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// 表示 <see cref="StructuralEqualityComparer{T}.Default"/> 的延迟初始化对象。 
        /// </summary>
        private static readonly Lazy<EqualityComparer<T>> LazyDefault =
            new Lazy<EqualityComparer<T>>(StructuralEqualityComparer<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructuralEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        protected StructuralEqualityComparer() { }

        /// <summary>
        /// 获取用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        public static new EqualityComparer<T> Default =>
            StructuralEqualityComparer<T>.LazyDefault.Value;

        /// <summary>
        /// 创建用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        private static EqualityComparer<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if ((itemType.MakeArrayType() == type) && !itemType.IsPointer)
                {
                    return (EqualityComparer<T>)Activator.CreateInstance(
                        typeof(SZArrayEqualityComparer<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayEqualityComparer<T>();
                }
            }
            else if (type.IsGenericType)
            {
                if (StructuralEqualityComparer<T>.IsEnumerable(type))
                {
                    return new EnumerableEqualityComparer<T>();
                }
                else if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (EqualityComparer<T>)Activator.CreateInstance(
                        typeof(KeyValuePairEqualityComparer<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return EqualityComparer<T>.Default;
                }
            }
            else
            {
                return EqualityComparer<T>.Default;
            }
        }

        /// <summary>
        /// 确定指定类型是否为 <see cref="IEnumerable{T}"/>。
        /// </summary>
        /// <param name="type">要确定是否为 <see cref="IEnumerable{T}"/> 的类型。</param>
        /// <returns>若 <paramref name="type"/> 为 <see cref="IEnumerable{T}"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        private static bool IsEnumerable(Type type)
        {
            foreach (var iType in type.GetInterfaces())
            {
                if (iType.IsGenericType &&
                    iType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return true;
                }
            }
            return false;
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
    /// 提供获取 <see cref="StructuralEqualityComparer{T}"/> 类的默认实例的方法。
    /// </summary>
    internal static class StructuralEqualityComparer
    {
        /// <summary>
        /// 获取 <see cref="StructuralEqualityComparer{T}"/> 的默认实例。
        /// </summary>
        /// <param name="type">结构对对象的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructuralEqualityComparer{T}"/> 的默认实例。</returns>
        internal static IEqualityComparer GetDefault(Type type)
        {
            var typeComparer = typeof(StructuralEqualityComparer<>).MakeGenericType(type);
            var nameDefualt = nameof(StructuralEqualityComparer<object>.Default);
            var propertyDefault = typeComparer.GetProperty(nameDefualt);
            return (IEqualityComparer)propertyDefault.GetValue(null);
        }
    }
}
