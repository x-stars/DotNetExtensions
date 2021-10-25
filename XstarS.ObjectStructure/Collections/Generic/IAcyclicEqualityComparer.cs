using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供用于无环地比较对象是否相等的方法。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    public interface IAcyclicEqualityComparer<in T> : IEqualityComparer<T>
    {
        /// <summary>
        /// 无环地确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        bool Equals(T? x, T? y, ISet<ObjectPair> compared);

        /// <summary>
        /// 无环地获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        int GetHashCode(T? obj, ISet<object> computed);
    }
}
