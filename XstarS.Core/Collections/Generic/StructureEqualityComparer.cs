using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供结构化对象基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="TStructure">结构化对象的类型。</typeparam>
    [Serializable]
    public abstract class StructureEqualityComparer<TStructure> : EqualityComparer<TStructure>
    {
        /// <summary>
        /// 表示 <see cref="StructureEqualityComparer{TCollection}.Default"/> 的延迟初始化对象。 
        /// </summary>
        private static readonly Lazy<EqualityComparer<TStructure>> LazyDefault =
            new Lazy<EqualityComparer<TStructure>>(
                StructureEqualityComparer<TStructure>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructureEqualityComparer{TCollection}"/> 类的新实例。
        /// </summary>
        protected StructureEqualityComparer() { }

        /// <summary>
        /// 获取用于结构化对象比较的默认的 <see cref="EqualityComparer{TCollection}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{TCollection}"/> 实例。</returns>
        public static new EqualityComparer<TStructure> Default =>
            StructureEqualityComparer<TStructure>.LazyDefault.Value;

        /// <summary>
        /// 创建用于结构化对象比较的默认的 <see cref="EqualityComparer{TCollection}"/> 实例。
        /// </summary>
        /// <returns>用于结构化对象比较的默认的 <see cref="EqualityComparer{TCollection}"/> 实例。</returns>
        private static EqualityComparer<TStructure> CreateDefault()
        {
            var type = typeof(TStructure);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if ((itemType.MakeArrayType() == type) && !itemType.IsPointer)
                {
                    return (EqualityComparer<TStructure>)Activator.CreateInstance(
                        typeof(SZArrayEqualityComparer<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayEqualityComparer<TStructure>();
                }
            }
            else if (type.IsGenericType)
            {
                if (StructureEqualityComparer<TStructure>.IsEnumerable(type))
                {
                    return new EnumerableEqualityComparer<TStructure>();
                }
                else if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (EqualityComparer<TStructure>)Activator.CreateInstance(
                        typeof(KeyValuePairEqualityComparer<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return EqualityComparer<TStructure>.Default;
                }
            }
            else
            {
                return EqualityComparer<TStructure>.Default;
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
        /// <param name="nextHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
        protected int CombineHashCode(int hashCode, int nextHashCode)
        {
            return hashCode * -1521134295 + nextHashCode;
        }
    }
}
