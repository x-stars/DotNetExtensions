using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供代理派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ProxyFactory<T> : ProxyFactoryBase<T> where T : class
    {
        /// <summary>
        /// <see cref="ProxyFactory{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ProxyFactory<T>> LazyDefault =
            new Lazy<ProxyFactory<T>>(() => new ProxyFactory<T>());

        /// <summary>
        /// 提供代理派生类型的 <see cref="ProxyFactory"/> 对象。
        /// </summary>
        private readonly ProxyFactory InternalFactory;

        /// <summary>
        /// 初始化 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private ProxyFactory()
        {
            var type = typeof(T);

            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(T));
            }

            this.InternalFactory = ProxyFactory.Default(type);
        }

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyFactory{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyFactory<T> Default => ProxyFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 创建代理派生类型。
        /// </summary>
        /// <returns>创建的代理派生类型。</returns>
        protected override Type CreateProxyType() => this.InternalFactory.ProxyType;
    }

    /// <summary>
    /// 提供指定类型的代理派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    public sealed partial class ProxyFactory : ProxyFactoryBase<object>
    {
        /// <summary>
        /// <see cref="ProxyFactory.Default(Type)"/> 的延迟初始化值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ProxyFactory>> LazyDefaults =
            new ConcurrentDictionary<Type, Lazy<ProxyFactory>>();

        /// <summary>
        /// 原型类型中所有可在程序集外部重写的方法。
        /// </summary>
        private MethodInfo[] BaseMethods;

        /// <summary>
        /// 代理派生类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder ProxyTypeBuilder;

        /// <summary>
        /// 代理派生类型中用于访问原型类型中对应方法的方法。
        /// </summary>
        private Dictionary<MethodInfo, MethodInfo> BaseAccessMethods;

        /// <summary>
        /// 代理派生类型中用于保存原型类型中定义的 <see cref="OnMemberInvokeAttribute"/> 特性的字段。
        /// </summary>
        private FieldInfo[] OnMemberInvokeFields;

        /// <summary>
        /// 代理派生类型中用于保存原型类型中定义的 <see cref="OnMethodInvokeAttribute"/> 特性的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo[]> MethodsOnMethodInvokeFields;

        /// <summary>
        /// 代理派生类型中对应原型类型中方法的代理方法的 <see cref="MethodInvoker"/> 委托的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> ProxyDelegateFields;

        /// <summary>
        /// 以原型类型初始化 <see cref="ProxyFactory"/> 类的新实例。
        /// </summary>
        /// <param name="type">原型类型，应为接口或非密封类。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        private ProxyFactory(Type type)
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
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// 获取以指定类型为原型类型的 <see cref="ProxyFactory"/> 类的实例。
        /// </summary>
        /// <param name="type">原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ProxyFactory"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static ProxyFactory Default(Type type) =>
            ProxyFactory.LazyDefaults.GetOrAdd(type,
                newType => new Lazy<ProxyFactory>(
                    () => new ProxyFactory(newType))).Value;

        /// <summary>
        /// 创建代理派生类型。
        /// </summary>
        /// <returns>创建的派生类型。</returns>
        protected override Type CreateProxyType() => this.BuildProxyType();

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        private Type BuildProxyType()
        {
            // 初始化原型类型中可以被重写的方法。
            this.InitializeBaseMethods();

            // 定义代理派生类型。
            this.DefineProxyType();

            // 定义代理派生类型的各个成员。
            this.DefineConstructors();
            this.DefineBaseAccessMethods();
            this.DefineAttributesType();
            this.DefineDelegatesType();
            this.DefineOverrideMethods();

            // 完成类型创建。
            return this.ProxyTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 初始化原型类型中可以重写的方法成员信息。
        /// </summary>
        private void InitializeBaseMethods()
        {
            this.BaseMethods = this.BaseType.GetAccessibleMethods().Where(
                baseMethod => baseMethod.IsOverridable()).ToArray();
        }

        /// <summary>
        /// 以原型类型为基础，定义代理派生类型。
        /// </summary>
        private void DefineProxyType()
        {
            var baseType = this.BaseType;

            // 定义动态程序集。
            var asmName = $"{baseType.ToString()}(Proxy)";
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
                $"<Proxy>{string.Join("-", typeNames)}" +
                (baseType.IsGenericType ? $"<{string.Join(",", genericArgumentNames)}>" : "");

            // 获取原型类型信息。
            var isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseType.GetInterfaces() :
                baseType.GetInterfaces().Concat(new[] { baseType }).ToArray();

            // 定义动态类型。
            var proxyType = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public |
                TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                parent, interfaces);
            this.ProxyTypeBuilder = proxyType;
        }

        /// <summary>
        /// 以原型类型中构造函数为基础定义代理派生类型中的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.BaseType;
            var proxyType = this.ProxyTypeBuilder;

            var isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);

            var baseConstructors = parent.GetConstructors(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(
                constructor => constructor.IsPublic || constructor.IsFamily);
            foreach (var baseConstructor in baseConstructors)
            {
                proxyType.DefineDefaultConstructor(baseConstructor);
            }
        }

        /// <summary>
        /// 定义用于访问原型类型中对应方法的方法。
        /// </summary>
        private void DefineBaseAccessMethods()
        {
            var proxyType = this.ProxyTypeBuilder;
            var baseMethods = this.BaseMethods;

            var baseAccessMethods = new Dictionary<MethodInfo, MethodInfo>();
            for (int i = 0; i < baseMethods.Length; i++)
            {
                var baseMethod = baseMethods[i];
                var accessMethod = proxyType.DefineBaseAccessMethod(baseMethod);
                baseAccessMethods[baseMethod] = accessMethod;
            }
            this.BaseAccessMethods = baseAccessMethods;
        }

        /// <summary>
        /// 定义用于保存原型类型中定义的 <see cref="OnMethodInvokeAttribute"/> 特性的字段的类型。
        /// </summary>
        private void DefineAttributesType()
        {
            var attributesType = new AttributesStorage(this);
            this.OnMemberInvokeFields = attributesType.OnMemberInvokeFields;
            this.MethodsOnMethodInvokeFields = attributesType.MethodsOnMethodInvokeFields;
        }

        /// <summary>
        /// 定义包含代理方法和代理方法的 <see cref="MethodInvoker"/> 委托字段的类型。
        /// </summary>
        private void DefineDelegatesType()
        {
            var delegatesType = new DelegatesStorage(this);
            this.ProxyDelegateFields = delegatesType.ProxyDelegateFields;
        }

        /// <summary>
        /// 定义所有代理方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineOverrideMethods()
        {
            var baseMethods = this.BaseMethods;

            for (int i = 0; i < baseMethods.Length; i++)
            {
                var baseMethod = baseMethods[i];
                this.DefineProxyOverrideMethod(baseMethod);
            }
        }

        /// <summary>
        /// 定义调用代理方法的 <see cref="MethodInvoker"/> 委托的方法，并重写原型类型中的对应方法。
        /// </summary>
        /// <param name="baseMethod">
        /// 要调用代理方法的 <see cref="MethodInvoker"/> 委托的方法在原型类型中对应的方法。</param>
        private void DefineProxyOverrideMethod(MethodInfo baseMethod)
        {
            var proxyType = this.ProxyTypeBuilder;
            var proxyDelegateField = this.ProxyDelegateFields[baseMethod];
            var newSlot = baseMethod.DeclaringType.IsInterface;

            // 对于没有任何代理特性的方法则定义默认重写方法并返回。
            if (proxyDelegateField is null)
            {
                proxyType.DefineDefaultOverrideMethod(baseMethod);
                return;
            }

            // 定义方法。
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
            var hasReturns = baseReturnParam.ParameterType != typeof(void);
            var proxyMethod = proxyType.DefineMethod(baseMethod.Name,
                attributes, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            // 泛型参数。
            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() : proxyMethod.DefineGenericParameters(
                Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var methodGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericParameterAttributes(
                    methodGenericParam.GenericParameterAttributes);
            }
            if (baseGenericParams.Length != 0)
            {
                proxyDelegateField = TypeBuilder.GetField(
                    proxyDelegateField.ReflectedType.MakeGenericType(
                        proxyMethod.GetGenericArguments()), proxyDelegateField);
            }
            // 普通参数。
            var returnParam = proxyMethod.DefineParameter(0, baseReturnParam.Attributes, null);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = proxyMethod.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }
            // 生成 IL 代码。
            var ilGen = proxyMethod.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldsfld, proxyDelegateField);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Castclass, proxyType);
                ilGen.EmitLdcI4(baseParameters.Length);
                ilGen.Emit(OpCodes.Newarr, typeof(object));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    ilGen.Emit(OpCodes.Dup);
                    ilGen.EmitLdcI4(i);
                    ilGen.EmitLdarg(i + 1);
                    ilGen.Emit(OpCodes.Box, baseParameter.ParameterType);
                    ilGen.Emit(OpCodes.Stelem_Ref);
                }
                ilGen.Emit(OpCodes.Call, ReflectionData.T_MethodInvoker_IM_Invoke);
                if (hasReturns)
                {
                    ilGen.Emit(OpCodes.Unbox_Any, baseMethod.ReturnType);
                }
                else
                {
                    ilGen.Emit(OpCodes.Pop);
                }
                ilGen.Emit(OpCodes.Ret);
            }
        }
    }
}
