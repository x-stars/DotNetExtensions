namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供泛型集合的扩展方法。
    /// </summary>
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// 确定当前集合与指定集合的元素是否对应相等。
        /// </summary>
        /// <param name="value">要进行相等比较的集合。</param>
        /// <param name="other">要与当前集合进行相等比较的集合。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool CollectionEquals<T>(this T value, T other)
        {
            return CollectionEqualityComparer<T>.Default.Equals(value, other);
        }

        /// <summary>
        /// 获取当前集合遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="value">要为其获取哈希代码的集合。</param>
        /// <returns>遍历 <paramref name="value"/> 的元素得到的哈希代码。</returns>
        public static int GetCollectionHashCode<T>(this T value)
        {
            return CollectionEqualityComparer<T>.Default.GetHashCode(value);
        }
    }
}
