using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 提供用于验证枚举值的信息。
        /// </summary>
        /// <typeparam name="T">枚举的类型。</typeparam>
        private static class EnumInfo<T> where T : struct, Enum
        {
            /// <summary>
            /// 表示当前枚举类型的所有值的集合。
            /// </summary>
            private static readonly HashSet<T> Values =
                new HashSet<T>((T[])Enum.GetValues(typeof(T)));

            /// <summary>
            /// 表示当前位域枚举类型的所有位域的组合。
            /// </summary>
            private static readonly ulong AllFlags =
                ((T[])Enum.GetValues(typeof(T))).Aggregate(
                    0UL, (last, value) => last | Convert.ToUInt64(value));

            /// <summary>
            /// 获取当前枚举类型是否为位域枚举类型。
            /// </summary>
            /// <returns>若当前枚举类型带有 <see cref="FlagsAttribute"/> 特性，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
            internal static bool IsFlags { get; } =
                typeof(T).GetCustomAttributes(false).Any(attr => attr is FlagsAttribute);

            /// <summary>
            /// 确定当前枚举类型是否包含指定的枚举值。
            /// </summary>
            /// <param name="value">要确定是否包含的枚举值。</param>
            /// <returns>若当前枚举类型包含 <paramref name="value"/> 表示的枚举值，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
            internal static bool ContainsValue(T value) => EnumInfo<T>.Values.Contains(value);

            /// <summary>
            /// 确定当前位域枚举类型是否包含指定的位域组合值。
            /// </summary>
            /// <param name="flag">要确定是否包含的位域组合值。</param>
            /// <returns>若当前枚举类型包含 <paramref name="flag"/> 表示的位域组合值，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
            internal static bool ContainsFlag(T flag) =>
                (EnumInfo<T>.AllFlags | Convert.ToUInt64(flag)) == EnumInfo<T>.AllFlags;
        }

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
            this IValueInfo<T> valueInfo, string? message = null)
            where T : struct, Enum
        {
            if (EnumInfo<T>.IsFlags)
            {
                if (!EnumInfo<T>.ContainsFlag(valueInfo.Value))
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(valueInfo.Name, valueInfo.Value, message);
                }
            }
            else
            {
                if (!EnumInfo<T>.ContainsValue(valueInfo.Value))
                {
                    ThrowHelper.ThrowArgumentOutOfRangeException(valueInfo.Name, valueInfo.Value, message);
                }
            }

            return valueInfo;
        }
    }
}
