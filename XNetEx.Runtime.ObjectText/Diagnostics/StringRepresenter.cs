namespace XNetEx.Diagnostics;

/// <summary>
/// 提供将指定字符串包装后表示为字符串的方法。
/// </summary>
internal sealed class StringRepresenter : Representer<string>
{
    /// <summary>
    /// 初始化 <see cref="StringRepresenter"/> 类的新实例。
    /// </summary>
    public StringRepresenter() { }

    /// <summary>
    /// 将指定字符串包装后表示为字符串。
    /// </summary>
    /// <param name="value">要进行包装并表示的字符串。</param>
    /// <returns>字符串 <paramref name="value"/> 的包装形式。</returns>
    public override string Represent(string? value) =>
        (value is null) ? Representer<string>.NullRefString : $"\"{value}\"";
}
