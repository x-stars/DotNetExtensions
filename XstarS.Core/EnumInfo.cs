using System;

namespace XstarS
{
    /// <summary>
    /// 提供枚举类型的元数据信息。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    public static class EnumInfo<TEnum> where TEnum : struct, Enum
    {
        /// <summary>
        /// 获取当前枚举类型是否应用了 <see cref="FlagsAttribute"/> 特性。
        /// </summary>
        /// <returns>当前枚举类型是否应用了 <see cref="FlagsAttribute"/> 特性。</returns>
        public static bool IsFlags =>
            Attribute.IsDefined(typeof(TEnum), typeof(FlagsAttribute), inherit: false);

        /// <summary>
        /// 检索当前枚举中常数名称的数组。
        /// </summary>
        /// <returns>当前枚举的常数名称的字符串数组。</returns>
        public static string[] Names => Enum.GetNames(typeof(TEnum));

        /// <summary>
        /// 检索当前枚举中常数值的数组。
        /// </summary>
        /// <returns>当前枚举的常数值的数组。</returns>
        public static TEnum[] Values => (TEnum[])Enum.GetValues(typeof(TEnum));

        /// <summary>
        /// 获取当前枚举类型的基础类型的类型代码。
        /// </summary>
        /// <returns>当前枚举类型的基础类型的类型代码。</returns>
        public static TypeCode TypeCode => default(TEnum).GetTypeCode();

        /// <summary>
        /// 返回当前枚举的基础类型。
        /// </summary>
        /// <returns>当前枚举的的基础类型。</returns>
        public static Type UnderlyingType => Enum.GetUnderlyingType(typeof(TEnum));

        /// <summary>
        /// 确定指定的枚举常数是否存在于当前枚举中。
        /// </summary>
        /// <param name="value">当前枚举中的常量值。</param>
        /// <returns>如果当前枚举中的某个常量的值等于 <paramref name="value"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsDefined(TEnum value) => Enum.IsDefined(typeof(TEnum), value);

        /// <summary>
        /// 在当前枚举中检索具有指定值的常数的名称。
        /// </summary>
        /// <param name="value">特定枚举常量的值。</param>
        /// <returns>当前枚举中值为 <paramref name="value"/> 的枚举常量的名称；
        /// 如果没有找到这样的常量，则为 <see langword="null"/>。</returns>
        public static string? NameOf(TEnum value) => Enum.GetName(typeof(TEnum), value);

        /// <summary>
        /// 将一个或多个枚举常数的名称或数字值的字符串表示转换成等效的当前枚举对象。
        /// </summary>
        /// <param name="value">包含要转换的值或名称的字符串。</param>
        /// <returns>e当前枚举类型的对象，其值由 <paramref name="value"/> 表示。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 为空字符串或只包含空格。
        /// 或 <paramref name="value"/> 是一个名称，但不是当前枚举定义的指定常量之一。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> 超出了当前枚举的基础类型范围。</exception>
        public static TEnum Parse(string value) => (TEnum)Enum.Parse(typeof(TEnum), value);

        /// <summary>
        /// 将具有整数值的指定对象转换为当前枚举成员。
        /// </summary>
        /// <param name="value">要转换为枚举成员的值。</param>
        /// <returns>值为 <paramref name="value"/> 的枚举对象。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> 不是有效的枚举基础类型的值。</exception>
        public static TEnum ValueOf(object value) => (TEnum)Enum.ToObject(typeof(TEnum), value);
    }
}
