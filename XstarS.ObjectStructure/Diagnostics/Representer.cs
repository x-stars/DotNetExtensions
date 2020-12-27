using System;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IRepresenter{T}"/> 的抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    public abstract class Representer<T> : IRepresenter, IRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="Representer{T}"/> 类的新实例。
        /// </summary>
        protected Representer() { }

        /// <summary>
        /// 获取默认的 <see cref="Representer{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="Representer{T}"/> 类的实例。</returns>
        public static Representer<T> Default { get; } = Representer<T>.CreateDefault();

        /// <summary>
        /// 创建 <see cref="Representer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="Representer{T}"/> 类的默认实例。</returns>
        private static Representer<T> CreateDefault()
        {
            if (typeof(IRepresentable).IsAssignableFrom(typeof(T)))
            {
                return (Representer<T>)Activator.CreateInstance(
                    typeof(RepresentableRepresenter<>).MakeGenericType(typeof(T)));
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
        string IRepresenter.Represent(object value) => this.Represent((T)value);
    }
}
