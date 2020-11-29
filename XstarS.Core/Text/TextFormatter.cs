using System;

namespace XstarS.Text
{
    /// <summary>
    /// 为 <see cref="ITextFormatter{T}"/> 提供抽象基类。
    /// </summary>
    [Serializable]
    public abstract class TextFormatter<T> : ITextFormatter, ITextFormatter<T>
    {
        /// <summary>
        /// 将指定对象格式化为字符串。
        /// </summary>
        /// <param name="value">要格式化的对象。</param>
        /// <returns><paramref name="value"/> 的格式化字符串。</returns>
        public abstract string Format(T value);

        /// <summary>
        /// 将指定对象格式化为字符串。
        /// </summary>
        /// <param name="value">要格式化的对象。</param>
        /// <returns><paramref name="value"/> 的格式化字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="value"/> 无法转换强到 <typeparamref name="T"/> 类型。</exception>
        string ITextFormatter.Format(object value) => this.Format((T)value);
    }
}
