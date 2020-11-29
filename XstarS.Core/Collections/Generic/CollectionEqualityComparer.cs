using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供集合基于元素的相等比较的方法。
    /// </summary>
    [Serializable]
    public abstract class CollectionEqualityComparer<TCollection> : EqualityComparer<TCollection>
    {
        /// <summary>
        /// 表示 <see cref="CollectionEqualityComparer{T}.Default"/> 的延迟初始化对象。 
        /// </summary>
        private static readonly Lazy<EqualityComparer<TCollection>> LazyDefault =
            new Lazy<EqualityComparer<TCollection>>(
                CollectionEqualityComparer<TCollection>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="CollectionEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        protected CollectionEqualityComparer() { }

        /// <summary>
        /// 获取用于集合比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于集合比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        public static new EqualityComparer<TCollection> Default =>
            CollectionEqualityComparer<TCollection>.LazyDefault.Value;

        /// <summary>
        /// 创建用于集合比较的默认的 <see cref="EqualityComparer{T}"/> 实例。
        /// </summary>
        /// <returns>用于集合比较的默认的 <see cref="EqualityComparer{T}"/> 实例。</returns>
        private static EqualityComparer<TCollection> CreateDefault()
        {
            var type = typeof(TCollection);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if ((itemType.MakeArrayType() == type) && !itemType.IsPointer)
                {
                    return (EqualityComparer<TCollection>)Activator.CreateInstance(
                        typeof(SZArrayEqualityComparer<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayEqualityComparer<TCollection>();
                }
            }
            else if (type.IsGenericType)
            {
                if (CollectionEqualityComparer<TCollection>.IsEnumerable(type))
                {
                    return new EnumerableEqualityComparer<TCollection>();
                }
                else if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (EqualityComparer<TCollection>)Activator.CreateInstance(
                        typeof(KeyValuePairEqualityComparer<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return EqualityComparer<TCollection>.Default;
                }
            }
            else
            {
                return EqualityComparer<TCollection>.Default;
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
                if (iType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
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
        /// <param name="newHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="newHashCode"/> 组合得到的哈希代码。</returns>
        protected int CombineHashCode(int hashCode, int newHashCode)
        {
            return hashCode * -1521134295 + newHashCode;
        }
    }
}
