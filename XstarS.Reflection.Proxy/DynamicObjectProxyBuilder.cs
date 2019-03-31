using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从指定原型类型和代理委托动态构造代理派生类型及其实例的方法。
    /// </summary>
    public class DynamicObjectProxyBuilder : ProxyBuilderBase<object>
    {
        /// <summary>
        /// 用于 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的
        /// <see cref="OnInvokeHandler"/> 委托的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Guid, OnInvokeHandler> Handlers =
            new ConcurrentDictionary<Guid, OnInvokeHandler>();

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="DynamicObjectProxyBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象，应为非抽象非密封类。</param>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不为公共非抽象非密封类。</exception>
        internal DynamicObjectProxyBuilder(Type type)
        {
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new TypeAccessException();
            }
            this.PrototypeType = type;
            this.InitializeBaseMethods();
        }

        /// <summary>
        /// 代理类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 原型类型中所有可在程序集外部重写的方法。
        /// </summary>
        internal MethodInfo[] BaseMethods { get; private set; }

        /// <summary>
        /// 代理特性类型中方法对应的 <see cref="OnInvokeHandler"/> 代理委托的 <see cref="Guid"/>。
        /// </summary>
        internal Dictionary<MethodInfo, List<Guid>> MethodHandlerGuids { get; private set; }

        /// <summary>
        /// 代理特性类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        internal TypeBuilder CustomProxyBaseType { get; private set; }

        /// <summary>
        /// 代理特性类型的 <see cref="Type"/> 对象。
        /// </summary>
        internal Type ProxyBaseType { get; private set; }

        /// <summary>
        /// 用于构造代理类型的 <see cref="ObjectProxyBuilder"/> 对象。
        /// </summary>
        internal ObjectProxyBuilder InternalBuilder { get; private set; }

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicObjectProxyBuilder"/> 类的实例。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的
        /// <see cref="DynamicObjectProxyBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicObjectProxyBuilder Create(Type type) => new DynamicObjectProxyBuilder(type);

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicObjectProxyBuilder"/> 类的实例，
        /// 并将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的 <see cref="DynamicObjectProxyBuilder"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已添加到所有可重写方法。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicObjectProxyBuilder Create(Type type, OnInvokeHandler handler)
        {
            var builder = DynamicObjectProxyBuilder.Create(type);
            builder.AddOnInvoke(handler);
            return builder;
        }

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="DynamicObjectProxyBuilder"/> 类的实例，
        /// 并根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>以 <paramref name="type"/> 为原型类型的 <see cref="DynamicObjectProxyBuilder"/> 类的实例，
        /// 其中 <paramref name="handler"/> 代理委托已根据
        /// <paramref name="methodFilter"/> 的指示添加到可重写方法。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static DynamicObjectProxyBuilder Create(
            Type type, OnInvokeHandler handler, Predicate<MethodInfo> methodFilter)
        {
            var builder = DynamicObjectProxyBuilder.Create(type);
            builder.AddOnInvoke(handler, methodFilter);
            return builder;
        }

        /// <summary>
        /// 获取被分配了指定 <see cref="Guid"/> 的 <see cref="OnInvokeHandler"/> 委托。
        /// 此方法用于 <see cref="InvokeDelegateOnMethodInvokeAttribute"/> 特性的内部实现，请不要调用此方法。
        /// </summary>
        /// <param name="guid"><see cref="OnInvokeHandler"/> 委托对应的 <see cref="Guid"/>。</param>
        /// <returns>被分配了指定 <see cref="Guid"/> 的 <see cref="OnInvokeHandler"/> 委托。</returns>
        public static OnInvokeHandler GetHandler(Guid guid) => DynamicObjectProxyBuilder.Handlers[guid];

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="method">要添加代理委托的可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="method"/> 不为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
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
            DynamicObjectProxyBuilder.Handlers[handlerGuid] = handler;
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
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
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
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
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
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
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
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler)
        {
            this.AddOnInvoke(handler, method => true);
        }

        /// <summary>
        /// 初始化原型类型中可以重写的方法成员信息。
        /// </summary>
        private void InitializeBaseMethods()
        {
            var baseMethods = this.PrototypeType.GetAccessibleMethods().Where(
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

            this.ProxyBaseType = this.CustomProxyBaseType.CreateTypeInfo();
        }

        /// <summary>
        /// 以原型类型为基础，定义代理特性派生类型。
        /// </summary>
        private void DefineProxyBaseType()
        {
            var baseType = this.PrototypeType;

            // 定义动态程序集。
            var asmName = $"{baseType.ToString()}#{this.GetHashCode().ToString()}";
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
                $"{string.Join("-", typeNames)}" +
                (baseType.IsGenericType ? $"<{string.Join(",", genericArgumentNames)}>" : "") +
                $"(Dynamic#{this.GetHashCode().ToString()})";

            // 获取原型类型信息。
            bool isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseType.GetInterfaces() :
                baseType.GetInterfaces().Concat(new[] { baseType }).ToArray();

            // 定义动态类型。
            var objectProxyType = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public |
                TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                parent, interfaces);
            this.CustomProxyBaseType = objectProxyType;
        }

        /// <summary>
        /// 以原型类型中构造函数为基础定义代理特性类型中的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.PrototypeType;
            var objectProxyType = this.CustomProxyBaseType;

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
        private void DefineAttributesOverrideMethods(MethodInfo baseMethod)
        {
            var objectProxyType = this.CustomProxyBaseType;
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

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        protected override Type BuildProxyType()
        {
            this.BuildProxyBaseType();

            this.InternalBuilder = ObjectProxyBuilder.OfType(this.ProxyBaseType);
            return this.InternalBuilder.ProxyType;
        }
    }
}
