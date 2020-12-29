using System;
using System.Collections;
using System.Collections.Generic;
using XstarS.Collections;
using XstarS.Collections.Generic;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 为结构化对象的字符串表示对象提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    internal abstract class StructuralRepresenterBase<T> : AcyclicRepresenter<T>
    {
        /// <summary>
        /// 表示 <see cref="StructuralRepresenterBase{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<StructuralRepresenterBase<T>> LazyDefault =
            new Lazy<StructuralRepresenterBase<T>>(
                StructuralRepresenterBase<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="StructuralRepresenterBase{T}"/> 类的新实例。
        /// </summary>
        protected StructuralRepresenterBase() { }

        /// <summary>
        /// 获取 <see cref="StructuralRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralRepresenterBase{T}"/> 类的默认实例。</returns>
        public new static StructuralRepresenterBase<T> Default =>
            StructuralRepresenterBase<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="StructuralRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralRepresenterBase{T}"/> 类的默认实例。</returns>
        private static StructuralRepresenterBase<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsArray)
            {
                var itemType = type.GetElementType();
                if (itemType.IsPointer)
                {
                    return new PointerArrayRepresenter<T>();
                }
                if (itemType.MakeArrayType() == type)
                {
                    return (StructuralRepresenterBase<T>)Activator.CreateInstance(
                        typeof(SZArrayRepresenter<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayRepresenter<T>();
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                return (StructuralRepresenterBase<T>)Activator.CreateInstance(
                    typeof(SZArrayRepresenter<>).MakeGenericType(type));
            }
            else if (type == typeof(DictionaryEntry))
            {
                return (StructuralRepresenterBase<T>)(object)new DictionaryEntryRepresenter();
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (StructuralRepresenterBase<T>)Activator.CreateInstance(
                        typeof(KeyValuePairRepresenter<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return new PlainRepresenter<T>();
                }
            }
            else
            {
                return new PlainRepresenter<T>();
            }
        }
    }
}
