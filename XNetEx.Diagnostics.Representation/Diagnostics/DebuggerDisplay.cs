using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XNetEx.Diagnostics;

using StringPair = KeyValuePair<string, string>;

/// <summary>
/// 提供对象的 <see cref="DebuggerDisplayAttribute"/> 的格式化方法。
/// </summary>
public static class DebuggerDisplay
{
    /// <summary>
    /// 表示用于匹配 <see cref="DebuggerDisplayAttribute"/> 中的成员表达式的正则表达式。
    /// </summary>
    private static readonly Regex ExpressionMatcher =
        new Regex(@"(?<={)([^,{}]+)(,([^,{}]+))?(?=})");

    /// <summary>
    /// 表示指定类型的 <see cref="DebuggerDisplayAttribute"/> 的格式化方法的委托。
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Func<object, string?>> Formatters =
        new ConcurrentDictionary<Type, Func<object, string?>>();

    /// <summary>
    /// 将指定对象表示为 <see cref="DebuggerDisplayAttribute"/> 指定的格式。
    /// </summary>
    /// <param name="instance">要进行格式化表示的对象。</param>
    /// <returns><paramref name="instance"/> 按照
    /// <see cref="DebuggerDisplayAttribute"/> 格式化得到的字符串。</returns>
    public static string? Format(object instance)
    {
        if (instance == null) { return null; }
        var type = instance.GetType();
        var formatter = DebuggerDisplay.Formatters.GetOrAdd(type,
            newType => DebuggerDisplay.CreateFormatter(type));
        return formatter.Invoke(instance);
    }

    /// <summary>
    /// 获取指定类型的 <see cref="DebuggerDisplayAttribute"/> 的格式化字符串。
    /// </summary>
    /// <param name="type">要获取格式字符串的对象的类型。</param>
    /// <returns><paramref name="type"/> 类型的
    /// <see cref="DebuggerDisplayAttribute"/> 的格式化字符串；
    /// 若并未应用此特性，则为 <see langword="null"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
    public static string? GetFormat(Type type)
    {
        if (type is null) { throw new ArgumentNullException(nameof(type)); }
        if (!type.IsDefined(typeof(DebuggerDisplayAttribute), inherit: true)) { return null; }
        var attribute = Attribute.GetCustomAttribute(type, typeof(DebuggerDisplayAttribute));
        return ((DebuggerDisplayAttribute)attribute!).Value;
    }

    /// <summary>
    /// 创建指定类型的 <see cref="DebuggerDisplayAttribute"/> 的格式化方法的委托。
    /// </summary>
    /// <param name="type">要创建格式化方法的对象的类型。</param>
    /// <returns><paramref name="type"/> 类型的
    /// <see cref="DebuggerDisplayAttribute"/> 的格式化方法的委托。</returns>
    private static Func<object, string?> CreateFormatter(Type type)
    {
        var index = 0;
        var display = DebuggerDisplay.GetFormat(type);
        if (display is null) { return instance => instance?.ToString(); }
        var format = ExpressionMatcher.Replace(display, match => (index++).ToString());
        var formatters = ExpressionMatcher.Matches(display).Cast<Match>()
            .Select(match => new StringPair(match.Groups[1].Value, match.Groups[3].Value))
            .Select(member => DebuggerDisplay.CreateMemberFormatter(type, member.Key, member.Value))
            .ToArray();
        return instance => string.Format(format,
            Array.ConvertAll(formatters, formatter => formatter.Invoke(instance)));
    }

    /// <summary>
    /// 创建指定成员表达式的的格式化方法的委托。
    /// </summary>
    /// <param name="type">成员所在的类型。</param>
    /// <param name="name">要访问的成员的名称；对于方法调用，应以 <c>()</c> 结尾。</param>
    /// <param name="format">成员的格式化方法；应为 <c>nq</c> 或 <c>raw</c>，或为空值。</param>
    /// <returns><paramref name="type"/> 类型中名为 <paramref name="name"/>
    /// 的成员按照 <paramref name="format"/> 指定的格式进行格式化的方法的委托。</returns>
    private static Func<object, string?> CreateMemberFormatter(Type type, string name, string format)
    {
        var accessor = DebuggerDisplay.CreateMemberAccessor(type, name);
        var formatter = DebuggerDisplay.CreateValueFormatter(format);
        return instance => formatter.Invoke(accessor.Invoke(instance));
    }

    /// <summary>
    /// 创建指定类型的指定名称的成员的访问方法的委托。
    /// </summary>
    /// <param name="type">成员所在的类型。</param>
    /// <param name="name">要访问的成员的名称；对于方法调用，应以 <c>()</c> 结尾。</param>
    /// <returns><paramref name="type"/> 类型中名为
    /// <paramref name="name"/> 的成员的访问方法的委托。</returns>
    private static Func<object, object?> CreateMemberAccessor(Type type, string name)
    {
        name = name.Trim();
        var instFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        if (!name.EndsWith("()"))
        {
            var field = type.GetField(name, instFlags);
            if (field is not null)
            {
                return instance => field.GetValue(instance);
            }

            var property = type.GetProperties(instFlags)
                .Where(property => property.Name == name)
                .Where(property => property.GetIndexParameters().Length == 0)
                .Where(property => property.CanRead)
                .SingleOrDefault();
            if (property is not null)
            {
                return instance => property.GetValue(instance);
            }
        }
        else
        {
            var method = type.GetMethods(instFlags)
                .Where(method => method.Name == name)
                .Where(method => method.GetParameters().Length == 0)
                .SingleOrDefault();
            if (method is not null)
            {
                return instance => method.Invoke(instance, Array.Empty<object>());
            }
        }

        return instance => $"<!{new MissingMemberException()}>";
    }

    /// <summary>
    /// 创建指定格式字符串对应的格式化方法的委托。
    /// </summary>
    /// <param name="format">成员的格式化方法；应为 <c>nq</c> 或 <c>raw</c>，或为空值。</param>
    /// <returns>按照 <paramref name="format"/> 指定的格式进行格式化的方法的委托。</returns>
    private static Func<object?, string?> CreateValueFormatter(string format)
    {
        return instance => instance switch
        {
            string text => format switch
            {
                "nq" or "raw" => text,
                "" => $"\"{text}\"",
                _ => $"<!{new FormatException()}>"
            },
            object value => value.GetType().IsPrimitive ?
                value.ToString() : $"[{value.ToString()}]",
            _ => string.Empty
        };
    }
}
