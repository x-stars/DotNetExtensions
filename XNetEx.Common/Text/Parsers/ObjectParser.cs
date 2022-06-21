using System;

namespace XNetEx.Text;

/// <summary>
/// 为字符串解析对象提供抽象基类。
/// </summary>
/// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
internal abstract class ObjectParser<T> where T : notnull
{
    /// <summary>
    /// 初始化 <see cref="ObjectParser{T}"/> 类的新实例。
    /// </summary>
    protected ObjectParser() { }

    /// <summary>
    /// 获取 <see cref="ObjectParser{T}"/> 类的默认实例。
    /// </summary>
    /// <returns><see cref="ObjectParser{T}"/> 类的默认实例。</returns>
    public static ObjectParser<T> Default { get; } = ObjectParser<T>.CreateDefault();

    /// <summary>
    /// 创建 <see cref="ObjectParser{T}"/> 类的默认实例。
    /// </summary>
    /// <returns><see cref="ObjectParser{T}"/> 类的默认实例。</returns>
    private static ObjectParser<T> CreateDefault()
    {
        if (typeof(T).IsEnum)
        {
            return new EnumNameParser<T>();
        }
        else if (typeof(T) == typeof(string))
        {
            return (ObjectParser<T>)(object)new StringParser();
        }
        else if (typeof(T) == typeof(Type))
        {
            return (ObjectParser<T>)(object)new TypeNameParser();
        }
        else if (ParseMethodParser<T>.CanParse)
        {
            return new ParseMethodParser<T>();
        }
        else if (ConstructorParser<T>.CanParse)
        {
            return new ConstructorParser<T>();
        }
        else if (ExtensionMethodParser<T>.CanParse)
        {
            return new ExtensionMethodParser<T>();
        }
        else
        {
            return new InvalidObjectParser<T>();
        }
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
    /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
    /// <exception cref="OverflowException">
    /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
    public abstract T Parse(string text);

    /// <summary>
    /// 尝试将指定的字符串表示形式转换为其等效的对象，并返回转换是否成功。
    /// </summary>
    /// <param name="text">包含要转换的对象的字符串。</param>
    /// <param name="result">与 <paramref name="text"/> 中的内容等效的对象。</param>
    /// <returns>若 <paramref name="text"/> 成功转换，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public bool TryParse(string text, out T? result)
    {
        try { result = this.Parse(text); return true; }
        catch (Exception) { result = default(T); return false; }
    }
}
