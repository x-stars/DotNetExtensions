using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using XstarS.Collections.Specialized;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IObjectRepresenter{T}"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObjectRepresenter<T>
        : IAcyclicObjectRepresenter, IAcyclicObjectRepresenter<T>
    {
        /// <summary>
        /// 表示已经在路径上访问过的对象的字符串表示。
        /// </summary>
        private const string PathedRepresent = "{ ... }";

        /// <summary>
        /// 获取默认的 <see cref="ObjectRepresenter{T}"/> 实例。
        /// </summary>
        /// <returns>默认的 <see cref="ObjectRepresenter{T}"/> 实例。</returns>
        public static ObjectRepresenter<T> Default { get; } = new ObjectToStringRepresenter<T>();

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public string Represent(T value)
        {
            var comparer = ReferenceEqualityComparer.Default;
            var pathed = new HashSet<object>(comparer);
            return this.Represent(value, pathed);
        }

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected string Represent(T value, ISet<object> pathed)
        {
            if (!pathed.Add(value))
            {
                return ObjectRepresenter<T>.PathedRepresent;
            }

            var represent = this.RepresentCore(value, pathed);
            pathed.Remove(value);
            return represent;
        }

        /// <summary>
        /// 在派生类中重写，将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected abstract string RepresentCore(T value, ISet<object> pathed);

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IObjectRepresenter.Represent(object value) => this.Represent((T)value);

        /// <summary>
        /// 将访指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IAcyclicObjectRepresenter.Represent(object value, ISet<object> pathed) =>
            this.Represent((T)value, pathed);

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string IAcyclicObjectRepresenter<T>.Represent(T value, ISet<object> pathed) =>
            this.Represent(value, pathed);
    }

    /// <summary>
    /// 提供获取 <see cref="ObjectRepresenter{T}"/> 类的默认实例的方法。
    /// </summary>
    internal static class ObjectRepresenter
    {
        /// <summary>
        /// 表示指定类型的 <see cref="ObjectRepresenter{T}"/> 的默认实例。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, IAcyclicObjectRepresenter> Defaults =
            new ConcurrentDictionary<Type, IAcyclicObjectRepresenter>();

        /// <summary>
        /// 获取指定类型的 <see cref="ObjectRepresenter{T}"/> 的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenter{T}"/> 的默认实例。</returns>
        internal static IAcyclicObjectRepresenter OfType(Type type)
        {
            return ObjectRepresenter.Defaults.GetOrAdd(type, ObjectRepresenter.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="ObjectRepresenter{T}"/> 的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="ObjectRepresenter{T}"/> 的默认实例。</returns>
        private static IAcyclicObjectRepresenter GetDefault(Type type)
        {
            var typeRepresenter = typeof(ObjectRepresenter<>).MakeGenericType(type);
            var nameDefualt = nameof(ObjectRepresenter<object>.Default);
            var propertyDefault = typeRepresenter.GetProperty(nameDefualt);
            return (IAcyclicObjectRepresenter)propertyDefault.GetValue(null);
        }
    }
}
