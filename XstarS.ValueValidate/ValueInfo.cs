using System;

namespace XstarS
{
    /// <summary>
    /// 提供对象的值的验证和对应异常的抛出所需的数据。
    /// </summary>
    /// <typeparam name="T">待验证的对象的类型。</typeparam>
    [Serializable]
    internal sealed class ValueInfo<T> : IValueInfo<T>
    {
        /// <summary>
        /// 使用对象的值和名称初始化 <see cref="ValueInfo{T}"/> 结构的新实例。
        /// </summary>
        /// <param name="value">待验证的参数的值。</param>
        /// <param name="name">待验证的参数的名称。</param>
        public ValueInfo(T value, string name = null)
        {
            this.Value = value;
            this.Name = name;
        }

        /// <summary>
        /// 待验证的对象的值。
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// 待验证的对象的名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 返回此实例的字符串表达形式。
        /// </summary>
        /// <returns>此实例的字符串表达形式。</returns>
        public override string ToString()
        {
            return $"{{ {nameof(this.Name)} = {this.Name}, " +
                $"{nameof(this.Value)} = {this.Value} }}";
        }
    }
}
