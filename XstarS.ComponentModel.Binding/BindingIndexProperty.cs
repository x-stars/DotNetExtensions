using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个用于数据绑定的索引属性。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的索引对应的值的类型。</typeparam>
    public class BindingIndexProperty<T> : BindingValueBase<T>
    {
        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的属性名称以及索引信息初始化 <see cref="BindingIndexProperty{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <param name="indexerName">应访问的索引器的名称。</param>
        /// <param name="indices">访问索引器时使用的参数。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingIndexProperty(object instance, string propertyName, string indexerName, params object[] indices)
            : base(instance, propertyName)
        {
            this.IndexerName = indexerName ?? throw new ArgumentNullException(nameof(indexerName));
            this.Indices = indices ?? throw new ArgumentNullException(nameof(indices));
        }

        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的属性名称以及索引信息初始化 <see cref="BindingProperty{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <param name="indices">访问索引器时使用的参数。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingIndexProperty(object instance, string propertyName, params object[] indices)
            : this(instance, propertyName, "Item", indices) { }

        /// <summary>
        /// 应访问的索引器的名称。
        /// </summary>
        protected string IndexerName { get; }

        /// <summary>
        /// 访问索引器时使用的参数。
        /// </summary>
        protected object[] Indices { get; }

        /// <summary>
        /// 构造 <see cref="BindingValueBase{T}.GetValue"/> 委托。
        /// </summary>
        /// <returns>构造完成的 <see cref="BindingValueBase{T}.GetValue"/> 委托。</returns>
        protected override Func<T> BuildGetValue()
        {
            try
            {
                var instance = Expression.Constant(this.Instance);
                var arguments = Enumerable.Range(0, this.Indices.Length).Select(
                    i => (Expression)Expression.Constant(this.Indices[i])).ToArray();
                var indexer = Expression.Property(instance, this.IndexerName, arguments);
                var getValue = Expression.Lambda<Func<T>>(indexer);
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
                var instance = Expression.Constant(this.Instance);
                var newValue = Expression.Parameter(typeof(T));
                var arguments = Enumerable.Range(0, this.Indices.Length).Select(
                    i => (Expression)Expression.Constant(this.Indices[i])).ToArray();
                var indexer = Expression.Property(instance, this.IndexerName, arguments);
                var assign = Expression.Assign(indexer, newValue);
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
