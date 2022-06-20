using System;
using System.Globalization;
using System.Windows.Data;

namespace XNetEx.Windows.Data;

/// <summary>
/// 表示不做任何转换的空转换器。
/// </summary>
[ValueConversion(typeof(object), typeof(object))]
public sealed class EmptyConverter : IValueConverter
{
    /// <summary>
    /// 初始化 <see cref="EmptyConverter"/> 类的新实例。
    /// </summary>
    public EmptyConverter() { }

    /// <summary>
    /// 原样返回绑定源生成的值。
    /// </summary>
    /// <param name="value">绑定源生成的值。</param>
    /// <param name="targetType">绑定目标属性的类型。不使用此参数。</param>
    /// <param name="parameter">要使用的转换器参数。不使用此参数。</param>
    /// <param name="culture">要用在转换器中的区域性。不使用此参数。</param>
    /// <returns><paramref name="value"/> 本身。</returns>
    public object Convert(object value,
        Type targetType, object parameter, CultureInfo culture) => value;

    /// <summary>
    /// 原样返回绑定目标生成的值。
    /// </summary>
    /// <param name="value">绑定目标生成的值。</param>
    /// <param name="targetType">绑定源属性的类型。不使用此参数。</param>
    /// <param name="parameter">要使用的转换器参数。不使用此参数。</param>
    /// <param name="culture">要用在转换器中的区域性。不使用此参数。</param>
    /// <returns><paramref name="value"/> 本身。</returns>
    public object ConvertBack(object value,
        Type targetType, object parameter, CultureInfo culture) => value;
}
