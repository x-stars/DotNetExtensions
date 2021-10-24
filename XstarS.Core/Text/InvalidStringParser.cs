using System;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Text
{
    /// <summary>
    /// 表示无法由字符串形式转换为数值形式的类型的字符串解析对象。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    internal sealed class InvalidStringParser<T> : SimpleStringParser<T> where T : notnull
    {
        /// <summary>
        /// 初始化 <see cref="InvalidStringParser{T}"/> 类的新实例。
        /// </summary>
        public InvalidStringParser() { }

        /// <summary>
        /// 不支持此方法，始终抛出 <see cref="InvalidCastException"/> 异常。
        /// </summary>
        /// <param name="text">要进行转换的字符串表达形式。</param>
        /// <returns>始终抛出 <see cref="InvalidCastException"/> 异常。</returns>
        /// <exception cref="InvalidCastException">始终抛出此异常。</exception>
        [DoesNotReturn]
        [return: NotNull]
        public override T Parse(string text) => throw new InvalidCastException();
    }
}
