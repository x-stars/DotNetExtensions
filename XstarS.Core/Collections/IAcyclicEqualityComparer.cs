using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供用于无环地比较对象是否相等的方法。
    /// </summary>
    public interface IAcyclicEqualityComparer : IEqualityComparer
    {
        /// <summary>
        /// 无环地确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经访问过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        bool Equals(object x, object y, ISet<KeyValuePair<object, object>> compared);

        /// <summary>
        /// 无环地获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经访问过的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        int GetHashCode(object obj, ISet<object> computed);
    }
}
