using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个用于数据绑定的属性或字段。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的属性或字段的类型。</typeparam>
    public class BindingProperty<T> : BindingMember<T>
    {
        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的属性名称初始化 <see cref="BindingProperty{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingProperty(object instance, string propertyName)
            : base(instance, propertyName) { }

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
                var property = Expression.PropertyOrField(instance, this.PropertyName);
                var getValue = Expression.Lambda<Func<T>>(property);
                return getValue.Compile();
            }
            catch (Exception)
            {
                throw new MissingMemberException(this.Instance.GetType().ToString(), this.PropertyName);
            }
        }

        /// <summary>
        /// 用于构造设置数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的设置数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造设置数据绑定值的委托。</exception>
        protected override Action<T> BuildSetValue()
        {
            try
            {
                var instance = Expression.Constant(this.Instance);
                var newValue = Expression.Parameter(typeof(T));
                var property = Expression.PropertyOrField(instance, this.PropertyName);
                var assign = Expression.Assign(property, newValue);
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
