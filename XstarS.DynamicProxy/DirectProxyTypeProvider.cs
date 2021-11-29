using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="MethodInvokeHandler"/> 的直接代理类型。
    /// </summary>
    public sealed class DirectProxyTypeProvider
    {
        /// <summary>
        /// 表示 <see cref="DirectProxyTypeProvider.OfType(Type)"/> 的延迟初始化对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<DirectProxyTypeProvider>> LazyOfTypes =
            new ConcurrentDictionary<Type, Lazy<DirectProxyTypeProvider>>();

        /// <summary>
        /// 表示用于定义代理类型的动态程序集的模块。
        /// </summary>
        private static readonly ModuleBuilder ProxyDynamicModule =
            DirectProxyTypeProvider.DefineProxyModule();

        /// <summary>
        /// 表示 <see cref="DirectProxyTypeProvider.ProxyType"/> 的延迟初始化对象。
        /// </summary>
        private readonly Lazy<Type> LazyProxyType;

        /// <summary>
        /// 表示 <see cref="DirectProxyTypeProvider.HandlerField"/> 的延迟初始化对象。
        /// </summary>
        private readonly Lazy<FieldInfo> LazyHandlerField;

        /// <summary>
        /// 表示原型类型中所有应按代理模式重写的方法。
        /// </summary>
        private MethodInfo[] BaseMethods;

        /// <summary>
        /// 表示原型类型中所有应按非代理模式重写的方法。
        /// </summary>
        private MethodInfo[] BaseNonMethods;

        /// <summary>
        /// 表示代理类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder ProxyTypeBuilder;

        /// <summary>
        /// 表示代理类型中所有访问原型类型方法的方法。
        /// </summary>
        private Dictionary<MethodInfo, MethodInfo> BaseInvokeMethods;

        /// <summary>
        /// 表示代理类型中所有原型类型方法的 <see cref="MethodDelegate"/> 委托的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodDelegateFields;

        /// <summary>
        /// 表示代理类型中存储原型类型方法的 <see cref="MethodInfo"/> 的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodInfoFields;

        /// <summary>
        /// 使用指定的原型类型初始化 <see cref="DirectProxyTypeProvider"/> 类的新实例。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        private DirectProxyTypeProvider(Type baseType)
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
            this.LazyProxyType = new Lazy<Type>(this.CreateProxyType);
            this.LazyHandlerField = new Lazy<FieldInfo>(this.FindHandlerField);
        }

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public Type BaseType { get; }

        /// <summary>
        /// 获取代理类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>代理类型的 <see cref="Type"/> 对象。</returns>
        public Type ProxyType => this.LazyProxyType.Value;

        /// <summary>
        /// 获取代理类型中的代理委托字段的 <see cref="FieldInfo"/> 对象。
        /// </summary>
        /// <returns>代理类型中的代理委托字段的 <see cref="FieldInfo"/> 对象。</returns>
        public FieldInfo HandlerField => this.LazyHandlerField.Value;

        /// <summary>
        /// 获取原型类型为指定类型的 <see cref="DirectProxyTypeProvider"/> 类的实例。
        /// </summary>
        /// <param name="baseType">原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="baseType"/> 的
        /// <see cref="DirectProxyTypeProvider"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        public static DirectProxyTypeProvider OfType(Type baseType) =>
            DirectProxyTypeProvider.LazyOfTypes.GetOrAdd(baseType,
                newBaseType => new Lazy<DirectProxyTypeProvider>(
                    () => new DirectProxyTypeProvider(newBaseType))).Value;

        /// <summary>
        /// 定义代理类型所在的动态程序集的模块。
        /// </summary>
        /// <returns>用于定义代理类型的动态程序集的模块。</returns>
        private static ModuleBuilder DefineProxyModule()
        {
            var assemblyName = typeof(DirectProxyTypeProvider).ToString();
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{assemblyName}.dll");
            return module;
        }

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="arguments">用于创建代理对象的参数数组。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个使用指定参数创建并使用指定代理委托的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="DirectProxyTypeProvider.BaseType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="DirectProxyTypeProvider.BaseType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public object CreateProxyInstance(object[] arguments, MethodInvokeHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            arguments ??= Array.Empty<object>();

            var proxy = (arguments.Length == 0) ?
                Activator.CreateInstance(this.ProxyType) :
                Activator.CreateInstance(this.ProxyType, arguments);
            this.HandlerField.SetValue(proxy, handler);
            return proxy;
        }

        /// <summary>
        /// 创建代理类型。
        /// </summary>
        /// <returns>创建的代理类型。</returns>
        private Type CreateProxyType()
        {
            this.InitializeBaseMethods();

            this.DefineProxyType();

            this.DefineProxyTypeConstructors();
            this.DefineBaseInvokeMethods();
            this.DefineBaseMethodInfoAndDelegateFields();
            this.DefineProxyOverrideMethods();
            this.DefineNonProxyOverrideMethods();

            return this.ProxyTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 搜寻代理类型中的代理委托字段。
        /// </summary>
        /// <returns>代理类型中的代理委托字段。</returns>
        private FieldInfo FindHandlerField()
        {
            return this.ProxyType.GetField("Handler",
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// 初始化原型类型中应该重写的方法。
        /// </summary>
        private void InitializeBaseMethods()
        {
            this.BaseMethods = this.BaseType.GetAccessibleMethods().Where(
                method => method.IsProxyOverride()).ToArray();
            this.BaseNonMethods = this.BaseType.GetAccessibleMethods().Where(
                method => method.IsNonProxyOverride()).ToArray();
        }

        /// <summary>
        /// 定义代理类型。
        /// </summary>
        private void DefineProxyType()
        {
            var baseType = this.BaseType;
            var module = DirectProxyTypeProvider.ProxyDynamicModule;

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
            var fullName = $"{@namespace}$DirectProxy@{joinedTypeNames}{joinedGenericArgumentNames}" +
                $"#{baseType.TypeHandle.Value.ToString()}";

            var baseIsInterface = baseType.IsInterface;
            var parent = !baseIsInterface ? baseType : typeof(object);
            var interfaces = !baseIsInterface ? baseType.GetInterfaces() :
                new[] { baseType }.Concat(baseType.GetInterfaces()).ToArray();

            var type = module.DefineType(fullName, TypeAttributes.Class |
                TypeAttributes.Public | TypeAttributes.BeforeFieldInit, parent, interfaces);

            this.ProxyTypeBuilder = type;
        }

        /// <summary>
        /// 定义代理类型的构造函数。
        /// </summary>
        private void DefineProxyTypeConstructors()
        {
            var baseType = this.BaseType;
            var type = this.ProxyTypeBuilder;

            var isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var baseConstructors = parent.GetConstructors().Where(
                constructor => constructor.IsInheritable()).ToArray();

            foreach (var baseConstructor in baseConstructors)
            {
                var constructor = type.DefineBaseInvokeConstructorLike(baseConstructor);
            }
        }

        /// <summary>
        /// 定义代理类型中访问原型类型方法的方法。
        /// </summary>
        private void DefineBaseInvokeMethods()
        {
            var baseType = this.BaseType;
            var type = this.ProxyTypeBuilder;
            var baseMethods = this.BaseMethods;

            var methods = new Dictionary<MethodInfo, MethodInfo>();
            for (int index = 0; index < baseMethods.Length; index++)
            {
                var baseMethod = baseMethods[index];
                var method = type.DefineBaseInvokeMethodLike(baseMethod, baseType);
                methods[baseMethod] = method;
            }

            this.BaseInvokeMethods = methods;
        }

        /// <summary>
        /// 定义所有原型类型方法的方法信息和方法委托字段。
        /// </summary>
        private void DefineBaseMethodInfoAndDelegateFields()
        {
            var baseType = this.BaseType;
            var baseMethods = this.BaseMethods;
            var type = this.ProxyTypeBuilder;
            var baseInvokeMethods = this.BaseInvokeMethods;

            var infoFields = new Dictionary<MethodInfo, FieldInfo>();
            var delegateFields = new Dictionary<MethodInfo, FieldInfo>();

            foreach (var baseMethod in baseMethods)
            {
                var baseInvokeMethod = baseInvokeMethods[baseMethod];
                var fields = type.DefineMethodInfoAndDelegateField(baseMethod, baseType, baseInvokeMethod);
                infoFields[baseMethod] = fields.Key;
                delegateFields[baseMethod] = fields.Value;
            }

            this.BaseMethodInfoFields = infoFields;
            this.BaseMethodDelegateFields = delegateFields;
        }

        /// <summary>
        /// 定义所有代理方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineProxyOverrideMethods()
        {
            var baseMethods = this.BaseMethods;
            var type = this.ProxyTypeBuilder;
            var baseMethodInfoFields = this.BaseMethodInfoFields;
            var baseMethodDelegateFields = this.BaseMethodDelegateFields;

            var handlerField = type.DefineField("Handler",
                typeof(MethodInvokeHandler), FieldAttributes.Assembly);

            foreach (var baseMethod in baseMethods)
            {
                var baseMethodInfoField = baseMethodInfoFields[baseMethod];
                var baseMethodDelegateField = baseMethodDelegateFields[baseMethod];
                var method = type.DefineProxyMethodOverride(baseMethod,
                    baseMethodInfoField, baseMethodDelegateField, handlerField, explicitOverride: true);
            }
        }

        /// <summary>
        /// 定义所有非代理方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineNonProxyOverrideMethods()
        {
            var baseMethods = this.BaseNonMethods;
            var type = this.ProxyTypeBuilder;

            foreach (var baseMethod in baseMethods)
            {
                var method = type.DefineNotImplementedMethodOverride(baseMethod, explicitOverride: true);
            }
        }
    }
}
