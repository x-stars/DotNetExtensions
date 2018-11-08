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
        public static string Repeat(this string source, int count) =>
            new string('*', count).Replace("*", source);

        /// <summary>
        /// 指示当前字符串是否匹配指定正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="pattern">要进行匹配的正则表达式模式。</param>
        /// <returns>若 <paramref name="source"/> 匹配 <paramref name="source"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool Matches(this string source, string pattern) =>
            new Regex(pattern).IsMatch(pattern);

        /// <summary>
        /// 返回一个新字符串，此字符串中所有匹配指定正则表达式模式的子字符串都被替换为指定值，
        /// </summary>
        /// <param name="source">一个 <see cref="string"/> 类型的对象。</param>
        /// <param name="pattern">指定要替换的正则表达式模式。</param>
        /// <param name="value">指定用于替换的新字符串。</param>
        /// <returns><paramref name="source"/> 中所有匹配 <paramref name="pattern"/>
        /// 的子字符串被替换为 <paramref name="value"/> 得到的新字符串。</returns>
        public static string ReplaceMatches(this string source, string pattern, string value)
        {
            var matches = new Regex(pattern).Matches(source);
            foreach (Match match in matches)
            {
                source = source.Replace(match.Value, value);
            }
            return source;
        }
    }
}
