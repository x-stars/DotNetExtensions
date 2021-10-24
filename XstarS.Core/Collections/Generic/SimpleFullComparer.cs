using System;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 为无参数的相等和大小关系比较器 <see cref="IFullComparer{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public abstract class SimpleFullComparer<T> : FullComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="SimpleFullComparer{T}"/> 类的新实例。
        /// </summary>
        protected SimpleFullComparer() { }

        /// <summary>
        /// 确定指定的对象是否等于当前对象。
        /// </summary>
        /// <param name="obj">要与当前对象进行比较的对象。</param>
        /// <returns>如果指定的对象等于当前对象，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        public sealed override bool Equals(object? obj) =>
            this.GetType() == obj?.GetType();

        /// <summary>
        /// 获取当前对象的哈希代码。
        /// </summary>
        /// <returns>当前对象的哈希代码。</returns>
        public sealed override int GetHashCode() =>
            this.GetType().GetHashCode() * -1521134295;
    }
}
