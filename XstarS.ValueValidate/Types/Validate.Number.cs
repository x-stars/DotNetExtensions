using System;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证整数是否在索引的有效范围内。
        /// </summary>
        /// <param name="source">一个 <see cref="IValidate{T}"/> 对象。</param>
        /// <param name="startIndex">索引的开始值。</param>
        /// <param name="endIndex">索引的结束值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="source"/> 的值不在
        /// <paramref name="startIndex"/> 和 <paramref name="endIndex"/> 之间。</exception>
        public static IValidate<int> IsInIndexRange(this IValidate<int> source,
            int startIndex, int endIndex,
            string message = null)
        {
            if ((source.Value < startIndex) || (source.Value > endIndex))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(source.Name, source.Value, message);
            }

            return source;
        }

        /// <summary>
        /// 验证整数是否在集合索引的有效范围内。
        /// </summary>
        /// <param name="source">一个 <see cref="IValidate{T}"/> 对象。</param>
        /// <param name="count">集合的大小。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="source"/>
        /// 的值不在 0 和 <paramref name="count"/> - 1 之间。</exception>
        public static IValidate<int> IsInIndexRange(this IValidate<int> source,
            int count,
            string message = null)
        {
            return source.IsInIndexRange(0, count - 1, message);
        }
    }
}
