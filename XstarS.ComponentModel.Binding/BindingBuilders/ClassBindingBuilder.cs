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
    [Serializable]
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
        /// 不是含有公共或保护访问级别的构造函数的公共非密封类。</exception>
        public ClassBindingBuilder(bool bindableOnly)
            : base(bindableOnly)
        {
            if (!(typeof(T).IsClass && typeof(T).IsPublic && !typeof(T).IsSealed))
            {
                throw new ArgumentException("Not a public inheritable class.", nameof(T));
            }
            if (!typeof(T).GetConstructors(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic).Any(
                ctor => ctor.IsPublic || ctor.IsFamily))
            {
                throw new ArgumentException("No public or family constructor.", nameof(T));
            }
        }

        /// <summary>
        /// 构造用于数据绑定的动态类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的动态类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code>方法。</exception>
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
                t_source, t_source.GetInterfaces());

            // 生成构造函数。
            var t_source_ic_Constructors =
                from constructor in t_source.GetConstructors(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                where constructor.IsPublic || constructor.IsFamily
                select constructor;
            foreach (var t_source_ic_Constructor in t_source_ic_Constructors)
            {
                t_Bindable.DefineDefaultConstructor(t_source_ic_Constructor, t_source.IsAbstract);
            }

            // 生成 PropertyChanged 事件。
            var ie_PropertyChanged = t_source.GetDerivedAccessibleEvent(
                nameof(INotifyPropertyChanged.PropertyChanged));
            var im_OnPropertyChanged = (MethodInfo)null;
            if (!(ie_PropertyChanged is null) && ie_PropertyChanged.AddMethod.IsAbstract)
            {
                var if_PropertyChanged = t_Bindable.DefineDefaultEvent(ie_PropertyChanged, false).Field;
                im_OnPropertyChanged = t_Bindable.DefineOnPropertyChangedMethod(if_PropertyChanged);
            }
            else
            {
                im_OnPropertyChanged = (
                    from method in t_source.GetDerivedAccessibleMethods(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    where method.Name == "OnPropertyChanged"
                    && Enumerable.SequenceEqual(
                        Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                        new[] { typeof(string) })
                    && method.ReturnType == typeof(void)
                    && (method.IsPublic || method.IsFamily) && !method.IsAbstract
                    select method).FirstOrDefault();
                if (im_OnPropertyChanged is null)
                {
                    throw new MissingMethodException(typeof(T).FullName, "OnPropertyChanged");
                }
            }

            // 生成属性。
            foreach (var t_source_ip_Property in t_source.GetDerivedAccessibleProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (t_source_ip_Property.IsOverridable())
                {
                    // 索引器。
                    if (t_source_ip_Property.GetIndexParameters().Length != 0)
                    {
                        if (t_source_ip_Property.IsAbstract())
                        {
                            t_Bindable.DefineNotImplementedPropertyOrIndexer(t_source_ip_Property, false);
                        }
                    }
                    // 属性。
                    else
                    {
                        var isBindable = (t_source_ip_Property.GetCustomAttribute(typeof(BindableAttribute))
                            is BindableAttribute bindable) && bindable.Bindable;
                        if (this.BindableOnly && !isBindable)
                        {
                            t_Bindable.DefineDefaultProperty(t_source_ip_Property, false);
                        }
                        else
                        {
                            t_Bindable.DefineBindableProperty(t_source_ip_Property, im_OnPropertyChanged, false);
                        }
                    }
                }
            }

            // 生成其它事件。
            foreach (var t_source_ie_Event in t_source.GetDerivedAccessibleEvents(
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
            foreach (var t_source_im_Method in t_source.GetDerivedAccessibleMethods(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if (!t_source_im_Method.IsSpecialName && t_source_im_Method.IsAbstract)
                {
                    t_Bindable.DefineNotImplementedMethod(t_source_im_Method, false);
                }
            }

            return t_Bindable.CreateTypeInfo();
        }
    }
}
