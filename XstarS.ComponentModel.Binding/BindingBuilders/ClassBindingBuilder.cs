using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从引用类型的原型构造用于数据绑定的实例的内部基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的实例的原型类型。</typeparam>
    internal class ClassBindingBuilder<T> : BindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="ClassBindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="ArgumentException"><typeparamref name="T"/>
        /// 不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        public ClassBindingBuilder(bool bindableOnly)
            : base(bindableOnly)
        {
            if (!(typeof(T).IsClass && !typeof(T).IsSealed))
            {
                throw new ArgumentException("Not an inheritable class.", nameof(T));
            }
            if (!typeof(T).GetConstructors(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic).Any(ctor =>
                (ctor.GetParameters().Length == 0) && (ctor.IsPublic || ctor.IsFamily)))
            {
                throw new ArgumentException("No public or protected default constructor.", nameof(T));
            }
        }

        /// <summary>
        /// 构造用于数据绑定的动态类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的动态类型。</returns>
        protected override Type BuildType()
        {
            bool bindableOnly = this.BindableOnly;

            // 定义动态类型。
            var t_source = typeof(T);
            var asmName = t_source.ToString() + bindableOnly.ToString();
            var a_Bindable = AppDomain.CurrentDomain.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var m_Bindable = a_Bindable.DefineDynamicModule(asmName);
            var t_Bindable = m_Bindable.DefineType($"Bindable{t_source.Name}",
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
                t_source, t_source.GetInterfaces());

            // 生成构造器。
            t_Bindable.DefineDefaultConstructor(MethodAttributes.Public);

            // 生成 PropertyChanged 事件。
            var im_SetProperty = (MethodBuilder)null;
            if (t_source.GetEvent(nameof(INotifyPropertyChanged.PropertyChanged)).AddMethod.IsAbstract)
            {
                im_SetProperty = t_Bindable.DefinePropertyChangedEvent(false).SetPropertyMethod;
            }
            else
            {
                var t_source_im_OnPropertyChanged = (
                    from method in t_source.GetMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    where method.Name == "OnPropertyChanged"
                    && method.GetParameters().ToList().ConvertAll(
                        param => param.ParameterType).SequenceEqual(new[] { typeof(string) })
                    && (method.IsPublic || method.IsFamily)
                    select method).FirstOrDefault();
                if (t_source_im_OnPropertyChanged is null)
                {
                    throw new ArgumentException("Method OnPropertyChanged(string) undefined.", nameof(T));
                }
                im_SetProperty = t_Bindable.DefineSetPropertyMethod(t_source_im_OnPropertyChanged);
            }

            // 生成属性。
            foreach (var t_source_ip_Property in t_source.GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (t_source_ip_Property.IsOverridable())
                {
                    var isBindable = (t_source_ip_Property.GetCustomAttribute(typeof(BindableAttribute))
                        is BindableAttribute bindable) && bindable.Bindable;
                    if (bindableOnly && !isBindable)
                    {
                        t_Bindable.DefineDefaultProperty(t_source_ip_Property, false);
                    }
                    else
                    {
                        t_Bindable.DefineBindableProperty(t_source_ip_Property, im_SetProperty, false);
                    }
                }
            }

            // 生成其它事件。
            foreach (var t_source_ie_Event in t_source.GetEvents(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (t_source_ie_Event.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    if (t_source_ie_Event.AddMethod.IsAbstract)
                    {
                        t_Bindable.DefineDefaultEvent(t_source_ie_Event, false);
                    }
                }
            }

            // 生成方法，并指定其抛出 NotImplementedException 异常。
            foreach (var t_source_im_Method in t_source.GetMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!t_source_im_Method.IsSpecialName && t_source_im_Method.IsAbstract)
                {
                    t_Bindable.DefineNotImplementedMethod(t_source_im_Method, false);
                }
            }

            return t_Bindable.CreateType();
        }
    }
}
