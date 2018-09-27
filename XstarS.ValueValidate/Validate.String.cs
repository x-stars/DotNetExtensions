using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XstarS
{
    public static partial class Validate
    {
        /// <summary>
        /// 验证字符串是否不是空字符串 <see cref="string.Empty"/>。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值等于 <see cref="string.Empty"/>。
        /// </exception>
        public static IValidate<string> IsNotEmpty(this IValidate<string> source,
            string message = null)
        {
            if (source.Value.Length == 0)
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否不是由空白字符 (参见 <see cref="char.IsWhiteSpace(char)"/>) 组成。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值由空白字符组成。
        /// </exception>
        public static IValidate<string> IsNotWhiteSpace(this IValidate<string> source,
            string message = null)
        {
            if (source.Value.All(c => char.IsWhiteSpace(c)))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否以指定字符串开始。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">应作为开始的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不以 <paramref name="value"/> 开始。
        /// </exception>
        public static IValidate<string> StartsWith(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (!source.Value.StartsWith(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否不以指定字符串开始。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">不应作为开始的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值以 <paramref name="value"/> 开始。
        /// </exception>
        public static IValidate<string> NotStartsWith(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (!source.Value.StartsWith(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否以指定字符串结束。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">应作为结束的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不以 <paramref name="value"/> 结束。
        /// </exception>
        public static IValidate<string> EndsWith(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (!source.Value.EndsWith(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否不以指定字符串结束。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">不应作为结束的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值以 <paramref name="value"/> 结束。
        /// </exception>
        public static IValidate<string> NotEndsWith(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (source.Value.EndsWith(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否包含指定字符串。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">应包含的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不包含 <paramref name="value"/>。
        /// </exception>
        public static IValidate<string> Contains(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (!source.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否不包含指定字符串。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">不应包含的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值包含 <paramref name="value"/>。
        /// </exception>
        public static IValidate<string> NotContains(this IValidate<string> source,
            string value,
            string message = null)
        {
            if (source.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否匹配指定的正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="regex">应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不能匹配 <paramref name="regex"/>。
        /// </exception>
        public static IValidate<string> IsMatch(this IValidate<string> source,
            Regex regex,
            string message = null)
        {
            if (!regex.IsMatch(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否匹配指定的正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="pattern">应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不能匹配 <paramref name="pattern"/>。
        /// </exception>
        public static IValidate<string> IsMatch(this IValidate<string> source,
            string pattern,
            string message = null)
        {
            return source.IsMatch(new Regex(pattern));
        }

        /// <summary>
        /// 验证字符串是否不匹配指定的正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="regex">不应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值能够匹配 <paramref name="regex"/>。
        /// </exception>
        public static IValidate<string> IsNotMatch(this IValidate<string> source,
            Regex regex,
            string message = null)
        {
            if (regex.IsMatch(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证字符串是否不匹配指定的正则表达式。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="pattern">不应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值能够匹配 <paramref name="pattern"/>。
        /// </exception>
        public static IValidate<string> IsNotMatch(this IValidate<string> source,
            string pattern,
            string message = null)
        {
            return source.IsNotMatch(new Regex(pattern));
        }
    }
}
