using System;
using System.Collections.Concurrent;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将结构化对象表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">结构化对象的类型。</typeparam>
    [Serializable]
    public class StructuralObjectRepresenter<T> : ObjectRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="StructuralObjectRepresenter{T}"/> 类型的新实例。
        /// </summary>
        public StructuralObjectRepresenter() { }

        /// <summary>
        /// 获取 <see cref="StructuralObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StructuralObjectRepresenter{T}"/> 类的默认实例。</returns>
        public static new StructuralObjectRepresenter<T> Default { get; } =
            new StructuralObjectRepresenter<T>();

        /// <summary>
        /// 将指定结构化对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的结构化对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public override string Represent(T value) => StructuralObjectRepresenter.Represent(value);
    }

    /// <summary>
    /// 提供将结构化对象表示为字符串的方法。
    /// </summary>
    public static class StructuralObjectRepresenter
    {
        /// <summary>
        /// 表示指定类型的 <see cref="StructuralObjectRepresenterBase{T}"/> 类的默认实例。
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
            return StructuralObjectRepresenter.OfType(value?.GetType()).Represent(value);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="StructuralObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructuralObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        internal static IAcyclicObjectRepresenter OfType(Type type)
        {
            return (type is null) ? StructuralObjectRepresenterBase<object>.Default :
                StructuralObjectRepresenter.Defaults.GetOrAdd(type, StructuralObjectRepresenter.GetDefault);
        }

        /// <summary>
        /// 获取指定类型的 <see cref="StructuralObjectRepresenterBase{T}"/> 类的默认实例。
        /// </summary>
        /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
        /// <returns>类型参数为 <paramref name="type"/> 的
        /// <see cref="StructuralObjectRepresenterBase{T}"/> 类的默认实例。</returns>
        private static IAcyclicObjectRepresenter GetDefault(Type type)
        {
            var typeRepresenter = typeof(StructuralObjectRepresenterBase<>).MakeGenericType(type);
            var nameDefualt = nameof(StructuralObjectRepresenterBase<object>.Default);
            var propertyDefault = typeRepresenter.GetProperty(nameDefualt);
            return (IAcyclicObjectRepresenter)propertyDefault.GetValue(null);
        }
    }
}
