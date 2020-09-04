using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using mstring = System.Text.StringBuilder;

namespace XstarS
{
    /// <summary>
    /// 提供字符串 <see cref="string"/> 的扩展方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指示当前字符串是否匹配指定正则表达式。
        /// </summary>
        /// <param name="text">要进行匹配的字符串。</param>
        /// <param name="pattern">要进行匹配的正则表达式模式。</param>
        /// <returns>若 <paramref name="text"/> 匹配 <paramref name="pattern"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pattern"/> 的正则表达式分析错误。</exception>
        public static bool IsMatch(this string text, string pattern) =>
            new Regex(pattern).IsMatch(text);

        /// <summary>
        /// 返回一个新字符串，此字符串将当前字符串重复指定次数。
        /// </summary>
        /// <param name="text">要重复的字符串。</param>
        /// <param name="count">字符串重复的次数。</param>
        /// <returns>将 <paramref name="text"/>
        /// 重复 <paramref name="count"/> 次得到的新字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> 小于 0。</exception>
        public static string Repeat(this string text, int count) =>
            new string('*', count).Replace("*", text);

        /// <summary>
        /// 返回一个新字符串，此字符串中所有匹配指定正则表达式模式的子字符串都被替换为指定值。
        /// </summary>
        /// <param name="text">要进行替换的字符串。</param>
        /// <param name="pattern">指定要替换的正则表达式模式。</param>
        /// <param name="value">指定用于替换的新字符串。</param>
        /// <returns><paramref name="text"/> 中所有匹配 <paramref name="pattern"/>
        /// 的子字符串被替换为 <paramref name="value"/> 得到的新字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pattern"/> 的正则表达式分析错误。</exception>
        public static string ReplaceMatches(this string text, string pattern, string value)
        {
            var matches = new Regex(pattern).Matches(text);
            foreach (Match match in matches)
            {
                text = text.Replace(match.Value, value);
            }
            return text;
        }

        /// <summary>
        /// 使用满足指定条件的所有字符为分隔符将字符串拆分为多个子字符串。
        /// </summary>
        /// <param name="text">要进行拆分的字符串。</param>
        /// <param name="isSeparator">判断字符是否为分隔字符的 <see cref="Predicate{T}"/> 委托。</param>
        /// <param name="options">指定是否要忽略分隔得到的空子字符串。</param>
        /// <returns><paramref name="text"/> 根据满足 <paramref name="isSeparator"/> 条件的字符分隔得到的子字符串，
        /// 并根据 <paramref name="options"/> 的指示去除空字符串。</returns>
        public static IEnumerable<string> Split(this string text,
            Predicate<char> isSeparator, StringSplitOptions options = StringSplitOptions.None)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }
            if (isSeparator is null)
            {
                throw new ArgumentNullException(nameof(isSeparator));
            }

            var token = new mstring();
            foreach (var c in text)
            {
                if (isSeparator(c))
                {
                    if ((token.Length > 0) ||
                        (options == StringSplitOptions.None))
                    {
                        yield return token.ToString();
                        token.Clear();
                    }
                }
                else
                {
                    token.Append(c);
                }
            }
            if ((token.Length > 0) ||
                (options == StringSplitOptions.None))
            {
                yield return token.ToString();
                token.Clear();
            }
        }
    }
}
