using System;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 提供字符串 <see cref="string"/> 分隔值相关的扩展方法。
    /// </summary>
    public static class StringTokenizeExtensions
    {
        /// <summary>
        /// 表示所有空白字符的集合。
        /// </summary>
        private static readonly char[] WhiteSpaces =
            Enumerable.Range(0, char.MaxValue + 1).Select(
                Convert.ToChar).Where(char.IsWhiteSpace).ToArray();

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

            return text.Split(StringTokenizeExtensions.WhiteSpaces,
                StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
