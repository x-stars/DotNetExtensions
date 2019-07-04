using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 用于构造数据绑定路径 <see cref="BindingPath{T}"/> 对象。
    /// </summary>
    public class BindingPathBuilder
    {
        /// <summary>
        /// 表示一个可提供用于数据绑定的属性或字段 <see cref="BindingProperty{T}"/> 的对象。
        /// </summary>
        /// <typeparam name="T">用于数据绑定的属性或字段的类型。</typeparam>
        internal class BindingPropertyProvider<T> : IBindingValueProvider
        {
            /// <summary>
            /// 设置数据绑定时应绑定到的属性名称。
            /// </summary>
            public readonly string PropertyName;

            /// <summary>
            /// 使用应绑定到的属性名称初始化 <see cref="BindingPropertyProvider{T}"/> 类的新实例。
            /// </summary>
            /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="propertyName"/> 为 <see langword="null"/>。</exception>
            public BindingPropertyProvider(string propertyName)
            {
                this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            }

            /// <summary>
            /// 根据包含数据绑定值的实例获取对应的 <see cref="BindingProperty{T}"/> 对象。
            /// </summary>
            /// <param name="instance">一个包含数据绑定值的实例。</param>
            /// <returns><paramref name="instance"/> 对应的 <see cref="BindingProperty{T}"/> 对象。</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
            public IBindingValue GetBindingValue(object instance) =>
                new BindingProperty<T>(instance, this.PropertyName);
        }

        /// <summary>
        /// 表示一个可提供用于数据绑定的索引属性或数组索引 <see cref="BindingIndex{T}"/> 的对象。
        /// </summary>
        /// <typeparam name="T">用于数据绑定的索引属性或数组索引的类型。</typeparam>
        internal class BindingIndexProvider<T> : IBindingValueProvider
        {
            /// <summary>
            /// 设置数据绑定时应绑定到的索引。
            /// </summary>
            public readonly object[] Indices;

            /// <summary>
            /// 使用应绑定到的索引初始化 <see cref="BindingIndexProvider{T}"/> 类的新实例。
            /// </summary>
            /// <param name="indices">设置数据绑定时应绑定到的索引。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="indices"/> 为 <see langword="null"/>。</exception>
            public BindingIndexProvider(params object[] indices)
            {
                this.Indices = indices ?? throw new ArgumentNullException(nameof(indices));
            }

            /// <summary>
            /// 根据包含数据绑定值的实例获取对应的 <see cref="BindingProperty{T}"/> 对象。
            /// </summary>
            /// <param name="instance">一个包含数据绑定值的实例。</param>
            /// <returns><paramref name="instance"/> 对应的 <see cref="BindingIndex{T}"/> 对象。</returns>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
            public IBindingValue GetBindingValue(object instance) =>
                new BindingIndex<T>(instance, this.Indices);
        }

        /// <summary>
        /// 用于构造数据绑定路径 <see cref="BindingPathBuilder.ValuePath"/> 的 <see cref="StringBuilder"/>。
        /// </summary>
        private readonly StringBuilder ValuePathBuilder;

        /// <summary>
        /// <see cref="BindingPathBuilder.Instance"/> 对应的表达式树。
        /// </summary>
        private readonly Expression InstanceExpression;

        /// <summary>
        /// 用于数据绑定的值的表达式树。
        /// </summary>
        private Expression ValueExpression;

        /// <summary>
        /// 数据绑定路径中所有节点的 <see cref="IBindingValue"/> 提供对象。
        /// </summary>
        private readonly List<IBindingValueProvider> BindingValueProviders;

        /// <summary>
        /// 数据绑定路径中最后一个节点的类型。
        /// </summary>
        private Type LastInstanceType;

        /// <summary>
        /// 使用包含数据绑定值的对象初始化 <see cref="BindingPathBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        public BindingPathBuilder(object instance)
        {
            this.Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.BindingValueProviders = new List<IBindingValueProvider>();
            this.InstanceExpression = Expression.Constant(this.Instance);
            this.ValueExpression = this.InstanceExpression;
            this.ValuePathBuilder = new StringBuilder();
            this.LastInstanceType = this.Instance.GetType();
        }

        /// <summary>
        /// 包含数据绑定值的对象。
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// 从包含数据绑定值的对象到数据绑定值的路径。
        /// </summary>
        public string ValuePath => this.ValuePathBuilder.ToString();

        /// <summary>
        /// 添加一个用于数据绑定的属性或字段到当前数据绑定路径。
        /// </summary>
        /// <typeparam name="T">用于数据绑定的属性或字段的类型。</typeparam>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <returns>当前 <see cref="BindingPathBuilder"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/>。</exception>
        public BindingPathBuilder AppendProperty<T>(string propertyName)
        {
            this.AppendValuePath(propertyName);
            this.ValueExpression = Expression.PropertyOrField(this.ValueExpression, propertyName);
            this.BindingValueProviders.Add(new BindingPropertyProvider<T>(propertyName));
            this.LastInstanceType = typeof(T);
            return this;
        }

        /// <summary>
        /// 添加一个用于数据绑定的索引属性或数组索引到当前数据绑定路径。
        /// </summary>
        /// <typeparam name="T">用于数据绑定的索引对应的值的类型。</typeparam>
        /// <param name="indices">设置数据绑定时应绑定到的索引。</param>
        /// <returns>当前 <see cref="BindingPathBuilder"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indices"/> 为 <see langword="null"/>。</exception>
        public BindingPathBuilder AppendIndex<T>(params object[] indices)
        {
            this.AppendValuePath("Item");
            var indicesExpr = Array.ConvertAll(indices, Expression.Constant);
            this.ValueExpression = this.LastInstanceType.IsArray ?
                Expression.ArrayAccess(this.ValueExpression, indicesExpr) :
                Expression.Property(this.ValueExpression, "Item", indicesExpr);
            this.BindingValueProviders.Add(new BindingIndexProvider<T>(indices));
            this.LastInstanceType = typeof(T);
            return this;
        }

        /// <summary>
        /// 清除当前数据绑定路径中的所有节点。
        /// </summary>
        /// <returns>当前 <see cref="BindingPathBuilder"/> 对象。</returns>
        public BindingPathBuilder Clear()
        {
            this.ValuePathBuilder.Clear();
            this.ValueExpression = this.InstanceExpression;
            this.BindingValueProviders.Clear();
            this.LastInstanceType = this.Instance.GetType();
            return this;
        }

        /// <summary>
        /// 根据当前已构建的数据绑定路径创建一个用于数据绑定的值 <see cref="BindingPath{T}"/>。
        /// </summary>
        /// <typeparam name="T">用于数据绑定的值的类型。</typeparam>
        /// <returns>使用当前数据绑定路径创建的 <see cref="BindingPath{T}"/> 对象。</returns>
        public BindingPath<T> MakeBindingValue<T>()
        {
            return new BindingPath<T>(this.Instance, this.ValuePath,
                this.ValueExpression, this.BindingValueProviders.ToArray());
        }

        /// <summary>
        /// 将具有指定名称的数据绑定路径节点添加到 <see cref="BindingPathBuilder.ValuePathBuilder"/> 中。
        /// </summary>
        /// <param name="propertyName">数据绑定路径节点的名称。</param>
        /// <returns>当前实例的 <see cref="BindingPathBuilder.ValuePathBuilder"/>。</returns>
        private StringBuilder AppendValuePath(string propertyName)
        {
            return (this.ValuePathBuilder.Length == 0) ?
                this.ValuePathBuilder.Append(propertyName) :
                this.ValuePathBuilder.Append(".").Append(propertyName);
        }
    }
}
