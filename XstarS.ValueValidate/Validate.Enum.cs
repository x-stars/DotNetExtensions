using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS
{
    public static partial class Validate
    {
        /// <summary>
        /// 验证枚举值是否是一个有效的枚举值。
        /// </summary>
        /// <typeparam name="T">待验证的参数的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> 的值不为有效的枚举值。
        /// </exception>
        public static IValidate<T> IsValidEnum<T>(this IValidate<T> source,
            string message = null)
            where T : Enum
        {
            if (typeof(T).GetCustomAttributes(false).Any(attr => attr is FlagsAttribute))
            {
                var values = Enum.GetValues(typeof(T));
                long allValues = 0L;
                foreach (var value in values)
                {
                    allValues |= Convert.ToInt64(value);
                }
                if ((allValues | Convert.ToInt64(source.Value)) != allValues)
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(source.Name, source.Value, message);
                }
            }
            else
            {
                if (!Enum.GetValues(typeof(T)).Cast<T>().Contains(source.Value))
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(source.Name, source.Value, message);
                }
            }

            return source;
        }
    }
}
