﻿using System;

namespace XNetEx.Text;

/// <summary>
/// 提供将字符串解析为对象的方法。
/// </summary>
public static class ObjectParser
{
    /// <summary>
    /// 将当前字符串表示形式转换为其等效的对象。
    /// </summary>
    /// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
    /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
    /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
    public static T ParseAs<T>(this string text) where T : notnull
    {
        return ObjectParser<T>.Default.Parse(text);
    }

    /// <summary>
    /// 尝试将指定的字符串表示形式转换为其等效的对象，并返回转换是否成功。
    /// </summary>
    /// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <param name="result">与 <paramref name="text"/> 中的内容等效的对象。</param>
    /// <returns>若 <paramref name="text"/> 成功转换，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool TryParseAs<T>(this string text, out T? result) where T : notnull
    {
        return ObjectParser<T>.Default.TryParse(text, out result);
    }
}