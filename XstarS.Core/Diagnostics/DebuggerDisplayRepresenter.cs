using System;
using System.Diagnostics;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供将应用了 <see cref="DebuggerDisplayAttribute"/> 特性的类型的对象表示为其调试器显示格式的方法。
    /// </summary>
    /// <typeparam name="T">应用了 <see cref="DebuggerDisplayAttribute"/> 特性的类型。</typeparam>
    [Serializable]
    internal sealed class DebuggerDisplayRepresenter<T> : SimpleRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="DebuggerDisplayRepresenter{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 当前类型未应用 <see cref="DebuggerDisplayAttribute"/> 特性。</exception>
        public DebuggerDisplayRepresenter()
        {
            if (!DebuggerDisplayRepresenter<T>.HasDebuggerDisplay)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 获取当前类型是否应用了 <see cref="DebuggerDisplayAttribute"/> 特性。
        /// </summary>
        /// <returns>若当前类型应用了 <see cref="DebuggerDisplayAttribute"/> 特性，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        internal static bool HasDebuggerDisplay =>
            Attribute.IsDefined(typeof(T), typeof(DebuggerDisplayAttribute));

        /// <summary>
        /// 将指定对象表示为其 <see cref="DebuggerDisplayAttribute"/> 特性定义的调试器显示格式的字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>调用 <paramref name="value"/> 的
        /// <see cref="DebuggerDisplayAttribute"/> 特性定义的调试器显示格式的字符串。</returns>
        public override string Represent(T? value) =>
            (value is null) ? Representer<T>.NullRefString : $"{{ {DebuggerDisplay.Format(value)} }}";
    }
}
