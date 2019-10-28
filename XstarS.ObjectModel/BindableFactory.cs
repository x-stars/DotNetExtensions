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
    /// 提供可绑定派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class BindableFactory<T> : BindableFactoryBase<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableFactory{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<BindableFactory<T>> LazyDefault =
            new Lazy<BindableFactory<T>>(() => new BindableFactory<T>());

        /// <summary>
        /// 提供可绑定派生类型的 <see cref="BindableFactory"/> 对象。
        /// </summary>
        private readonly BindableFactory InternalFactory;

        /// <summary>
        /// 初始化 <see cref="BindableFactory{T}"/> 类的新实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableFactory()
        {
            var type = typeof(T);

            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(T));
            }

            this.InternalFactory = BindableFactory.Default(type);
        }

        /// <summary>
        /// 初始化 <see cref="BindableFactory{T}"/> 类的新实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        private BindableFactory(Predicate<PropertyInfo> isBindable)
        {
            var type = typeof(T);

            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(T));
            }
            if (isBindable is null)
            {
                throw new ArgumentNullException(nameof(isBindable));
            }

            this.InternalFactory = BindableFactory.Custom(type, isBindable);
        }

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableFactory{T}"/> 类的实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <returns>一个将所有可重写属性设置为可绑定属性的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableFactory{T}"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableFactory<T> Default => BindableFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 创建一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableFactory{T}"/> 类的实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>一个将符合 <paramref name="isBindable"/> 条件的可重写属性设置为可绑定属性的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableFactory{T}"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        public static BindableFactory<T> Custom(Predicate<PropertyInfo> isBindable) =>
            new BindableFactory<T>(isBindable);

        /// <summary>
        /// 创建可绑定派生类型。
        /// </summary>
        /// <returns>创建的可绑定派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type CreateBindableType() => this.InternalFactory.BindableType;
    }

    /// <summary>
    /// 提供指定类型的可绑定派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    public sealed class BindableFactory : BindableFactoryBase<object>
    {
        /// <summary>
        /// <see cref="BindableFactory.Default(Type)"/> 的延迟初始化值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindableFactory>> LazyDefaults =
            new ConcurrentDictionary<Type, Lazy<BindableFactory>>();

        /// <summary>
        /// 可绑定派生类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder BindableTypeBuilder;

        /// <summary>
        /// <code>void OnPropertyChanged(string)</code> 方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        private MethodInfo OnPropertyChangedMethod;

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindableFactory"/> 类的新实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        private BindableFactory(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(type));
            }

            this.BaseType = type;
        }

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindableFactory"/> 类的新实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 或
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        private BindableFactory(Type type, Predicate<PropertyInfo> isBindable)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(type));
            }
            if (isBindable is null)
            {
                throw new ArgumentNullException(nameof(isBindable));
            }

            this.BaseType = type;
            this.IsBindable = isBindable;
        }

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// 用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。
        /// </summary>
        internal Predicate<PropertyInfo> IsBindable { get; }

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindableFactory"/> 类的实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <returns>一个将所有可重写属性设置为可绑定属性的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableFactory"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static BindableFactory Default(Type type) =>
            BindableFactory.LazyDefaults.GetOrAdd(type,
                newType => new Lazy<BindableFactory>(
                    () => new BindableFactory(newType))).Value;

        /// <summary>
        /// 创建一个以指定类型为原型类型的 <see cref="BindableFactory"/> 类的实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>一个将符合 <paramref name="isBindable"/> 条件的可重写属性设置可绑定属性的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableFactory"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> 或
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        public static BindableFactory Custom(Type type, Predicate<PropertyInfo> isBindable) =>
            new BindableFactory(type, isBindable);

        /// <summary>
        /// 创建可绑定派生类型。
        /// </summary>
        /// <returns>创建的可绑定派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type CreateBindableType() => this.BuildBindableType();

        /// <summary>
        /// 构造可绑定派生类型。
        /// </summary>
        /// <returns>构造完成的可绑定派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        private Type BuildBindableType()
        {
            // 定义可绑定派生类型。
            this.DefineBindableType();

            // 定义可绑定派生类型的各成员。
            this.DefineConstructors();
            this.DefinePropertyChangedEvent();
            this.DefineProperties();
            this.DefineEvents();
            this.DefineMethods();

            // 完成类型创建。
            return this.BindableTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 定义可绑定派生类型。
        /// </summary>
        private void DefineBindableType()
        {
            var baseType = this.BaseType;
            var typeID = (this.IsBindable is null) ? "" : $"#{this.GetHashCode().ToString()}";

            // 定义动态程序集。
            var validBaseFullName = baseType.ToString().Replace("<", "[").Replace(">", "]");
            var asmName = $"Bindable[{validBaseFullName}]{typeID}";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(asmName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{asmName}.dll");

            // 生成类型名称。
            var baseNamespace = baseType.Namespace;
            var @namespace = !(baseNamespace is null) ? $"{baseNamespace}." : "";
            var baseTypeNames = new List<string>();
            for (var nestedType = baseType; !(nestedType is null); nestedType = nestedType.DeclaringType)
            {
                baseTypeNames.Insert(0, nestedType.Name);
            }
            var typeNames = baseTypeNames.ToArray();
            var typeName = string.Join("-", typeNames);
            var baseGenericArgumentNames = Array.ConvertAll(
                baseType.GetGenericArguments(), genericArgument => genericArgument.ToString());
            var genericArgumentNames = Array.ConvertAll(
                baseGenericArgumentNames, name => name.Replace('.', '-').Replace('+', '-'));
            var genericArguments = baseType.IsGenericType ?
                $"<{string.Join(",", genericArgumentNames)}>" : "";
            var fullName = $"{@namespace}<Bindable>{typeName}{genericArguments}{typeID}";

            // 获取原型类型信息。
            var baseInterfaces = baseType.GetInterfaces();
            var parent = !baseType.IsInterface ? baseType : typeof(object);
            var interfaces = !baseType.IsInterface ? baseInterfaces :
                baseInterfaces.Concat(new[] { baseType }).ToArray();
            interfaces = baseInterfaces.Contains(typeof(INotifyPropertyChanged)) ?
                interfaces : interfaces.Concat(new[] { typeof(INotifyPropertyChanged) }).ToArray();

            // 定义动态类型。
            var typeAttributes = TypeAttributes.Class |
                TypeAttributes.Public | TypeAttributes.BeforeFieldInit;
            var type = module.DefineType(fullName, typeAttributes, parent, interfaces);
            this.BindableTypeBuilder = type;
        }

        /// <summary>
        /// 定义可绑定派生类型的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.BaseType;
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
            var baseType = this.BaseType;
            var type = this.BindableTypeBuilder;

            var methodOnPropertyChanged = default(MethodInfo);
            // 未实现 INotifyPropertyChanged 接口。
            if (!baseType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)))
            {
                var baseEventPropertyChanged =
                    typeof(INotifyPropertyChanged).GetEvent(nameof(INotifyPropertyChanged.PropertyChanged));
                var fieldPropertyChanged = type.DefineDefaultEvent(baseEventPropertyChanged).Value;
                methodOnPropertyChanged = type.DefineOnPropertyChangedMethod(fieldPropertyChanged);
            }
            // 已实现 INotifyPropertyChanged 接口。
            else
            {
                var baseEventPropertyChanged = (
                    from @event in baseType.GetAccessibleEvents()
                    where @event.Name == nameof(INotifyPropertyChanged.PropertyChanged)
                    where @event.EventHandlerType == typeof(PropertyChangedEventHandler)
                    select @event).FirstOrDefault();
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
                        where method.GetParameters().Length == 1
                        where method.GetParameters()[0].ParameterType == typeof(string)
                        where method.ReturnType == typeof(void)
                        where method.IsInheritableInstance()
                        where !method.IsAbstract
                        select method).FirstOrDefault();
                    if (methodOnPropertyChanged is null)
                    {
                        throw new MissingMethodException(baseType.ToString(), "OnPropertyChanged");
                    }
                }
            }

            this.OnPropertyChangedMethod = methodOnPropertyChanged;
        }

        /// <summary>
        /// 定义可绑定派生类型的属性。
        /// </summary>
        private void DefineProperties()
        {
            var baseType = this.BaseType;
            var type = this.BindableTypeBuilder;
            var methodOnPropertyChanged = this.OnPropertyChangedMethod;

            foreach (var baseProperty in baseType.GetAccessibleProperties().Where(
                property => property.GetAccessors(true).All(accessor => accessor.IsInheritableInstance())))
            {
                if (baseProperty.GetAccessors(true).All(accessor => accessor.IsOverridable()))
                {
                    // 索引器（带参属性）。
                    if (baseProperty.GetIndexParameters().Length != 0)
                    {
                        if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                        {
                            type.DefineNotImplementedProperty(baseProperty);
                        }
                    }
                    // 属性（无参属性）。
                    else
                    {
                        // 默认可绑定类型。
                        if (this.IsBindable is null)
                        {
                            type.DefineBindableProperty(baseProperty, methodOnPropertyChanged);
                        }
                        // 自定义可绑定类型。
                        else
                        {
                            if (this.IsBindable(baseProperty))
                            {
                                type.DefineBindableProperty(baseProperty, methodOnPropertyChanged);
                            }
                            else
                            {
                                if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                                {
                                    type.DefineDefaultProperty(baseProperty);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 定义可绑定派生类型的事件。
        /// </summary>
        private void DefineEvents()
        {
            var baseType = this.BaseType;
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
        /// 定义可绑定派生类型的方法。
        /// </summary>
        private void DefineMethods()
        {
            var baseType = this.BaseType;
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
