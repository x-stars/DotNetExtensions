using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个用于数据绑定的索引属性或数组索引。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的索引对应的值的类型。</typeparam>
    public class BindingIndex<T> : BindingMember<T>
    {
        /// <summary>
        /// <see cref="BindingIndex{T}.Indices"/> 的值。
        /// </summary>
        private readonly object[] InternalIndices;

        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的索引初始化 <see cref="BindingIndex{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="indices">设置数据绑定时应绑定到的索引。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingIndex(object instance, params object[] indices)
            : base(instance, "Item[]")
        {
            this.InternalIndices = indices ?? throw new ArgumentNullException(nameof(indices));
        }

        /// <summary>
        /// 当前实例的 <see cref="IDisposable"/> 检查对象。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前实例已经被释放。</exception>
        protected new BindingIndex<T> Disposable => (BindingIndex<T>)base.Disposable;

        /// <summary>
        /// 设置数据绑定时应绑定到的索引。
        /// </summary>
        protected object[] Indices => this.Disposable.InternalIndices;

        /// <summary>
        /// 构造获取数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的获取数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造获取数据绑定值的委托。</exception>
        protected override Func<T> BuildGetValue()
        {
            try
            {
                var instance = Expression.Constant(this.Instance);
                var indices = Array.ConvertAll(this.Indices, Expression.Constant);
                var indexer = (this.Instance is Array) ?
                    Expression.ArrayAccess(instance, indices) :
                    Expression.Property(instance, "Item", indices);
                var getValue = Expression.Lambda<Func<T>>(indexer);
                return getValue.Compile();
            }
            catch (Exception)
            {
                throw new MissingMemberException(this.Instance.GetType().ToString(), this.PropertyName);
            }
        }

        /// <summary>
        /// 构造设置数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的设置数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造设置数据绑定值的委托。</exception>
        protected override Action<T> BuildSetValue()
        {
            try
            {
                var instance = Expression.Constant(this.Instance);
                var newValue = Expression.Parameter(typeof(T));
                var indices = Array.ConvertAll(this.Indices, Expression.Constant);
                var indexer = (this.Instance is Array) ?
                    Expression.ArrayAccess(instance, indices) :
                    Expression.Property(instance, "Item", indices);
                var assign = Expression.Assign(indexer, newValue);
                var setValue = Expression.Lambda<Action<T>>(assign, newValue);
                return setValue.Compile();
            }
            catch (Exception)
            {
                throw new MissingMemberException(this.Instance.GetType().ToString(), this.PropertyName);
            }
        }
    }
}
