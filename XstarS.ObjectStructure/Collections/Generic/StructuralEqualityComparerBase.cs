using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 为结构化对象中的元素的相等比较器提供抽象基类。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    internal abstract class StructuralEqualityComparerBase<T> : AcyclicEqualityComparer<T>
    {
        /// <summary>
        /// 表示 <see cref="StructuralEqualityComparerBase{T}.Default"/> 的延迟初始化值。 
        /// </summary>
        private static readonly Lazy<StructuralEqualityComparerBase<T>> LazyDefault =
            new Lazy<StructuralEqualityComparerBase<T>>(StructuralEqualityComparerBase<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructuralEqualityComparerBase{T}"/> 类的新实例。
        /// </summary>
        protected StructuralEqualityComparerBase() { }

        /// <summary>
        /// 获取 <see cref="StructuralEqualityComparerBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralEqualityComparerBase{T}"/> 类的默认实例。</returns>
        public static new StructuralEqualityComparerBase<T> Default =>
            StructuralEqualityComparerBase<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="StructuralEqualityComparerBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralEqualityComparerBase{T}"/> 类的默认实例。</returns>
        private static StructuralEqualityComparerBase<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if (itemType.IsPointer)
                {
                    return new PointerArrayEqualityComparer<T>();
                }
                else if (itemType.MakeArrayType() == type)
                {
                    return (StructuralEqualityComparerBase<T>)Activator.CreateInstance(
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
                return (StructuralEqualityComparerBase<T>)(object)new DictionaryEntryEqualityComaprer();
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (StructuralEqualityComparerBase<T>)Activator.CreateInstance(
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
}
