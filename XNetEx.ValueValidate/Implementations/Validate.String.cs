using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace XNetEx
{
    static partial class Validate
    {
        /// <summary>
        /// 验证字符串是否不是空字符串 <see cref="string.Empty"/>。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值等于 <see cref="string.Empty"/>。</exception>
        public static IValueInfo<string> IsNotNullOrEmpty(
            this IValueInfo<string> valueInfo, string? message = null)
        {
            if (valueInfo.Value.Length == 0)
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否不是由空白字符 (参见 <see cref="char.IsWhiteSpace(char)"/>) 组成。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值由空白字符组成。</exception>
        public static IValueInfo<string> IsNotWhiteSpace(
            this IValueInfo<string> valueInfo, string? message = null)
        {
            if (valueInfo.Value.All(char.IsWhiteSpace))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串的长度是否在指定范围内。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="minLength">字符串允许的最小长度。</param>
        /// <param name="maxLength">字符串允许的最大长度。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException"><paramref name="valueInfo"/> 的长度不在
        /// <paramref name="minLength"/> 和 <paramref name="maxLength"/> 之间。</exception>
        public static IValueInfo<string> LengthIsInRange(
            this IValueInfo<string> valueInfo, int minLength, int maxLength, string? message = null)
        {
            if ((valueInfo.Value.Length < minLength) || (valueInfo.Value.Length > maxLength))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否以指定字符串开始。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">应作为开始的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不以 <paramref name="value"/> 开始。</exception>
        public static IValueInfo<string> StartsWith(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (!valueInfo.Value.StartsWith(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否不以指定字符串开始。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">不应作为开始的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值以 <paramref name="value"/> 开始。</exception>
        public static IValueInfo<string> NotStartsWith(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (!valueInfo.Value.StartsWith(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否以指定字符串结束。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">应作为结束的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不以 <paramref name="value"/> 结束。</exception>
        public static IValueInfo<string> EndsWith(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (!valueInfo.Value.EndsWith(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否不以指定字符串结束。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">不应作为结束的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值以 <paramref name="value"/> 结束。</exception>
        public static IValueInfo<string> NotEndsWith(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (valueInfo.Value.EndsWith(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否包含指定字符串。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">应包含的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不包含 <paramref name="value"/>。</exception>
        public static IValueInfo<string> Contains(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (!valueInfo.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否不包含指定字符串。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">不应包含的字符串。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值包含 <paramref name="value"/>。</exception>
        public static IValueInfo<string> NotContains(
            this IValueInfo<string> valueInfo, string value, string? message = null)
        {
            if (valueInfo.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否匹配指定的正则表达式。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="regex">应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不能匹配 <paramref name="regex"/>。</exception>
        public static IValueInfo<string> IsMatch(
            this IValueInfo<string> valueInfo, Regex regex, string? message = null)
        {
            if (!regex.IsMatch(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否匹配指定的正则表达式。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="pattern">应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不能匹配 <paramref name="pattern"/>。</exception>
        public static IValueInfo<string> IsMatch(
            this IValueInfo<string> valueInfo, string pattern, string? message = null)
        {
            return valueInfo.IsMatch(new Regex(pattern), message);
        }

        /// <summary>
        /// 验证字符串是否不匹配指定的正则表达式。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="regex">不应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值能够匹配 <paramref name="regex"/>。</exception>
        public static IValueInfo<string> IsNotMatch(
            this IValueInfo<string> valueInfo, Regex regex, string? message = null)
        {
            if (regex.IsMatch(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证字符串是否不匹配指定的正则表达式。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="pattern">不应匹配的正则表达式。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值能够匹配 <paramref name="pattern"/>。</exception>
        public static IValueInfo<string> IsNotMatch(
            this IValueInfo<string> valueInfo, string pattern, string? message = null)
        {
            return valueInfo.IsNotMatch(new Regex(pattern), message);
        }
    }
}
