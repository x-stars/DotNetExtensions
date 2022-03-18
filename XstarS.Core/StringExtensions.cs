using System;
using System.Collections.Generic;
using System.Linq;
using mstring = System.Text.StringBuilder;

namespace XstarS
{
    /// <summary>
    /// 提供字符串 <see cref="string"/> 的扩展方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 提供字符串分隔符的集合。
        /// </summary>
        private static class Separators
        {
            /// <summary>
            /// 表示所有行结束符的字符串的集合。
            /// </summary>
            internal static readonly string[] NewLines = new[] { "\r\n", "\r", "\n" };

            /// <summary>
            /// 表示所有空白字符的集合。
            /// </summary>
            internal static readonly char[] WhiteSpaces =
                Enumerable.Range(0, char.MaxValue + 1).Select(
                    Convert.ToChar).Where(char.IsWhiteSpace).ToArray();
        }

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
            new string(new char[count]).Replace("\0", text);

        /// <summary>
        /// 返回一个新字符串，此字符串的顺序与当前字符串的顺序相反。
        /// </summary>
        /// <param name="text">要反转顺序的字符串。</param>
        /// <returns>将 <paramref name="text"/> 反转顺序得到的新字符串。</returns>
        public static string? Reverse(this string? text)
        {
            if (text is null) { return null; }
            var chars = text.ToCharArray();
            Array.Reverse(chars);
            return new string(chars);
        }

        /// <summary>
        /// 使用满足指定条件的所有字符为分隔符将字符串拆分为多个子字符串。
        /// </summary>
        /// <param name="text">要进行拆分的字符串。</param>
        /// <param name="isSeparator">判断字符是否为分隔字符的 <see cref="Predicate{T}"/> 委托。</param>
        /// <param name="options">指定是否要忽略分隔得到的空子字符串。</param>
        /// <returns><paramref name="text"/> 根据满足
        /// <paramref name="isSeparator"/> 条件的字符分隔得到的子字符串，
        /// 并根据 <paramref name="options"/> 的指示去除空字符串。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        public static IEnumerable<string> Split(
            this string text, Predicate<char> isSeparator,
            StringSplitOptions options = StringSplitOptions.None)
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
            foreach (var @char in text)
            {
                if (isSeparator(@char))
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
                    token.Append(@char);
                }
            }
            if ((token.Length > 0) ||
                (options == StringSplitOptions.None))
            {
                yield return token.ToString();
                token.Clear();
            }
        }

        /// <summary>
        /// 将当前字符串按行分隔为多个子字符串。
        /// </summary>
        /// <param name="text">要进行分隔的字符串。</param>
        /// <returns>按行分隔得到子字符串的数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        public static string[] SplitLines(this string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var lines = text.Split(Separators.NewLines, StringSplitOptions.None);
            if (lines[^1].Length == 0)
            {
                var result = new string[lines.Length - 1];
                Array.Copy(lines, result, lines.Length - 1);
                lines = result;
            }
            return lines;
        }

        /// <summary>
        /// 将当前字符串按空白字符分隔为多个子字符串。
        /// </summary>
        /// <param name="text">要进行分隔的字符串。</param>
        /// <returns>按空白字符分隔得到子字符串的数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        public static string[] SplitTokens(this string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return text.Split(Separators.WhiteSpaces, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
