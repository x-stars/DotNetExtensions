using System.Diagnostics.CodeAnalysis;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将对象表示为字符串的方法。
    /// </summary>
    public interface IRepresenter
    {
        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent(object? value);
    }

    /// <summary>
    /// 提供将指定类型的对象表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    public interface IRepresenter<in T>
    {
        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string Represent([AllowNull] T value);
    }
}
