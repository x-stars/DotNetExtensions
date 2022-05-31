namespace XNetEx.Diagnostics
{
    /// <summary>
    /// 提供将结构化对象表示为字符串的扩展方法。
    /// </summary>
    public static class StructuralRepresentExtensions
    {
        /// <summary>
        /// 将当前结构化对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的结构化对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public static string StructuralRepresent<T>(this T? value) =>
            StructuralRepresenter.Represent(value);
    }
}
