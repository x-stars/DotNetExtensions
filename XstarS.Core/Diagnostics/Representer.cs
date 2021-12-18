using System;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 为字符串表示对象 <see cref="IRepresenter{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    public abstract class Representer<T> : IRepresenter, IRepresenter<T>
    {
        /// <summary>
        /// 表示空引用 <see langword="null"/> 的字符串表示形式。
        /// </summary>
        protected const string NullRefString = "(NullRef)";

        /// <summary>
        /// 表示 <see cref="Representer{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<Representer<T>> LazyDefault =
            new Lazy<Representer<T>>(Representer<T>.CreateDefault);

        /// <summary>
        /// 初始化 <see cref="Representer{T}"/> 类的新实例。
        /// </summary>
        protected Representer() { }

        /// <summary>
        /// 获取默认的 <see cref="Representer{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="Representer{T}"/> 类的实例。</returns>
        public static Representer<T> Default => Representer<T>.LazyDefault.Value;

        /// <summary>
        /// 创建 <see cref="Representer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="Representer{T}"/> 类的默认实例。</returns>
        private static Representer<T> CreateDefault()
        {
            var type = typeof(T);
            if (type.IsPrimitive)
            {
                return (Representer<T>)Activator.CreateInstance(
                    typeof(PrimitiveRepresenter<>).MakeGenericType(type));
            }
            else if (type == typeof(string))
            {
                return (Representer<T>)(object)new StringRepresenter();
            }
            else if (DebuggerDisplayRepresenter<T>.HasDebuggerDisplay)
            {
                return new DebuggerDisplayRepresenter<T>();
            }
            else
            {
                return new ToStringRepresenter<T>();
            }
        }

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
        string IRepresenter.Represent(object value) =>
            (value is null) ? Representer<T>.NullRefString : this.Represent((T)value);
    }
}
