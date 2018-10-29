using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 提供参数的验证和对应异常的抛出所需的数据。
    /// </summary>
    /// <typeparam name="T">待验证的参数的类型。</typeparam>
    internal struct Validate<T> : IValidate<T>
    {
        /// <summary>
        /// 使用参数的值和名称初始化 <see cref="Validate{T}"/> 结构的新实例。
        /// </summary>
        /// <param name="value">待验证的参数的值。</param>
        /// <param name="name">待验证的参数的名称。</param>
        public Validate(T value, string name = null)
        {
            this.Value = value;
            this.Name = name;
        }

        /// <summary>
        /// 待验证的参数的值。
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// 待验证的参数的名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="IValidate{T}"/> 的实例，
        /// 且 <see cref="IValidate{T}.Value"/> 和 <see cref="IValidate{T}.Name"/> 属性都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return (obj is IValidate<T> value) &&
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
    
    /// <summary>
    /// 提供参数的验证与对应异常的抛出的方法。
    /// 应先调用 <see cref="Validate.Value{T}(T, string)"/> 方法，
    /// 创建一个 <see cref="IValidate{T}"/> 接口的实例再进行参数的验证。
    /// </summary>
    public static partial class Validate
    {
        /// <summary>
        /// 使用参数的值和名称创建一个 <see cref="IValidate{T}"/> 接口的新实例。
        /// </summary>
        /// <typeparam name="T">待验证的参数的类型。</typeparam>
        /// <param name="value">待验证的参数的值。</param>
        /// <param name="name">待验证的参数的名称，一般通过 <see langword="nameof"/> 获取。</param>
        /// <returns><see cref="IValidate{T}"/> 接口的新实例。</returns>
        public static IValidate<T> Value<T>(T value, string name = null)
        {
            return new Validate<T>(value, name);
        }
    }
}
