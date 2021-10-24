using System;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将 <see cref="IRepresentable"/> 对象表示为其
    /// <see cref="IRepresentable.Represent()"/> 方法返回的字符串的方法。
    /// </summary>
    /// <typeparam name="T"><see cref="IRepresentable"/> 对象的类型。</typeparam>
    [Serializable]
    internal sealed class RepresentableRepresenter<T> : SimpleRepresenter<T>
        where T : IRepresentable
    {
        /// <summary>
        /// 初始化 <see cref="RepresentableRepresenter{T}"/> 类的新实例。
        /// </summary>
        public RepresentableRepresenter() { }

        /// <summary>
        /// 将指定对象表示为其 <see cref="IRepresentable.Represent()"/> 方法返回的字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>调用 <paramref name="value"/> 的
        /// <see cref="IRepresentable.Represent()"/> 方法返回的字符串。</returns>
        public override string Represent([AllowNull] T value) =>
            (value is null) ? Representer<T>.NullRefString : value.Represent();
    }
}
