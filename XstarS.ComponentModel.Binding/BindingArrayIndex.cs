using System;
using System.Linq;
using System.Linq.Expressions;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个用于数据绑定的数组索引。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的数组索引对应的值的类型。</typeparam>
    public class BindingArrayIndex<T> : BindingValueBase<T>
    {
        /// <summary>
        /// 使用包含数据绑定值的数组和应绑定到的索引初始化 <see cref="BindingIndexProperty{T}"/> 类的新实例。
        /// </summary>
        /// <param name="array">一个包含数据绑定值的数组。</param>
        /// <param name="indices">应绑定到的数组索引。</param>
        public BindingArrayIndex(Array array, params int[] indices)
            : base(array, string.Empty)
        {
            this.Indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }

        /// <summary>
        /// 应绑定到的数组索引。
        /// </summary>
        protected int[] Indices { get; }

        /// <summary>
        /// 构造 <see cref="BindingValueBase{T}.GetValue"/> 委托。
        /// </summary>
        /// <returns>构造完成的 <see cref="BindingValueBase{T}.GetValue"/> 委托。</returns>
        protected override Func<T> BuildGetValue()
        {
            try
            {
                var array = Expression.Constant(this.Instance);
                var indices = Enumerable.Range(0, this.Indices.Length).Select(
                    i => (Expression)Expression.Constant(this.Indices[i])).ToArray();
                var arrayIndex = Expression.ArrayAccess(array, indices);
                var getValue = Expression.Lambda<Func<T>>(arrayIndex);
                return getValue.Compile();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 构造 <see cref="BindingValueBase{T}.SetValue"/> 委托。
        /// </summary>
        /// <returns>构造完成的 <see cref="BindingValueBase{T}.SetValue"/> 委托。</returns>
        protected override Func<T, T> BuildSetValue()
        {
            try
            {
                var array = Expression.Constant(this.Instance);
                var newValue = Expression.Parameter(typeof(T));
                var indices = Enumerable.Range(0, this.Indices.Length).Select(
                    i => (Expression)Expression.Constant(this.Indices[i])).ToArray();
                var arrayIndex = Expression.ArrayAccess(array, indices);
                var assign = Expression.Assign(arrayIndex, newValue);
                var setValue = Expression.Lambda<Func<T, T>>(assign, newValue);
                return setValue.Compile();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
