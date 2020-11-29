using System;
using XstarS.Text;

namespace XstarS
{
    /// <summary>
    /// 提供将指定类型的对象格式化为字符串的默认方法。
    /// </summary>
    /// <typeparam name="T">要格式化为字符串的对象的类型。</typeparam>
    [Serializable]
    internal sealed class ObjectTextFormatter<T> : TextFormatter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ObjectTextFormatter{T}"/> 类的新实例。
        /// </summary>
        public ObjectTextFormatter() { }

        /// <summary>
        /// 将指定对象格式化为默认的字符串。
        /// </summary>
        /// <param name="value">要格式化的对象。</param>
        /// <returns>调用 <paramref name="value"/> 的
        /// <see cref="object.ToString()"/> 方法返回的字符串。</returns>
        public override string Format(T value) => value?.ToString();
    }
}
