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
    /// 提供基于 <see cref="INotifyPropertyChanged"/> 的属性更改通知类型。
    /// </summary>
    public sealed class ObservableTypeProvider
    {
        /// <summary>
        /// <see cref="ObservableTypeProvider.OfType(Type)"/> 的延迟初始化值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ObservableTypeProvider>> LazyOfTypes =
            new ConcurrentDictionary<Type, Lazy<ObservableTypeProvider>>();

        /// <summary>
        /// <see cref="ObservableTypeProvider.ObservableType"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Type> LazyObservableType;

        /// <summary>
        /// 属性更改通知类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder ObservableTypeBuilder;

        /// <summary>
        /// <c>void OnPropertyChanged(string)</c> 方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        private MethodInfo OnPropertyChangedMethod;

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
            if (!(((baseType.IsClass && !baseType.IsSealed) || baseType.IsInterface) &&
                baseType.IsVisible && !baseType.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseType));
            }

            this.BaseType = baseType;
            this.LazyObservableType = new Lazy<Type>(this.CreateObservableType);
        }

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// 获取属性更改通知类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <c>void OnPropertyChanged(string)</c> 方法。</exception>
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
        /// 创建属性更改通知类型。
        /// </summary>
        /// <returns>创建的属性更改通知类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <c>void OnPropertyChanged(string)</c> 方法。</exception>
        private Type CreateObservableType()
        {
            this.DefineObservableType();

            this.DefineObservableTypeConstructors();
            this.DefinePropertyChangedEvent();
            this.DefineObservableTypeProperties();
            this.DefineObservableTypeEvents();
            this.DefineObservableTypeMethods();

            return this.ObservableTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 定义属性更改通知类型。
        /// </summary>
        private void DefineObservableType()
        {
            var baseType = this.BaseType;

            var assemblyName = $"Observable[{baseType.ToString()}]";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{assemblyName}.dll");

            var baseNamespace = baseType.Namespace;
            var @namespace = !(baseNamespace is null) ? $"{baseNamespace}." : "";
            var baseTypeNames = new List<string>();
            for (var nestedType = baseType; !(nestedType is null); nestedType = nestedType.DeclaringType)
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
            var fullName = $"{@namespace}$Observable@{joinedTypeNames}{joinedGenericArgumentNames}";

            var baseInterfaces = baseType.GetInterfaces();
            var parent = !baseType.IsInterface ? baseType : typeof(object);
            var interfaces = !baseType.IsInterface ? baseInterfaces :
                baseInterfaces.Concat(new[] { baseType }).ToArray();
            interfaces = baseInterfaces.Contains(typeof(INotifyPropertyChanged)) ?
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
            var type = this.ObservableTypeBuilder;

            var baseConstructors = parent.GetConstructors().Where(
                constructor => constructor.IsInheritableInstance()).ToArray();
            foreach (var baseConstructor in baseConstructors)
            {
                type.DefineBaseInvokeConstructor(baseConstructor);
            }
        }

        /// <summary>
        /// 定义 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。
        /// </summary>
        private void DefinePropertyChangedEvent()
        {
            var baseType = this.BaseType;
            var type = this.ObservableTypeBuilder;

            var baseEvent = baseType.GetInterfaces().Contains(typeof(INotifyPropertyChanged)) ?
                baseType.GetAccessibleEvents().Where(@event =>
                    (@event.Name == nameof(INotifyPropertyChanged.PropertyChanged)) &&
                    (@event.EventHandlerType == typeof(PropertyChangedEventHandler))).FirstOrDefault() :
                typeof(INotifyPropertyChanged).GetEvent(nameof(INotifyPropertyChanged.PropertyChanged));

            if (baseEvent?.AddMethod.IsAbstract == true)
            {
                var field = type.DefineDefaultEventOverride(baseEvent).Value;
                var method = type.DefineOnPropertyChangedMethod(field);

                this.OnPropertyChangedMethod = method;
            }
            else
            {
                var method = baseType.GetAccessibleMethods().Where(method =>
                    (method.Name == "OnPropertyChanged") &&
                    (method.ReturnParameter.ParameterType == typeof(void)) &&
                    (method.GetParameters().Length == 1) &&
                    (method.GetParameters()[0].ParameterType == typeof(string)) &&
                    method.IsInheritableInstance() && !method.IsAbstract).FirstOrDefault();

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
            var type = this.ObservableTypeBuilder;
            var onPropertyChangedMethod = this.OnPropertyChangedMethod;

            foreach (var baseProperty in baseType.GetAccessibleProperties().Where(
                property => property.GetAccessors(true).All(accessor => accessor.IsInheritableInstance())))
            {
                if (baseProperty.GetAccessors(true).All(accessor => accessor.IsOverridable()))
                {
                    if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
                    {
                        if (baseProperty.GetIndexParameters().Length != 0)
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
            var type = this.ObservableTypeBuilder;

            foreach (var baseEvent in baseType.GetAccessibleEvents().Where(
                @event => @event.AddMethod.IsInheritableInstance()))
            {
                if (baseEvent.Name != nameof(INotifyPropertyChanged.PropertyChanged))
                {
                    if (baseEvent.AddMethod.IsAbstract)
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
            var type = this.ObservableTypeBuilder;

            foreach (var baseMethod in baseType.GetAccessibleMethods().Where(
                method => method.IsInheritableInstance()))
            {
                if (!baseMethod.IsSpecialName && baseMethod.IsAbstract)
                {
                    type.DefineNotImplementedMethodOverride(baseMethod);
                }
            }
        }
    }
}
