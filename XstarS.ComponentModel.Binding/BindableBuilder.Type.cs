using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从指定原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    public partial class BindableBuilder : BindableBuilderBase<object>
    {
        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindableBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型引用类型。</param>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindableBuilder(Type type) : this(type, false) { }

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindableBuilder"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型引用类型。</param>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindableBuilder(Type type, bool bindableOnly) : base(bindableOnly)
        {
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new TypeAccessException();
            }
            this.PrototypeType = type;
        }

        /// <summary>
        /// 用于数据绑定的类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于数据绑定的类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        internal TypeBuilder BindableDerivedType { get; private set; }

        /// <summary>
        /// <code>void OnPropertyChanged(string)</code> 方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal MethodInfo OnPropertyChangedMethod { get; private set; }

        /// <summary>
        /// 构造用于数据绑定的派生类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type BuildBindableType()
        {
            // 定义用于数据绑定的类型。
            this.DefineBindableType();

            // 定义用于数据绑定的类型的各成员。
            this.DefineConstructors();
            this.DefinePropertyChangedEvent();
            this.DefineProperties();
            this.DefineEvents();
            this.DefineMethods();

            // 完成类型创建。
            return this.BindableDerivedType.CreateTypeInfo();
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型。
        /// </summary>
        private void DefineBindableType()
        {
            var baseType = this.PrototypeType;

            // 定义动态程序集。
            var asmName = baseType.ToString() +
                $"({nameof(this.IsBindableOnly)}-{this.IsBindableOnly.ToString()})";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{asmName}.dll");

            // 生成类型名称。
            var baseNamespace = baseType.Namespace;
            var @namespace = baseNamespace;
            var baseTypeNames = new List<string>();
            for (var nestedType = baseType; !(nestedType is null); nestedType = nestedType.DeclaringType)
            {
                baseTypeNames.Insert(0, nestedType.Name);
            }
            var typeNames = baseTypeNames.ToArray();
            var baseGenericArgumentNames = Array.ConvertAll(
                baseType.GetGenericArguments(), genericArgument => genericArgument.ToString());
            var genericArgumentNames = Array.ConvertAll(
                baseGenericArgumentNames, name => name.Replace('.', '-').Replace('+', '-'));
            var typeName = (!(@namespace is null) ? $"{@namespace}." : "") +
                $"<Bindable>{string.Join("-", typeNames)}" +
                (baseType.IsGenericType ? $"<{string.Join(",", genericArgumentNames)}>" : "") +
                $"({nameof(this.IsBindableOnly)}-{this.IsBindableOnly.ToString()})";

            // 获取原型类型信息。
            var baseInterfaces = baseType.GetInterfaces();
            bool isInterface = baseType.IsInterface;
            var definedPropertyChanged = baseInterfaces.Contains(typeof(INotifyPropertyChanged));
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseInterfaces :
                baseInterfaces.Concat(new[] { baseType }).ToArray();
            interfaces = definedPropertyChanged ? interfaces :
                interfaces.Concat(new[] { typeof(INotifyPropertyChanged) }).ToArray();

            // 定义动态类型。
            var type = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public |
                TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                parent, interfaces);
            this.BindableDerivedType = type;
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.PrototypeType;
            var parent = !baseType.IsInterface ? baseType : typeof(object);
            var type = this.BindableDerivedType;

            var instanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var baseConstructors = parent.GetConstructors(instanceFlags).Where(
                constructor => constructor.IsInheritableInstance()).ToArray();
            foreach (var baseConstructor in baseConstructors)
            {
                type.DefineDefaultConstructor(baseConstructor);
            }
        }

        /// <summary>
        /// 定义 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。
        /// </summary>
        private void DefinePropertyChangedEvent()
        {
            var baseType = this.PrototypeType;
            var baseInterfaces = baseType.GetInterfaces();
            var definedPropertyChanged = baseInterfaces.Contains(typeof(INotifyPropertyChanged));
            var type = this.BindableDerivedType;

            var methodOnPropertyChanged = default(MethodInfo);
            // 未实现 INotifyPropertyChanged 接口。
            if (!definedPropertyChanged)
            {
                var baseEventPropertyChanged =
                    typeof(INotifyPropertyChanged).GetEvent(nameof(INotifyPropertyChanged.PropertyChanged));
                var fieldPropertyChanged = type.DefineDefaultEvent(baseEventPropertyChanged).Value;
                methodOnPropertyChanged = type.DefineOnPropertyChangedMethod(fieldPropertyChanged);
            }
            // 已实现 INotifyPropertyChanged 接口。
            else
            {
                var baseEventPropertyChanged = baseType.GetAccessibleEvents().Where(
                    @event => @event.Name == nameof(INotifyPropertyChanged.PropertyChanged)).SingleOrDefault();
                // 未实现 PropertyChanged 事件。
                if (!(baseEventPropertyChanged is null) && baseEventPropertyChanged.AddMethod.IsAbstract)
                {
                    var fieldPropertyChanged = type.DefineDefaultEvent(baseEventPropertyChanged).Value;
                    methodOnPropertyChanged = type.DefineOnPropertyChangedMethod(fieldPropertyChanged);
                }
                // 已实现 PropertyChanged 事件。
                else
                {
                    methodOnPropertyChanged = (
                        from method in baseType.GetAccessibleMethods()
                        where method.Name == "OnPropertyChanged"
                        && Enumerable.SequenceEqual(
                            Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                            new[] { typeof(string) })
                        && method.ReturnType == typeof(void)
                        && method.IsInheritableInstance() && !method.IsAbstract
                        select method).SingleOrDefault();
                    if (methodOnPropertyChanged is null)
                    {
                        throw new MissingMethodException(baseType.ToString(), "OnPropertyChanged");
                    }
                }
            }
            this.OnPropertyChangedMethod = methodOnPropertyChanged;
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型的属性。
        /// </summary>
        private void DefineProperties()
        {
            var baseType = this.PrototypeType;
            var type = this.BindableDerivedType;
            var methodOnPropertyChanged = this.OnPropertyChangedMethod;

            foreach (var baseProperty in baseType.GetAccessibleProperties().Where(
                property => property.GetAccessors(true).All(accessor => accessor.IsInheritableInstance())))
            {
                if (baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
                {
                    // 索引器。
                    if (baseProperty.GetIndexParameters().Length != 0)
                    {
                        if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                        {
                            type.DefineNotImplementedProperty(baseProperty);
                        }
                    }
                    // 属性。
                    else
                    {
                        var bindableAttribute = baseProperty.GetCustomAttribute<BindableAttribute>();
                        bool isBindable = !(bindableAttribute is null) && bindableAttribute.Bindable;
                        if (this.IsBindableOnly && !isBindable)
                        {
                            if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                            {
                                type.DefineDefaultProperty(baseProperty);
                            }
                        }
                        else
                        {
                            type.DefineBindableProperty(baseProperty, methodOnPropertyChanged);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型的事件。
        /// </summary>
        private void DefineEvents()
        {
            var baseType = this.PrototypeType;
            var type = this.BindableDerivedType;

            foreach (var baseEvent in baseType.GetAccessibleEvents().Where(
                @event => @event.AddMethod.IsInheritableInstance()))
            {
                if (baseEvent.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    if (baseEvent.AddMethod.IsAbstract)
                    {
                        type.DefineDefaultEvent(baseEvent);
                    }
                }
            }
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型的方法。
        /// </summary>
        private void DefineMethods()
        {
            var baseType = this.PrototypeType;
            var type = this.BindableDerivedType;

            foreach (var baseMethod in baseType.GetAccessibleMethods().Where(
                method => method.IsInheritableInstance()))
            {
                if (!baseMethod.IsSpecialName && baseMethod.IsAbstract)
                {
                    type.DefineNotImplementedMethod(baseMethod);
                }
            }
        }
    }
}
