using System;
using System.Text.RegularExpressions;

namespace XstarS.Text.RegularExpressions
{
    /// <summary>
    /// 提供字符串 <see cref="string"/> 基于正则表达式 <see cref="Regex"/> 的扩展方法。
    /// </summary>
    public static class StringRegexExtensions
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
        public static string ReplaceMatches(this string text, string pattern, string value) =>
            new Regex(pattern).Replace(text, value);
    }
}
