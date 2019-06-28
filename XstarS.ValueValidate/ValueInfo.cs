using System;
using System.Collections.Generic;

namespace XstarS
{
    /// <summary>
    /// 提供对象的值的验证和对应异常的抛出所需的数据。
    /// </summary>
    /// <typeparam name="T">待验证的对象的类型。</typeparam>
    [Serializable]
    internal struct ValueInfo<T> : IValueInfo<T>
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
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>如果 <paramref name="obj"/> 是 <see cref="IValueInfo{T}"/> 的实例，
        /// 且 <see cref="IValueInfo{T}.Value"/> 和 <see cref="IValueInfo{T}.Name"/> 属性都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj)
        {
            return (obj is IValueInfo<T> value) &&
                EqualityComparer<T>.Default.Equals(this.Value, value.Value) &&
                (this.Name == value.Name);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = -1670801664;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.Value);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
            return hashCode;
        }

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
