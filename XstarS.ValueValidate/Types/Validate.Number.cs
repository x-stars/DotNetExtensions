using System;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证整数是否在索引的有效范围内。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="startIndex">索引的开始值。</param>
        /// <param name="endIndex">索引的结束值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="valueInfo"/> 的值不在
        /// <paramref name="startIndex"/> 和 <paramref name="endIndex"/> 之间。</exception>
        public static IValueInfo<int> IsInIndexRange(
            this IValueInfo<int> valueInfo,
            int startIndex, int endIndex,
            string message = null)
        {
            if ((valueInfo.Value < startIndex) || (valueInfo.Value > endIndex))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(
                    valueInfo.Name, valueInfo.Value, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证整数是否在集合索引的有效范围内。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="count">集合的大小。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="IndexOutOfRangeException"><paramref name="valueInfo"/>
        /// 的值不在 0 和 <paramref name="count"/> - 1 之间。</exception>
        public static IValueInfo<int> IsInIndexRange(
            this IValueInfo<int> valueInfo,
            int count,
            string message = null)
        {
            return valueInfo.IsInIndexRange(0, count - 1, message);
        }
    }
}
