namespace XstarS.Text
{
    /// <summary>
    /// 提供将对象格式化为字符串的方法。
    /// </summary>
    public interface ITextFormatter
    {
        /// <summary>
        /// 将指定对象格式化为字符串。
        /// </summary>
        /// <param name="value">要格式化的对象。</param>
        /// <returns><paramref name="value"/> 的格式化字符串。</returns>
        string Format(object value);
    }

    /// <summary>
    /// 提供将指定类型的对象格式化为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要格式化为字符串的对象的类型。</typeparam>
    public interface ITextFormatter<T>
    {
        /// <summary>
        /// 将指定对象格式化为字符串。
        /// </summary>
        /// <param name="value">要格式化的对象。</param>
        /// <returns><paramref name="value"/> 的格式化字符串。</returns>
        string Format(T value);
    }
}
