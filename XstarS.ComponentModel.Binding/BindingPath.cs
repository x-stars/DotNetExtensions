using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示从包含数据绑定值的实例到用于数据绑定的值的路径。
    /// 应使用 <see cref="BindingPathBuilder"/> 构造此类的实例。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的值的类型。</typeparam>
    public class BindingPath<T> : BindingValueBase<T>
    {
        /// <summary>
        /// <see cref="BindingPath{T}.Instance"/> 的值。
        /// </summary>
        private readonly object InternalInstance;

        /// <summary>
        /// <see cref="BindingPath{T}.ValuePath"/> 的值。
        /// </summary>
        private readonly string InternalValuePath;

        /// <summary>
        /// <see cref="BindingPath{T}.ValueExpression"/> 的值。
        /// </summary>
        private readonly Expression InternalValueExpression;

        /// <summary>
        /// <see cref="BindingPath{T}.BindingValueProviders"/> 的值。
        /// </summary>
        private readonly IBindingValueProvider[] InternalBindingValueProviders;

        /// <summary>
        /// <see cref="BindingPath{T}.BindingValues"/> 的值。
        /// </summary>
        private readonly IBindingValue[] InternalBindingValues;

        /// <summary>
        /// 使用包含数据绑定值的对象、数据绑定值路径和路径上各节点的
        /// <see cref="IBindingValue"/> 提供对象初始化 <see cref="BindingPath{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。</param>
        /// <param name="valuePath">从包含数据绑定值的对象到数据绑定值的路径。</param>
        /// <param name="valueExpression">访问数据绑定值的表达式树。</param>
        /// <param name="bindingMemberProviders">数据绑定路径上各节点的 <see cref="IBindingValue"/> 提供对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal BindingPath(object instance,
            string valuePath, Expression valueExpression,
            IBindingValueProvider[] bindingMemberProviders)
        {
            this.InternalInstance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.InternalValuePath = valuePath ??
                throw new ArgumentNullException(nameof(valuePath));
            this.InternalValueExpression = valueExpression ??
                throw new ArgumentNullException(nameof(valueExpression));
            this.InternalBindingValueProviders = bindingMemberProviders ??
                throw new ArgumentNullException(nameof(bindingMemberProviders));
            this.InternalBindingValues = new IBindingValue[this.InternalBindingValueProviders.Length];
            this.InitializeBindingValues();
        }

        /// <summary>
        /// 当前实例的 <see cref="IDisposable"/> 检查对象。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前实例已经被释放。</exception>
        protected new BindingPath<T> Disposable => (BindingPath<T>)base.Disposable;

        /// <summary>
        /// 包含数据绑定值的对象。
        /// </summary>
        public object Instance => this.Disposable.InternalInstance;

        /// <summary>
        /// 从包含数据绑定值的对象到数据绑定值的路径。
        /// </summary>
        public string ValuePath => this.Disposable.InternalValuePath;

        /// <summary>
        /// 访问数据绑定值的表达式树。
        /// </summary>
        private Expression ValueExpression => this.Disposable.InternalValueExpression;

        /// <summary>
        /// 数据绑定路径上各节点的 <see cref="IBindingValue"/> 提供对象。
        /// </summary>
        private IBindingValueProvider[] BindingValueProviders => this.Disposable.InternalBindingValueProviders;

        /// <summary>
        /// 从包含数据绑定值的对象依次向下直到数据绑定值的 <see cref="IBindingValue"/> 对象。
        /// </summary>
        private IBindingValue[] BindingValues => this.Disposable.InternalBindingValues;

        /// <summary>
        /// 构造获取数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的获取数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造获取数据绑定值的委托。</exception>
        protected override Func<T> BuildGetValue()
        {
            try
            {
                var getValue = Expression.Lambda<Func<T>>(this.ValueExpression);
                return getValue.Compile();
            }
            catch (Exception)
            {
                throw new MissingMemberException(this.Instance.GetType().ToString(), this.ValuePath);
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
                var newValue = Expression.Parameter(typeof(T));
                var assign = Expression.Assign(this.ValueExpression, newValue);
                var setValue = Expression.Lambda<Action<T>>(assign, newValue);
                return setValue.Compile();
            }
            catch (Exception)
            {
                throw new MissingMemberException(this.Instance.GetType().ToString(), this.ValuePath);
            }
        }

        /// <summary>
        /// 释放此实例占用的非托管资源。并根据指示释放托管资源。
        /// 将取消当前实例对 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的订阅。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    foreach (var bindingValue in this.BindingValues)
                    {
                        if (!(bindingValue is null))
                        {
                            bindingValue.ValueChanged -= this.BindingValue_ValueChanged;
                            bindingValue.Dispose();
                        }
                    }
                }

                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// 初始化从包含数据绑定值的对象依次向下直到数据绑定值的 <see cref="IBindingValue"/> 对象。
        /// </summary>
        private void InitializeBindingValues()
        {
            var instance = this.Instance;
            for (int i = 0; i < this.BindingValues.Length; i++)
            {
                if (instance is null) { break; }
                this.BindingValues[i] = this.BindingValueProviders[i].GetBindingValue(instance);
                this.BindingValues[i].ValueChanged += this.BindingValue_ValueChanged;
                instance = this.BindingValues[i].Value;
            }
        }

        /// <summary>
        /// 在绑定路径中某一节点的值改变时，更新 <see cref="BindingPath{T}.BindingValues"/>。
        /// </summary>
        /// <param name="valueChanged">值发生更改的 <see cref="IBindingValue"/> 对象。</param>
        private void UpdateBindingValues(object valueChanged)
        {
            bool changed = false;
            for (int i = 0; i < this.BindingValues.Length; i++)
            {
                if (changed)
                {
                    this.BindingValues[i]?.Dispose();
                    var instance = this.BindingValues[i - 1]?.Value;
                    if (instance is null)
                    {
                        this.BindingValues[i] = null;
                    }
                    else
                    {
                        this.BindingValues[i] = this.BindingValueProviders[i].GetBindingValue(instance);
                        this.BindingValues[i].ValueChanged += this.BindingValue_ValueChanged;
                    }
                }
                if (this.BindingValues[i] == valueChanged)
                {
                    changed = true;
                }
            }
        }

        /// <summary>
        /// <see cref="BindingPath{T}.BindingValues"/> 的
        /// <see cref="IBindingValue.ValueChanged"/> 事件的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void BindingValue_ValueChanged(object sender, EventArgs e)
        {
            this.UpdateBindingValues(sender);
            this.OnValueChanged();
        }
    }
}
