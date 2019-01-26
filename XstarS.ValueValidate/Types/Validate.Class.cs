using System;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证对象是否不为 <see langword="null"/>。
        /// </summary>
        /// <typeparam name="T">待验证的参数的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 的值为 <see langword="null"/>。</exception>
        public static IValidate<T> IsNotNull<T>(this IValidate<T> source,
            string message = null)
            where T : class
        {
            if (source.Value is null)
            {
                ThrowHelper.ThrowArgumentNullException(source.Name, message);
            }

            return source;
        }
    }
}
