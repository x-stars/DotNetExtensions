using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型接口构造用于数据绑定的派生类型及其实例的内部实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型接口。</typeparam>
    internal class InterfaceBindingBuilder<T> : BindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="InterfaceBindingBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        internal InterfaceBindingBuilder() : this(false) { }

        /// <summary>
        /// 初始化 <see cref="InterfaceBindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        internal InterfaceBindingBuilder(bool bindableOnly) : base(bindableOnly)
        {
            if (!(typeof(T).IsInterface && typeof(T).IsPublic))
            {
                throw new TypeAccessException();
            }
        }

        /// <summary>
        /// 构造用于数据绑定的派生类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的派生类型。</returns>
        protected override Type BuildBindableType()
        {
            // 定义动态类型。
            var baseType = typeof(T);
            var asmName = $"{baseType.ToString()}({this.BindableOnly.ToString()})";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{asmName}.dll");
            var typeName = $"{baseType.Namespace}.Bindable{baseType.Name}" + (baseType.IsGenericType ?
                $"<{string.Join<Type>(",", baseType.GetGenericArguments())}>".Replace(".", "-") : "");
            var type = module.DefineType(typeName, TypeAttributes.Class |
                TypeAttributes.Public | TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                typeof(object), (new[] { baseType }).Concat(baseType.GetInterfaces()).ToArray());

            // 生成构造函数。
            type.DefineDefaultConstructor(MethodAttributes.Public |
                MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            // 生成 PropertyChanged 事件。
            var baseEventPropertyChanged = baseType.GetAccessibleEvents().Where(
                property => property.Name == nameof(INotifyPropertyChanged.PropertyChanged)).SingleOrDefault();
            var fieldPropertyChanged = type.DefineDefaultEvent(baseEventPropertyChanged, true).Field;
            var methodOnPropertyChanged = type.DefineOnPropertyChangedMethod(fieldPropertyChanged);

            // 生成属性。
            foreach (var baseProperty in baseType.GetAccessibleProperties())
            {
                // 索引器。
                if (baseProperty.GetIndexParameters().Length != 0)
                {
                    type.DefineNotImplementedProperty(baseProperty, true);
                }
                // 属性。
                else
                {
                    if (this.BindableOnly && !(baseProperty.
                        GetCustomAttribute<BindableAttribute>()?.Bindable == true))
                    {
                        type.DefineDefaultProperty(baseProperty, true);
                    }
                    else
                    {
                        type.DefineBindableProperty(baseProperty, methodOnPropertyChanged, true);
                    }
                }
            }

            // 生成其它事件。
            foreach (var baseEvent in baseType.GetAccessibleEvents())
            {
                if (baseEvent.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    type.DefineDefaultEvent(baseEvent, true);
                }
            }

            // 生成方法，并指定其抛出 NotImplementedException 异常。
            foreach (var baseMethod in baseType.GetAccessibleMethods())
            {
                if (!baseMethod.IsSpecialName)
                {
                    type.DefineNotImplementedMethod(baseMethod, true);
                }
            }

            return type.CreateTypeInfo();
        }
    }
}
