using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将非结构化对象表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">非结构化对象的类型。</typeparam>
    internal sealed class PlainRepresenter<T> : InternalStructuralRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="PlainRepresenter{T}"/> 类的新实例。
        /// </summary>
        public PlainRepresenter() { }

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected override string RepresentCore([DisallowNull] T value, ISet<object> represented)
        {
            return Representer<T>.Default.Represent(value);
        }
    }
}
