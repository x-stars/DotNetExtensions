using System;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将对象表示为字符串的默认方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    internal sealed class DefaultObjectRepresenter<T> : ObjectRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="DefaultObjectRepresenter{T}"/> 类的新实例。
        /// </summary>
        public DefaultObjectRepresenter() { }

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public override string Represent(T value) => ObjectRepresenter.Represent(value);
    }
}
