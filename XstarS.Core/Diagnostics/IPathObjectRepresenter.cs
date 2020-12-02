using System.Collections.Generic;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将访问路径中的对象表示为字符串的方法。
    /// </summary>
    public interface IPathObjectRepresenter
    {
        /// <summary>
        /// 将访问路径中的指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent(object value, ISet<object> pathed);
    }

    /// <summary>
    /// 提供将访问路径中的指定类型的对象表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    public interface IPathObjectRepresenter<in T>
    {
        /// <summary>
        /// 将访问路径中的指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent(T value, ISet<object> pathed);
    }
}
