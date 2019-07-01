using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证枚举值是否是一个有效的枚举值。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="valueInfo"/> 的值不为有效的枚举值。</exception>
        public static IValueInfo<T> IsValidEnum<T>(
            this IValueInfo<T> valueInfo,
            string message = null)
            where T : struct
        {
            if (typeof(T).GetCustomAttributes(false).Any(attr => attr is FlagsAttribute))
            {
                var values = Enum.GetValues(typeof(T));
                long allValues = 0L;
                foreach (var value in values)
                {
                    allValues |= Convert.ToInt64(value);
                }
                if ((allValues | Convert.ToInt64(valueInfo.Value)) != allValues)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(valueInfo.Name, valueInfo.Value, message);
                }
            }
            else
            {
                if (!Enum.GetValues(typeof(T)).Cast<T>().Contains(valueInfo.Value))
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(valueInfo.Name, valueInfo.Value, message);
                }
            }

            return valueInfo;
        }
    }
}
