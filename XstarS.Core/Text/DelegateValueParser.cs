using System;

namespace XstarS.Text
{
    /// <summary>
    /// 表示以 <see cref="Converter{TInput, TOutput}"/> 委托定义的字符串解析对象。
    /// </summary>
    /// <typeparam name="T">要转换为的数值的类型。</typeparam>
    [Serializable]
    internal sealed class DelegateValueParser<T> : ValueParser<T>
    {
        /// <summary>
        /// 表示将字符串解析为指定类型的数值的方法的委托。
        /// </summary>
        private readonly Converter<string, T> Delegate;

        /// <summary>
        /// 以指定的委托初始化 <see cref="DelegateValueParser{T}"/> 类的新实例。
        /// </summary>
        /// <param name="parser">用于解析字符串为数值的方法的
        /// <see cref="Converter{TInput, TOutput}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parser"/> 为 <see langword="null"/>。</exception>
        public DelegateValueParser(Converter<string, T> parser)
        {
            this.Delegate = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的数值形式。
        /// </summary>
        /// <param name="text">包含要转换的数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public override T Parse(string text) => this.Delegate.Invoke(text);
    }
}
