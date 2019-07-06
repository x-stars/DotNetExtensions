using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从代理委托构造代理派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ProxyTypeBuilder<T> : ProxyTypeProviderBase<T> where T : class
    {
        /// <summary>
        /// 构造代理派生类型的 <see cref="ProxyTypeBuilder"/> 对象。
        /// </summary>
        private readonly ProxyTypeBuilder InternalBuilder;

        /// <summary>
        /// 初始化 <see cref="ProxyTypeBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public ProxyTypeBuilder()
        {
            this.InternalBuilder = new ProxyTypeBuilder(typeof(T));
        }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="method">要添加代理委托的可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="method"/> 不为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, MethodInfo method) =>
            this.InternalBuilder.AddOnInvoke(handler, method);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, params MethodInfo[] methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, IEnumerable<MethodInfo> methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, Predicate<MethodInfo> methodFilter) =>
            this.InternalBuilder.AddOnInvoke(handler, methodFilter);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler) =>
            this.InternalBuilder.AddOnInvoke(handler);

        /// <summary>
        /// 创建代理派生类型。
        /// </summary>
        /// <returns>创建的代理派生类型。</returns>
        protected override Type CreateProxyType() => this.InternalBuilder.ProxyType;
    }

    /// <summary>
    /// 提供从指定原型类型和代理委托构造代理派生类型及其实例的方法。
    /// </summary>
    public sealed class ProxyTypeBuilder : ProxyTypeProviderBase<object>
    {
        /// <summary>
        /// 用于 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的
        /// <see cref="OnInvokeHandler"/> 委托的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Guid, OnInvokeHandler> Handlers =
            new ConcurrentDictionary<Guid, OnInvokeHandler>();

        /// <summary>
        /// 原型类型中所有可在程序集外部重写的方法。
        /// </summary>
        private MethodInfo[] BaseMethods;

        /// <summary>
        /// 代理特性派生类型中方法对应的 <see cref="OnInvokeHandler"/> 代理委托的 <see cref="Guid"/>。
        /// </summary>
        private IDictionary<MethodInfo, List<Guid>> MethodHandlerGuids;

        /// <summary>
        /// 代理特性派生类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder ProxyBaseTypeBuilder;

        /// <summary>
        /// 代理特性派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        private Type ProxyBaseType;

        /// <summary>
        /// 提供代理派生类型的 <see cref="ProxyTypeProvider"/> 对象。
        /// </summary>
        private ProxyTypeProvider InternalProvider;

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="ProxyTypeBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public ProxyTypeBuilder(Type type)
        {
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new TypeAccessException();
            }

            this.BaseType = type;
            this.InitializeBaseMethods();
        }

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// 获取被分配了指定 <see cref="Guid"/> 的 <see cref="OnInvokeHandler"/> 委托。
        /// 此方法用于 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的内部实现，请不要调用此方法。
        /// </summary>
        /// <param name="guid"><see cref="OnInvokeHandler"/> 委托对应的 <see cref="Guid"/>。</param>
        /// <returns>被分配了指定 <see cref="Guid"/> 的 <see cref="OnInvokeHandler"/> 委托。</returns>
        public static OnInvokeHandler GetHandler(Guid guid) => ProxyTypeBuilder.Handlers[guid];

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="method">要添加代理委托的可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="method"/> 不为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, MethodInfo method)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }
            if (!this.BaseMethods.Contains(method))
            {
                throw new MethodAccessException();
            }
            if (!(this.ProxyBaseType is null))
            {
                throw new NotSupportedException();
            }

            var handlerGuid = Guid.NewGuid();
            ProxyTypeBuilder.Handlers[handlerGuid] = handler;
            this.MethodHandlerGuids[method].Add(handlerGuid);
        }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, params MethodInfo[] methods)
        {
            if (methods is null)
            {
                throw new ArgumentNullException(nameof(methods));
            }

            foreach (var method in methods)
            {
                this.AddOnInvoke(handler, method);
            }
        }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, IEnumerable<MethodInfo> methods)
        {
            if (methods is null)
            {
                throw new ArgumentNullException(nameof(methods));
            }

            foreach (var method in methods)
            {
                this.AddOnInvoke(handler, method);
            }
        }

        /// <summary>
        /// 根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, Predicate<MethodInfo> methodFilter)
        {
            if (methodFilter is null)
            {
                throw new ArgumentNullException(nameof(methodFilter));
            }

            foreach (var method in this.BaseMethods)
            {
                if (methodFilter(method))
                {
                    this.AddOnInvoke(handler, method);
                }
            }
        }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler)
        {
            this.AddOnInvoke(handler, method => true);
        }

        /// <summary>
        /// 创建代理派生类型。
        /// </summary>
        /// <returns>创建的代理派生类型。</returns>
        protected override Type CreateProxyType() => this.BuildProxyType();

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        private Type BuildProxyType()
        {
            this.BuildProxyBaseType();

            this.InternalProvider = ProxyTypeProvider.Default(this.ProxyBaseType);
            return this.InternalProvider.ProxyType;
        }

        /// <summary>
        /// 初始化原型类型中可以重写的方法成员信息。
        /// </summary>
        private void InitializeBaseMethods()
        {
            var baseMethods = this.BaseType.GetAccessibleMethods().Where(
                baseMethod => baseMethod.IsOverridable()).ToArray();
            this.BaseMethods = baseMethods;
            var methodHandlerGuids = baseMethods.ToDictionary(
                baseMethod => baseMethod, baseMethod => new List<Guid>());
            this.MethodHandlerGuids = methodHandlerGuids;
        }

        /// <summary>
        /// 构造代理特性派生类型。
        /// </summary>
        private void BuildProxyBaseType()
        {
            this.DefineProxyBaseType();

            this.DefineConstructors();
            this.DefineOverrideMethods();

            this.ProxyBaseType = this.ProxyBaseTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 定义代理特性派生类型。
        /// </summary>
        private void DefineProxyBaseType()
        {
            var baseType = this.BaseType;

            // 定义动态程序集。
            var asmName = $"{baseType.ToString()}(ProxyBase#{this.GetHashCode().ToString()})";
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
                $"<ProxyBase>{string.Join("-", typeNames)}" +
                (baseType.IsGenericType ? $"<{string.Join(",", genericArgumentNames)}>" : "") +
                $"#{this.GetHashCode().ToString()}";

            // 获取原型类型信息。
            bool isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseType.GetInterfaces() :
                baseType.GetInterfaces().Concat(new[] { baseType }).ToArray();

            // 定义动态类型。
            var customProxyBaseType = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Abstract |
                TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                parent, interfaces);
            this.ProxyBaseTypeBuilder = customProxyBaseType;
        }

        /// <summary>
        /// 定义代理特性派生类型的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.BaseType;
            var objectProxyType = this.ProxyBaseTypeBuilder;

            bool isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);

            var baseConstructors = parent.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(
                constructor => constructor.IsPublic || constructor.IsFamily);
            foreach (var baseConstructor in baseConstructors)
            {
                objectProxyType.DefineDefaultConstructor(baseConstructor);
            }
        }

        /// <summary>
        /// 定义所有添加代理特性的方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineOverrideMethods()
        {
            var baseMethods = this.BaseMethods;

            foreach (var baseMethod in baseMethods)
            {
                this.DefineAttributesOverrideMethods(baseMethod);
            }
        }

        /// <summary>
        /// 定义添加 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的方法，并重写原型类型中的对应方法。
        /// </summary>
        /// <param name="baseMethod">
        /// 要添加 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的方法在原型类型中对应的方法。</param>
        private void DefineAttributesOverrideMethods(MethodInfo baseMethod)
        {
            var objectProxyType = this.ProxyBaseTypeBuilder;
            var methodHandlerGuids = this.MethodHandlerGuids;

            var method = objectProxyType.DefineDefaultOverrideMethod(baseMethod);
            var handlerGuids = methodHandlerGuids[baseMethod];
            for (int i = handlerGuids.Count - 1; i >= 0; i--)
            {
                var handlerGuid = handlerGuids[i];
                var attribute = new CustomAttributeBuilder(
                    ReflectionData.T_InvokeDelegateOnMethodInvokeAttribute_IC_ctor,
                    new object[] { handlerGuid.ToString() });
                method.SetCustomAttribute(attribute);
            }
        }
    }
}
