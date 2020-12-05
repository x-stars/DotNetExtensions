using System;
using System.Collections.Generic;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将指定类型的对象表示为其 <see cref="object.ToString()"/> 方法返回的字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为为字符串的对象的类型。</typeparam>
    [Serializable]
    internal sealed class ObjectToStringRepresenter<T> : ObjectRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ObjectToStringRepresenter{T}"/> 类的新实例。
        /// </summary>
        public ObjectToStringRepresenter() { }

        /// <summary>
        /// 将指定对象表示为其 <see cref="object.ToString()"/> 方法返回的字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="pathed">已经在路径中访问过的对象。</param>
        /// <returns>调用 <paramref name="value"/> 的
        /// <see cref="object.ToString()"/> 方法返回的字符串。</returns>
        protected override string RepresentCore(T value, ISet<object> pathed) => value.ToString();
    }
}
