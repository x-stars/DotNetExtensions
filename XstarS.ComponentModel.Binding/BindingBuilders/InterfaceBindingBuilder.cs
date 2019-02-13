using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从接口类型的原型构造用于数据绑定的实例的内部基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的实例的原型类型。</typeparam>
    [Serializable]
    internal class InterfaceBindingBuilder<T> : BindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="InterfaceBindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        public InterfaceBindingBuilder(bool bindableOnly)
            : base(bindableOnly)
        {
            if (!(typeof(T).IsInterface && typeof(T).IsPublic))
            {
                throw new ArgumentException("Not a public interface.", nameof(T));
            }
        }

        /// <summary>
        /// 构造用于数据绑定的动态类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的动态类型。</returns>
        protected override Type BuildType()
        {
            // 定义动态类型。
            var t_source = typeof(T);
            var asmName = t_source.ToString() + this.BindableOnly.ToString();
            var a_Bindable = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var m_Bindable = a_Bindable.DefineDynamicModule($"{asmName}.dll");
            var t_Bindable = m_Bindable.DefineType($"Bindable{t_source.Name}",
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
                null, (new[] { t_source }).Concat(t_source.GetInterfaces()).ToArray());

            // 生成构造函数。
            t_Bindable.DefineDefaultConstructor(MethodAttributes.Public);

            // 生成 PropertyChanged 事件。
            var if_PropertyChanged = t_Bindable.DefineDefaultEvent(
                t_source.GetDerivedAccessibleEvent(nameof(INotifyPropertyChanged.PropertyChanged)), true).Field;
            var im_OnPropertyChanged = t_Bindable.DefineOnPropertyChangedMethod(if_PropertyChanged);

            // 生成属性。
            foreach (var t_source_ip_Property in t_source.GetDerivedAccessibleProperties())
            {
                // 索引器。
                if (t_source_ip_Property.GetIndexParameters().Length != 0)
                {
                    t_Bindable.DefineNotImplementedPropertyOrIndexer(t_source_ip_Property, true);
                }
                // 属性。
                else
                {
                    var isBindable = (t_source_ip_Property.GetCustomAttribute(typeof(BindableAttribute))
                        is BindableAttribute bindable) && bindable.Bindable;
                    if (this.BindableOnly && !isBindable)
                    {
                        t_Bindable.DefineDefaultProperty(t_source_ip_Property, true);
                    }
                    else
                    {
                        t_Bindable.DefineBindableProperty(t_source_ip_Property, im_OnPropertyChanged, true);
                    }
                }
            }

            // 生成其它事件。
            foreach (var t_source_ie_Event in t_source.GetDerivedAccessibleEvents())
            {
                if (t_source_ie_Event.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    t_Bindable.DefineDefaultEvent(t_source_ie_Event, true);
                }
            }

            // 生成方法，并指定其抛出 NotImplementedException 异常。
            foreach (var t_source_im_Method in t_source.GetDerivedAccessibleMethods())
            {
                if (!t_source_im_Method.IsSpecialName)
                {
                    t_Bindable.DefineNotImplementedMethod(t_source_im_Method, true);
                }
            }

            return t_Bindable.CreateTypeInfo();
        }
    }
}
