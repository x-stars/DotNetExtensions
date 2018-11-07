using System;
using System.Collections.Generic;

namespace XstarS
{
    static partial class Validate
    {
        /// <summary>
        /// 验证对象是否不是一个空值（无参构造的结构的实例）。
        /// </summary>
        /// <typeparam name="T">待验证的参数的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> 的值为空值（无参构造的结构的实例）。
        /// </exception>
        public static IValidate<T> IsNotEmpty<T>(this IValidate<T> source,
            string message = null)
            where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(source.Value, new T()))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }
    }
}
