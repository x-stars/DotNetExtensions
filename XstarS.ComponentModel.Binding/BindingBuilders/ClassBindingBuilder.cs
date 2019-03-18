using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型引用类型构造用于数据绑定的派生类型及其实例的内部实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型引用类型。</typeparam>
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
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共非密封类。</exception>
        internal ClassBindingBuilder(bool bindableOnly) : base(bindableOnly)
        {
            if (!(typeof(T).IsClass && typeof(T).IsPublic && !typeof(T).IsSealed))
            {
                throw new TypeAccessException();
            }
        }

        /// <summary>
        /// 构造用于数据绑定的派生类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type BuildType()
        {
            // 定义动态类型。
            var baseType = typeof(T);
            var asmName = $"{baseType.ToString()}({this.BindableOnly.ToString()})";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{asmName}.dll");
            var typeName = $"{baseType.Namespace}.Bindable{baseType.Name}" + (baseType.IsGenericType ?
                $"<{string.Join<Type>(",", baseType.GetGenericArguments())}>".Replace(".", "-") : "");
            var type = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.BeforeFieldInit,
                baseType, baseType.GetInterfaces());

            // 生成构造函数。
            var constructors = baseType.GetConstructors(BindingFlags.Instance |
                BindingFlags.Public | BindingFlags.NonPublic).Where(
                constructor => constructor.IsPublic || constructor.IsFamily);
            foreach (var constructor in constructors)
            {
                type.DefineDefaultConstructor(constructor);
            }

            // 生成 PropertyChanged 事件。
            var baseEventPropertyChanged = baseType.GetAccessibleEvents().Where(
                @event => @event.Name == nameof(INotifyPropertyChanged.PropertyChanged)).SingleOrDefault();
            var methodOnPropertyChanged = (MethodInfo)null;
            if (!(baseEventPropertyChanged is null) && baseEventPropertyChanged.AddMethod.IsAbstract)
            {
                var fieldPropertyChanged = type.DefineDefaultEvent(baseEventPropertyChanged, false).Field;
                methodOnPropertyChanged = type.DefineOnPropertyChangedMethod(fieldPropertyChanged);
            }
            else
            {
                methodOnPropertyChanged = (
                    from method in baseType.GetAccessibleMethods()
                    where method.Name == "OnPropertyChanged"
                    && Enumerable.SequenceEqual(
                        Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                        new[] { typeof(string) })
                    && method.ReturnType == typeof(void)
                    && method.IsInheritableInstance()
                    select method).SingleOrDefault();
                if (methodOnPropertyChanged is null)
                {
                    throw new MissingMethodException(typeof(T).ToString(), "OnPropertyChanged");
                }
            }

            // 生成属性。
            foreach (var baseProperty in baseType.GetAccessibleProperties().Where(
                property => property.GetAccessors(true).All(accessor => accessor.IsInheritableInstance())))
            {
                if (baseProperty.IsOverridable())
                {
                    // 索引器。
                    if (baseProperty.GetIndexParameters().Length != 0)
                    {
                        if (baseProperty.IsAbstract())
                        {
                            type.DefineNotImplementedProperty(baseProperty, false);
                        }
                    }
                    // 属性。
                    else
                    {
                        if (this.BindableOnly && !(baseProperty.
                            GetCustomAttribute<BindableAttribute>()?.Bindable == true))
                        {
                            type.DefineDefaultProperty(baseProperty, false);
                        }
                        else
                        {
                            type.DefineBindableProperty(baseProperty, methodOnPropertyChanged, false);
                        }
                    }
                }
            }

            // 生成其它事件。
            foreach (var baseEvent in baseType.GetAccessibleEvents().Where(
                @event => @event.AddMethod.IsInheritableInstance()))
            {
                if (baseEvent.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    if (baseEvent.AddMethod.IsAbstract)
                    {
                        type.DefineDefaultEvent(baseEvent, false);
                    }
                }
            }

            // 生成方法，并指定其抛出 NotImplementedException 异常。
            foreach (var baseMethod in baseType.GetAccessibleMethods().Where(
                method => method.IsInheritableInstance()))
            {
                if (!baseMethod.IsSpecialName && baseMethod.IsAbstract)
                {
                    type.DefineNotImplementedMethod(baseMethod, false);
                }
            }

            return type.CreateTypeInfo();
        }
    }
}
