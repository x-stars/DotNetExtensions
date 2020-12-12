using System;
using System.Collections.Concurrent;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IObjectRepresenter{T}"/> 的抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    public abstract class ObjectRepresenter<T> : IObjectRepresenter, IObjectRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ObjectRepresenter{T}"/> 类的新实例。
        /// </summary>
        protected ObjectRepresenter() { }

        /// <summary>
        /// 获取默认的 <see cref="ObjectRepresenter{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="ObjectRepresenter{T}"/> 类的实例。</returns>
        public static ObjectRepresenter<T> Default { get; } = new DefaultObjectRepresenter<T>();

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public abstract string Represent(T value);

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IObjectRepresenter.Represent(object value) => this.Represent((T)value);
    }

    /// <summary>
    /// 提供将对象表示为字符串的方法。
    /// </summary>
    public static class ObjectRepresenter
    {
        /// <summary>
        /// 表示指定类型的 <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。
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
        /// 获取指定类型的 <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        internal static IAcyclicObjectRepresenter OfType(Type type)
        {
            return (type is null) ? ObjectRepresenterBase<object>.Default :
                ObjectRepresenter.Defaults.GetOrAdd(type, ObjectRepresenter.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        private static IAcyclicObjectRepresenter GetDefault(Type type)
        {
            var typeRepresenter = typeof(ObjectRepresenterBase<>).MakeGenericType(type);
            var nameDefualt = nameof(ObjectRepresenterBase<object>.Default);
            var propertyDefault = typeRepresenter.GetProperty(nameDefualt);
            return (IAcyclicObjectRepresenter)propertyDefault.GetValue(null);
        }
    }
}
