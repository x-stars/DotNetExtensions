using System.Collections.Generic;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将对象无环地表示为字符串的方法。
    /// </summary>
    public interface IAcyclicObjectRepresenter : IObjectRepresenter
    {
        /// <summary>
        /// 将指定对象表示无环地为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent(object value, ISet<object> represented);
    }

    /// <summary>
    /// 提供将指定类型的对象无环地表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    public interface IAcyclicObjectRepresenter<in T> : IObjectRepresenter<T>
    {
        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent(T value, ISet<object> represented);
    }
}
