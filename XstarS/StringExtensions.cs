using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XstarS
{
    /// <summary>
    /// 提供字符串 <see cref="string"/> 的扩展方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 返回一个新字符串，此字符串将当前字符串重复指定次数。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="count">字符串重复的次数。</param>
        /// <returns>将 <paramref name="source"/>
        /// 重复 <paramref name="count"/> 次得到的新字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/> 小于 0。</exception>
        public static string Repeat(this string source, int count) =>
            new string('*', count).Replace("*", source);

        /// <summary>
        /// 指示当前字符串是否匹配指定正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="pattern">要进行匹配的正则表达式模式。</param>
        /// <returns>若 <paramref name="source"/> 匹配 <paramref name="pattern"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pattern"/> 的正则表达式分析错误。</exception>
        public static bool Matches(this string source, string pattern) =>
            new Regex(pattern).IsMatch(source);

        /// <summary>
        /// 返回一个新字符串，此字符串中所有匹配指定正则表达式模式的子字符串都被替换为指定值。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="pattern">指定要替换的正则表达式模式。</param>
        /// <param name="value">指定用于替换的新字符串。</param>
        /// <returns><paramref name="source"/> 中所有匹配 <paramref name="pattern"/>
        /// 的子字符串被替换为 <paramref name="value"/> 得到的新字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pattern"/> 的正则表达式分析错误。</exception>
        public static string ReplaceMatches(this string source, string pattern, string value)
        {
            var matches = new Regex(pattern).Matches(source);
            foreach (Match match in matches)
            {
                source = source.Replace(match.Value, value);
            }
            return source;
        }

        /// <summary>
        /// 枚举当前字符串的所有长度小于等于指定值的子字符串。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="maxLength">指定最大子字符串长度，负数表示无限制。</param>
        /// <returns><paramref name="source"/> 中所有长度小于等于
        /// <paramref name="maxLength"/> 的子字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<string> EnumerateSubstrings(this string source, int maxLength = -1)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            maxLength = (maxLength < 0) ? source.Length : maxLength;

            yield return string.Empty;
            if (maxLength == 0) { yield break; }
            for (int length = 1; length <= maxLength; length++)
            {
                for (int index = 0; index < source.Length; index++)
                {
                    if (length + index <= source.Length)
                    {
                        yield return source.Substring(index, length);
                    }
                }
            }
        }

        /// <summary>
        /// 枚举当前字符串的所有长度等于指定值的子字符串。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="length">指定子字符串长度。</param>
        /// <returns><paramref name="source"/> 中所有长度等于
        /// <paramref name="length"/> 的子字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="length"/> 小于 0。</exception>
        public static IEnumerable<string> EnumerateSubstringsLength(this string source, int length)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0)
            {
                yield return string.Empty;
                yield break;
            }
            for (int index = 0; index < source.Length; index++)
            {
                if (index + length <= source.Length)
                {
                    yield return source.Substring(index, length);
                }
            }
        }
    }
}
