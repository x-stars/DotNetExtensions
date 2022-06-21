using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XNetEx.Text;

/// <summary>
/// 表示使用类似于 <see cref="Uri.Uri(string)"/> 的构造函数将字符串转换为对象的字符串解析对象。
/// </summary>
/// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
internal sealed class ConstructorParser<T> : ObjectParser<T> where T : notnull
{
    /// <summary>
    /// 初始化 <see cref="ConstructorParser{T}"/> 类的新实例。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// <typeparamref name="T"/> 类型不提供从字符串构造的公共构造函数。</exception>
    public ConstructorParser()
    {
        if (!ConstructorParser<T>.CanParse)
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// 获取当前类型是否包含类似于 <see cref="Uri.Uri(string)"/> 的构造函数。
    /// </summary>
    /// <returns>若当前类型包含类似于 <see cref="Uri.Uri(string)"/> 的构造函数；
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    internal static bool CanParse
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        get => StringConstructor.CtorInfo is not null;
    }

    /// <summary>
    /// 将指定的字符串表示形式转换为其等效的对象。
    /// </summary>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
    /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
    public override T Parse(string text)
    {
        return (T)StringConstructor.CtorInfo!.Invoke(new[] { text })!;
    }

    /// <summary>
    /// 提供类似于 <see cref="Uri.Uri(string)"/> 的构造函数的元数据。
    /// </summary>
    private static class StringConstructor
    {
        /// <summary>
        /// 表示由字符串解析构造指定类型的对象的构造函数的元数据对象。
        /// </summary>
        internal static readonly ConstructorInfo? CtorInfo =
            typeof(T).GetConstructor(new[] { typeof(string) });
    }
}
