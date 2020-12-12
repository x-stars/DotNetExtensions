using System;
using System.Collections;
using System.Collections.Generic;
using XstarS.Collections;
using XstarS.Collections.Generic;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IObjectRepresenter{T}"/> 的抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    internal abstract class ObjectRepresenterBase<T> : AcyclicObjectRepresenter<T>
    {
        /// <summary>
        /// 表示 <see cref="ObjectRepresenterBase{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ObjectRepresenterBase<T>> LazyDefault =
            new Lazy<ObjectRepresenterBase<T>>(ObjectRepresenterBase<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="ObjectRepresenterBase{T}"/> 类的新实例。
        /// </summary>
        protected ObjectRepresenterBase() { }

        /// <summary>
        /// 获取 <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        public new static ObjectRepresenterBase<T> Default => ObjectRepresenterBase<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        private static ObjectRepresenterBase<T> CreateDefault()
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
                    return (ObjectRepresenterBase<T>)Activator.CreateInstance(
                        typeof(SZArrayRepresenter<>).MakeGenericType(itemType));
                }
                else
                {
                    return new ArrayRepresenter<T>();
                }
            }
            else if (Array.IndexOf(type.GetInterfaces(), typeof(IEnumerable)) != -1)
            {
                return new EnumerableRepresenter<T>();
            }
            else if (type == typeof(DictionaryEntry))
            {
                return (ObjectRepresenterBase<T>)(object)new DictionaryEntryRepresenter();
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (ObjectRepresenterBase<T>)Activator.CreateInstance(
                        typeof(KeyValuePairRepresenter<,>).MakeGenericType(keyValueTypes));
                }
                else
                {
                    return new ObjectToStringRepresenter<T>();
                }
            }
            else
            {
                return new ObjectToStringRepresenter<T>();
            }
        }
    }
}
