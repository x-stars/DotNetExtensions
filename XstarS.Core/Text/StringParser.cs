﻿using System;

namespace XstarS.Text
{
    /// <summary>
    /// 为字符串解析对象 <see cref="IStringParser{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要解析为的数值的类型。</typeparam>
    [Serializable]
    public abstract class StringParser<T> : IStringParser, IStringParser<T>
    {
        /// <summary>
        /// 初始化 <see cref="StringParser{T}"/> 类的新实例。
        /// </summary>
        protected StringParser() { }

        /// <summary>
        /// 获取 <see cref="StringParser{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StringParser{T}"/> 类的默认实例。</returns>
        public static StringParser<T> Default { get; } = StringParser<T>.CreateDefault();

        /// <summary>
        /// 创建以指定的委托定义的 <see cref="StringParser{T}"/> 类的实例。
        /// </summary>
        /// <param name="parser">用于解析字符串为数值的方法的
        /// <see cref="Converter{TInput, TOutput}"/> 委托。</param>
        /// <returns>以委托定义的指定类型的 <see cref="StringParser{T}"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parser"/> 为 <see langword="null"/>。</exception>
        public static StringParser<T> Create(Converter<string, T> parser)
        {
            return new DelegateStringParser<T>(parser);
        }

        /// <summary>
        /// 创建 <see cref="StringParser{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="StringParser{T}"/> 类的默认实例。</returns>
        private static StringParser<T> CreateDefault()
        {
            if (typeof(T) == typeof(string))
            {
                return (StringParser<T>)(object)new DefaultStringParser();
            }
            if (typeof(T) == typeof(Type))
            {
                return (StringParser<T>)(object)new TypeStringParser();
            }
            else if (typeof(T).IsEnum)
            {
                return new EnumStringParser<T>();
            }
            else if (ParsableStringParser<T>.CanParse)
            {
                return new ParsableStringParser<T>();
            }
            else
            {
                return new InvalidStringParser<T>();
            }
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
        public abstract T Parse(string text);

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
        object IStringParser.Parse(string text) => this.Parse(text);
    }

    /// <summary>
    /// 提供将字符串解析为数值的方法。
    /// </summary>
    public static class StringParser
    {
        /// <summary>
        /// 创建以指定的委托定义的指定类型的 <see cref="StringParser{T}"/> 类的实例。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="parser">用于解析字符串为数值的方法的
        /// <see cref="Converter{TInput, TOutput}"/> 委托。</param>
        /// <returns>以委托定义的指定类型的 <see cref="StringParser{T}"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="parser"/> 为 <see langword="null"/>。</exception>
        public static StringParser<T> Create<T>(Converter<string, T> parser)
        {
            return StringParser<T>.Create(parser);
        }

        /// <summary>
        /// 将当前字符串表示形式转换为其等效的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型，
        /// 应为枚举类型或包含类似于 <see cref="int.Parse(string)"/> 的方法。</typeparam>
        /// <param name="text">包含要转换的数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public static T ParseAs<T>(this string text)
        {
            return StringParser<T>.Default.Parse(text);
        }
    }
}
