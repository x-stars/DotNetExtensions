namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供结构化对象相等比较的扩展方法。
    /// </summary>
    public static class StructuralEqualityExtensions
    {
        /// <summary>
        /// 确定当前结构化对象与指定结构化对象的元素是否对应相等。
        /// </summary>
        /// <param name="value">要进行相等比较的结构化对象。</param>
        /// <param name="other">要与当前结构化对象进行相等比较的结构化对象。</param>
        /// <typeparam name="T">结构化对象的类型。</typeparam>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool StructuralEquals<T>(this T? value, T? other) =>
            StructuralEqualityComparer.Equals(value, other);

        /// <summary>
        /// 获取当前结构化对象遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="value">要为其获取哈希代码的结构化对象。</param>
        /// <typeparam name="T">结构化对象的类型。</typeparam>
        /// <returns>遍历 <paramref name="value"/> 的元素得到的哈希代码。</returns>
        public static int GetStructuralHashCode<T>(this T? value) =>
            StructuralEqualityComparer.GetHashCode(value);
    }
}
