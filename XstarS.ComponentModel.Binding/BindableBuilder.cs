using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从指定原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    public sealed class BindableBuilder : BindableBuilderBase<object>
    {
        /// <summary>
        /// 对所有可重写属性设置数据绑定的
        /// <see cref="BindableBuilder"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindableBuilder>>
            LazyDefaultOfTypes = new ConcurrentDictionary<Type, Lazy<BindableBuilder>>();

        /// <summary>
        /// 以指定原型类型初始化 <see cref="BindableBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型引用类型。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableBuilder(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new TypeAccessException();
            }

            this.PrototypeType = type;
        }

        /// <summary>
        /// 以指定原型类型和用于筛选可绑定属性的条件初始化 <see cref="BindableBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型引用类型。</param>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 或
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableBuilder(Type type, Predicate<PropertyInfo> isBindable)
            : this(type)
        {
            if (isBindable is null)
            {
                throw new ArgumentNullException(nameof(isBindable));
            }

            this.IsBindable = isBindable;
        }

        /// <summary>
        /// 用于数据绑定的类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 指示当前 <see cref="BindableBuilder"/> 是否用于构造默认的数据绑定派生类型。
        /// </summary>
        public bool IsDefault => this.IsBindable is null;

        /// <summary>
        /// 用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。
        /// </summary>
        private Predicate<PropertyInfo> IsBindable { get; }

        /// <summary>
        /// 用于数据绑定的类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder BindableTypeBuilder { get; set; }

        /// <summary>
        /// <code>void OnPropertyChanged(string)</code> 方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        private MethodInfo OnPropertyChangedMethod { get; set; }

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定对所有可重写属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <returns>一个对所有可重写属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder Default(Type type) =>
            BindableBuilder.LazyDefaultOfTypes.GetOrAdd(type,
                newType => new Lazy<BindableBuilder>(
                    () => new BindableBuilder(newType))).Value;

        /// <summary>
        /// 创建一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定对符合指定条件的可重写属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>一个对符合 <paramref name="isBindable"/> 条件的可重写属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 或
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder Custom(Type type, Predicate<PropertyInfo> isBindable) =>
            new BindableBuilder(type, isBindable);

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
            return this.BindableTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型。
        /// </summary>
        private void DefineBindableType()
        {
            var baseType = this.PrototypeType;
            var typeID = this.IsDefault ? "" : $"#{this.GetHashCode().ToString()}";

            // 定义动态程序集。
            var asmName = $"{baseType.ToString()}(Bindable{typeID})";
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
                (baseType.IsGenericType ? $"<{string.Join(",", genericArgumentNames)}>" : "") + typeID;

            // 获取原型类型信息。
            var baseInterfaces = baseType.GetInterfaces();
            var isInterface = baseType.IsInterface;
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
            this.BindableTypeBuilder = type;
        }

        /// <summary>
        /// 定义用于数据绑定的派生类型的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.PrototypeType;
            var parent = !baseType.IsInterface ? baseType : typeof(object);
            var type = this.BindableTypeBuilder;

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
            var type = this.BindableTypeBuilder;

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
            var type = this.BindableTypeBuilder;
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
                        if (!this.IsDefault && !this.IsBindable(baseProperty))
                        {
                            type.DefineDefaultProperty(baseProperty);
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
            var type = this.BindableTypeBuilder;

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
            var type = this.BindableTypeBuilder;

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
