using System;

namespace XNetEx.Text
{
    /// <summary>
    /// 提供将字符串转换为对象的方法。
    /// </summary>
    public interface IStringParser
    {
        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的对象。
        /// </summary>
        /// <param name="text">包含要转换的对象的字符串。</param>
        /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了要转换为的数值类型能表示的范围。</exception>
        object Parse(string text);
    }

    /// <summary>
    /// 提供字符串转换为指定类型的对象的方法。
    /// </summary>
    /// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
    public interface IStringParser<out T> where T : notnull
    {
        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的对象。
        /// </summary>
        /// <param name="text">包含要转换的对象的字符串。</param>
        /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        T Parse(string text);
    }
}
