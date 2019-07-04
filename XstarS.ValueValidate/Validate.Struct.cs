using System;
using System.Collections.Generic;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证对象是否不是一个空值（无参构造的结构的实例）。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="valueInfo"/> 的值为空值（无参构造的结构的实例）。</exception>
        public static IValueInfo<T> IsNotEmpty<T>(
            this IValueInfo<T> valueInfo, string message = null)
            where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(valueInfo.Value, new T()))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }
    }
}
