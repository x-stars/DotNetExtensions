using System;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 为 <see cref="IObjectRepresenter{T}"/> 提供抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ObjectRepresenter<T> : IObjectRepresenter, IObjectRepresenter<T>
    {
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
        public abstract string Format(T value);

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IObjectRepresenter.Format(object value) => this.Format((T)value);
    }
}
