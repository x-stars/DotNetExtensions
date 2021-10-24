﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将指定类型的对象表示为其 <see cref="object.ToString()"/> 方法返回的字符串的包装的方法。
    /// </summary>
    /// <typeparam name="T">要表为为字符串的对象的类型。</typeparam>
    [Serializable]
    internal sealed class ToStringRepresenter<T> : SimpleRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ToStringRepresenter{T}"/> 类的新实例。
        /// </summary>
        public ToStringRepresenter() { }

        /// <summary>
        /// 将指定对象表示为其 <see cref="object.ToString()"/> 方法返回的字符串的包装。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>调用 <paramref name="value"/> 的
        /// <see cref="object.ToString()"/> 方法返回的字符串的包装。</returns>
        public override string Represent([AllowNull] T value) =>
            (value is null) ? Representer<T>.NullRefString : $"[{value?.ToString()}]";
    }
}
