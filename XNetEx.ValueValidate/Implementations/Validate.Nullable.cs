using System;

namespace XNetEx
{
    static partial class Validate
    {
        /// <summary>
        /// 验证对象是否不为 <see langword="null"/>。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueInfo"/> 的值为 <see langword="null"/>。</exception>
        public static IValueInfo<T?> IsNotNull<T>(
            this IValueInfo<T?> valueInfo, string? message = null)
            where T : struct
        {
            if (valueInfo.Value is null)
            {
                ThrowHelper.ThrowArgumentNullException(valueInfo.Name, message);
            }

            return valueInfo;
        }
    }
}
