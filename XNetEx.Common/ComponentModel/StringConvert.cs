using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;

namespace XNetEx.ComponentModel;

/// <summary>
/// 提供将字符串转换为对象的方法。
/// </summary>
public static class StringConvert
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> Converters =
        new ConcurrentDictionary<Type, TypeConverter>();

    private static TypeConverter GetConverter<T>() =>
        StringConvert.Converters.GetOrAdd(typeof(T), TypeDescriptor.GetConverter);

    /// <summary>
    /// 获取当前字符串表示形式是否能转换为指定类型的对象。
    /// </summary>
    /// <typeparam name="T">要从字符串转换为对象的类型。</typeparam>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <returns>若 <paramref name="text"/> 能转换为 <typeparamref name="T"/> 类型的对象，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool CanConvertTo<T>(string text)
    {
        var converter = StringConvert.GetConverter<T>();
        return converter.CanConvertFrom(typeof(string)) && converter.IsValid(text);
    }

    /// <summary>
    /// 将当前字符串表示形式转换为其等效的对象。
    /// </summary>
    /// <typeparam name="T">要从字符串转换为对象的类型。</typeparam>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <param name="culture"><see cref="CultureInfo"/>。
    /// 如果传递 <see langword="null"/>，则采用当前区域性。</param>
    /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
    /// <exception cref="NotSupportedException"><paramref name="text"/> 不能转换为适当的对象。</exception>
    /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
    /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
    public static T? ConvertTo<T>(this string text, CultureInfo? culture = null)
    {
        var converter = StringConvert.GetConverter<T>();
        return (T?)converter.ConvertFromString(context: null, culture, text);
    }

    /// <summary>
    /// 尝试将指定的字符串表示形式转换为其等效的对象，并返回转换是否成功。
    /// </summary>
    /// <typeparam name="T">要从字符串转换为对象的类型。</typeparam>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <param name="result">与 <paramref name="text"/> 中的内容等效的对象。</param>
    /// <param name="culture"><see cref="CultureInfo"/>。
    /// 如果传递 <see langword="null"/>，则采用当前区域性。</param>
    /// <returns>若 <paramref name="text"/> 成功转换，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool TryConvertTo<T>(
        this string text, out T? result, CultureInfo? culture = null)
    {
        if (StringConvert.CanConvertTo<T>(text))
        {
            try
            {
                var converter = StringConvert.GetConverter<T>();
                result = (T?)converter.ConvertFromString(context: null, culture, text);
                return true;
            }
            catch (Exception) { }
        }
        result = default(T?);
        return false;
    }
}
