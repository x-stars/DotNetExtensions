using System;

namespace XstarS.Text
{
    /// <summary>
    /// 表示使用 <see cref="Enum.Parse(Type, string)"/> 方法将字符串转换为枚举的字符串解析对象。
    /// </summary>
    /// <typeparam name="T">要转换为的枚举的类型。</typeparam>
    [Serializable]
    internal sealed class EnumStringParser<T> : SimpleStringParser<T> where T : notnull
    {
        /// <summary>
        /// 初始化 <see cref="EnumStringParser{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="T"/> 不为枚举类型。</exception>
        public EnumStringParser()
        {
            if (!typeof(T).IsEnum)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的枚举对象。
        /// </summary>
        /// <param name="text">包含要转换的枚举的字符串。</param>
        /// <returns>与 <paramref name="text"/> 中的内容等效的枚举对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public override T Parse(string text) => (T)Enum.Parse(typeof(T), text);
    }
}
