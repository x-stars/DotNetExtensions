using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using XstarS.Collections;
using XstarS.Collections.Generic;
using XstarS.Collections.Specialized;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IObjectRepresenter{T}"/> 的抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    public abstract class ObjectRepresenter<T>
        : IAcyclicObjectRepresenter, IAcyclicObjectRepresenter<T>
    {
        /// <summary>
        /// 表示已经在路径中表示过的对象的字符串表示。
        /// </summary>
        private const string RepresentedString = "{ ... }";

        /// <summary>
        /// 表示 <see cref="ObjectRepresenter{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ObjectRepresenter<T>> LazyDefault =
            new Lazy<ObjectRepresenter<T>>(ObjectRepresenter<T>.CreateDefault);

        /// <summary>
        /// 获取 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ObjectRepresenter{T}"/> 类的默认实例。</returns>
        public static ObjectRepresenter<T> Default => ObjectRepresenter<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ObjectRepresenter{T}"/> 类的默认实例。</returns>
        private static ObjectRepresenter<T> CreateDefault()
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
                    return (ObjectRepresenter<T>)Activator.CreateInstance(
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
                return (ObjectRepresenter<T>)(object)new DictionaryEntryRepresenter();
            }
            else if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>))
                {
                    var keyValueTypes = type.GetGenericArguments();
                    return (ObjectRepresenter<T>)Activator.CreateInstance(
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

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public string Represent(T value)
        {
            var comparer = ReferenceEqualityComparer.Default;
            var represented = new HashSet<object>(comparer);
            return this.Represent(value, represented);
        }

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected string Represent(T value, ISet<object> represented)
        {
            if ((object)value is null) { return null; }

            if (!represented.Add(value))
            {
                return ObjectRepresenter<T>.RepresentedString;
            }

            var represent = this.RepresentCore(value, represented);
            represented.Remove(value);
            return represent;
        }

        /// <summary>
        /// 在派生类中重写，将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected abstract string RepresentCore(T value, ISet<object> represented);

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string IAcyclicObjectRepresenter<T>.Represent(T value, ISet<object> represented)
        {
            return this.Represent(value, represented);
        }

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IObjectRepresenter.Represent(object value)
        {
            return this.Represent((T)value);
        }

        /// <summary>
        /// 将访指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IAcyclicObjectRepresenter.Represent(object value, ISet<object> represented)
        {
            return this.Represent((T)value, represented);
        }
    }

    /// <summary>
    /// 提供将对象表示为字符串的方法。
    /// </summary>
    public static class ObjectRepresenter
    {
        /// <summary>
        /// 表示指定类型的 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IAcyclicObjectRepresenter> Defaults =
            new ConcurrentDictionary<Type, IAcyclicObjectRepresenter>();

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public static string Represent(object value)
        {
            return ObjectRepresenter.OfType(value?.GetType()).Represent(value);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenter{T}"/> 类的默认实例。</returns>
        internal static IAcyclicObjectRepresenter OfType(Type type)
        {
            return (type is null) ? ObjectRepresenter<object>.Default :
                ObjectRepresenter.Defaults.GetOrAdd(type, ObjectRepresenter.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenter{T}"/> 类的默认实例。</returns>
        private static IAcyclicObjectRepresenter GetDefault(Type type)
        {
            var typeRepresenter = typeof(ObjectRepresenter<>).MakeGenericType(type);
            var nameDefualt = nameof(ObjectRepresenter<object>.Default);
            var propertyDefault = typeRepresenter.GetProperty(nameDefualt);
            return (IAcyclicObjectRepresenter)propertyDefault.GetValue(null);
        }
    }
}
