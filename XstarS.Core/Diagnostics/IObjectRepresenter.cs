namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将对象表示为字符串的方法。
    /// </summary>
    public interface IObjectRepresenter
    {
        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Format(object value);
    }

    /// <summary>
    /// 提供将指定类型的对象表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    public interface IObjectRepresenter<T>
    {
        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Format(T value);
    }
}
