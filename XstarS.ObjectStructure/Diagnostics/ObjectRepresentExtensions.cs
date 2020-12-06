namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将对象表示为字符串的扩展方法。
    /// </summary>
    public static class ObjectRepresentExtensions
    {
        /// <summary>
        /// 将当前对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public static string RepresentToString<T>(this T value) =>
            ObjectRepresenter.Represent(value);
    }
}
