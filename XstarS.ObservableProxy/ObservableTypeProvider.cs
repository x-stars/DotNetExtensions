using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection;
using XstarS.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供基于 <see cref="INotifyPropertyChanged"/> 的属性更改通知类型。
    /// </summary>
    public sealed class ObservableTypeProvider
    {
        /// <summary>
        /// 提供用于定义属性更改通知类型的动态程序集的模块。
        /// </summary>
        private static class ModuleProvider
        {
            /// <summary>
            /// 表示用于定义属性更改通知类型的动态程序集的模块。
            /// </summary>
            internal static readonly ModuleBuilder ObservableTypesModule =
                ModuleProvider.CreateObservableTypesModule();

            /// <summary>
            /// 定义属性更改通知类型所在的动态程序集的模块。
            /// </summary>
            /// <returns>用于定义属性更改通知类型的动态程序集的模块。</returns>
            private static ModuleBuilder CreateObservableTypesModule()
            {
                var assemblyName = typeof(ObservableTypeProvider).ToString();
                var assembly = AssemblyBuilder.DefineDynamicAssembly(
                    new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
                var module = assembly.DefineDynamicModule($"{assemblyName}.dll");
                return module;
            }
        }

        /// <summary>
        /// 表示 <see cref="ObservableTypeProvider.OfType(Type)"/> 的延迟初始化对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ObservableTypeProvider>> LazyOfTypes =
            new ConcurrentDictionary<Type, Lazy<ObservableTypeProvider>>();

        /// <summary>
        /// 表示 <see cref="ObservableTypeProvider.ObservableType"/> 的延迟初始化对象。
        /// </summary>
        private readonly Lazy<Type> LazyObservableType;

        /// <summary>
        /// 表示属性更改通知类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder? ObservableTypeBuilder;

        /// <summary>
        /// 表示 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法。
        /// </summary>
        private MethodInfo? OnPropertyChangedMethod;

        /// <summary>
        /// 使用指定的原型类型初始化 <see cref="ObservableTypeProvider"/> 类的新实例。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        private ObservableTypeProvider(Type baseType)
        {
            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }
            if (!(baseType.IsVisible && !baseType.IsSealed && !baseType.ContainsGenericParameters))
            {
                var inner = new TypeAccessException();
                throw new ArgumentException(inner.Message, nameof(baseType), inner);
            }

            this.BaseType = baseType;
            this.LazyObservableType = new Lazy<Type>(this.CreateObservableType);
        }

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public Type BaseType { get; }

        /// <summary>
        /// 获取属性更改通知类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>属性更改通知类型的 <see cref="Type"/> 对象。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义可访问的 <c>OnPropertyChanged</c> 触发方法。</exception>
        public Type ObservableType => this.LazyObservableType.Value;

        /// <summary>
        /// 获取原型类型为指定类型的 <see cref="ObservableTypeProvider"/> 类的实例。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <returns>原型类型为 <paramref name="baseType"/> 的
        /// <see cref="ObservableTypeProvider"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        public static ObservableTypeProvider OfType(Type baseType) =>
            ObservableTypeProvider.LazyOfTypes.GetOrAdd(baseType,
                newBaseType => new Lazy<ObservableTypeProvider>(
                    () => new ObservableTypeProvider(newBaseType))).Value;

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建属性更改通知类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个使用指定参数创建的属性更改通知类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="ObservableTypeProvider.ObservableType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ObservableTypeProvider.ObservableType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public object CreateObservableInstance(object?[]? arguments)
        {
            var observable = ((arguments is null) || (arguments.Length == 0)) ?
                Activator.CreateInstance(this.ObservableType)! :
                Activator.CreateInstance(this.ObservableType, arguments)!;
            return (INotifyPropertyChanged)observable;
        }

        /// <summary>
        /// 创建属性更改通知类型。
        /// </summary>
        /// <returns>创建的属性更改通知类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义可访问的 <c>OnPropertyChanged</c> 触发方法。</exception>
        private Type CreateObservableType()
        {
            this.DefineObservableType();

            this.DefineObservableTypeConstructors();
            this.DefinePropertyChangedEvent();
            this.DefineObservableTypeProperties();
            this.DefineObservableTypeEvents();
            this.DefineObservableTypeMethods();

            return this.ObservableTypeBuilder!.CreateTypeInfo()!;
        }

        /// <summary>
        /// 定义属性更改通知类型。
        /// </summary>
        private void DefineObservableType()
        {
            var baseType = this.BaseType;
            var module = ModuleProvider.ObservableTypesModule;

            var baseNamespace = baseType.Namespace;
            var @namespace = (baseNamespace is not null) ? $"{baseNamespace}." : "";
            var baseTypeNames = new List<string>();
            for (var nestedType = baseType; nestedType is not null; nestedType = nestedType.DeclaringType)
            {
                baseTypeNames.Insert(0, nestedType.Name);
            }
            var typeNames = baseTypeNames.ToArray();
            var joinedTypeNames = string.Join("-", typeNames);
            var baseGenericArgumentNames = Array.ConvertAll(
                baseType.GetGenericArguments(), genericArgument => genericArgument.ToString());
            var genericArgumentNames = Array.ConvertAll(
                baseGenericArgumentNames, name => name.Replace('.', '-').Replace('+', '-'));
            var joinedGenericArgumentNames = baseType.IsGenericType ?
                $"<{string.Join(",", genericArgumentNames)}>" : "";
            var fullName = $"{@namespace}$Observable@{joinedTypeNames}{joinedGenericArgumentNames}" +
                $"#{baseType.TypeHandle.Value.ToString()}";

            var baseIsInterface = baseType.IsInterface;
            var parent = !baseIsInterface ? baseType : typeof(object);
            var interfaces = !baseIsInterface ? baseType.GetInterfaces() :
                new[] { baseType }.Concat(baseType.GetInterfaces()).ToArray();
            interfaces = interfaces.Contains(typeof(INotifyPropertyChanged)) ?
                interfaces : interfaces.Concat(new[] { typeof(INotifyPropertyChanged) }).ToArray();

            var type = module.DefineType(fullName, TypeAttributes.Class |
                TypeAttributes.Public | TypeAttributes.BeforeFieldInit, parent, interfaces);

            this.ObservableTypeBuilder = type;
        }

        /// <summary>
        /// 定义属性更改通知类型的构造函数。
        /// </summary>
        private void DefineObservableTypeConstructors()
        {
            var baseType = this.BaseType;
            var parent = !baseType.IsInterface ? baseType : typeof(object);
            var type = this.ObservableTypeBuilder!;

            var baseConstructors = parent.GetConstructors().Where(
                constructor => constructor.IsInheritable()).ToArray();
            foreach (var baseConstructor in baseConstructors)
            {
                type.DefineBaseInvokeConstructorLike(baseConstructor);
            }
        }

        /// <summary>
        /// 定义 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。
        /// </summary>
        private void DefinePropertyChangedEvent()
        {
            var baseType = this.BaseType;
            var type = this.ObservableTypeBuilder!;

            var baseEvent = baseType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)) ?
                baseType.GetAccessibleEvents().Where(@event =>
                    (@event.Name == nameof(INotifyPropertyChanged.PropertyChanged)) &&
                    (@event.EventHandlerType == typeof(PropertyChangedEventHandler))).FirstOrDefault() :
                typeof(INotifyPropertyChanged).GetEvent(nameof(INotifyPropertyChanged.PropertyChanged));

            if (baseEvent?.AddMethod!.IsAbstract == true)
            {
                var field = type.DefineDefaultEventOverride(baseEvent).Field;
                var method = type.DefineOnPropertyChangedMethod(field);

                this.OnPropertyChangedMethod = method;
            }
            else
            {
                var method = baseType.GetAccessibleMethods().Where(method =>
                    (method.Name == "OnPropertyChanged") &&
                    (method.ReturnParameter.ParameterType == typeof(void)) &&
                    (method.GetParameters().Length == 1) &&
                    (method.GetParameters()[0].ParameterType == typeof(PropertyChangedEventArgs)) &&
                    (method.IsInheritable() && !method.IsAbstract)).FirstOrDefault();

                if (method is null)
                {
                    throw new MissingMethodException(baseType.ToString(), "OnPropertyChanged");
                }

                this.OnPropertyChangedMethod = method;
            }
        }

        /// <summary>
        /// 定义属性更改通知类型的属性。
        /// </summary>
        private void DefineObservableTypeProperties()
        {
            var baseType = this.BaseType;
            var type = this.ObservableTypeBuilder!;
            var onPropertyChangedMethod = this.OnPropertyChangedMethod!;

            var baseProperties = baseType.GetAccessibleProperties().Where(
                property => property.GetAccessors(true).All(accessor => accessor.IsInheritable()));
            foreach (var baseProperty in baseProperties)
            {
                if (baseProperty.GetAccessors(true).All(accessor => accessor.IsOverridable()))
                {
                    if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                    {
                        if (baseProperty.GetIndexParameters().Length != 0)
                        {
                            type.DefineNotImplementedPropertyOverride(baseProperty);
                        }
                        else if (baseProperty.PropertyType.IsNotBoxable())
                        {
                            type.DefineNotImplementedPropertyOverride(baseProperty);
                        }
                        else
                        {
                            type.DefineObservableAutoPropertyOverride(baseProperty, onPropertyChangedMethod);
                        }
                    }
                    else
                    {
                        type.DefineObservableBaseInvokePropertyOverride(baseProperty, onPropertyChangedMethod);
                    }
                }
            }
        }

        /// <summary>
        /// 定义属性更改通知类型的事件。
        /// </summary>
        private void DefineObservableTypeEvents()
        {
            var baseType = this.BaseType;
            var type = this.ObservableTypeBuilder!;

            var baseEvents = baseType.GetAccessibleEvents().Where(
                @event => @event.AddMethod!.IsInheritable());
            foreach (var baseEvent in baseEvents)
            {
                if ((baseEvent.Name != nameof(INotifyPropertyChanged.PropertyChanged)) &&
                    (baseEvent.EventHandlerType != typeof(PropertyChangedEventHandler)))
                {
                    if (baseEvent.AddMethod!.IsAbstract)
                    {
                        type.DefineNotImplementedEventOverride(baseEvent);
                    }
                }
            }
        }

        /// <summary>
        /// 定义属性更改通知类型的方法。
        /// </summary>
        private void DefineObservableTypeMethods()
        {
            var baseType = this.BaseType;
            var type = this.ObservableTypeBuilder!;

            var baseMethods = baseType.GetAccessibleMethods().Where(
                method => method.IsInheritable());
            foreach (var baseMethod in baseMethods)
            {
                if (!baseMethod.IsSpecialName && baseMethod.IsAbstract)
                {
                    type.DefineNotImplementedMethodOverride(baseMethod);
                }
            }
        }
    }
}
