using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="ProxyInvokeHandler"/> 的代理类型。
    /// </summary>
    public sealed class ProxyTypeProvider
    {
        /// <summary>
        /// <see cref="ProxyTypeProvider.OfType(Type)"/> 的延迟初始化值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ProxyTypeProvider>> LazyOfTypes =
            new ConcurrentDictionary<Type, Lazy<ProxyTypeProvider>>();

        /// <summary>
        /// <see cref="ProxyTypeProvider.ProxyType"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Type> LazyProxyType;

        /// <summary>
        /// <see cref="ProxyTypeProvider.HandlerField"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<FieldInfo> LazyHandlerField;

        /// <summary>
        /// 原型类型中所有可在程序集外部重写的方法。
        /// </summary>
        private MethodInfo[] BaseMethods;

        /// <summary>
        /// 代理类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        private TypeBuilder ProxyTypeBuilder;

        /// <summary>
        /// 代理类型中所有访问原型类型方法的方法。
        /// </summary>
        private Dictionary<MethodInfo, MethodInfo> BaseInvokeMethods;

        /// <summary>
        /// 代理类型中所有原型类型方法的 <see cref="MethodDelegate"/> 委托的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodDelegateFields;

        /// <summary>
        /// 代理类型中存储原型类型方法的 <see cref="MethodInfo"/> 的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodInfoFields;

        /// <summary>
        /// 代理类型中 <see cref="ProxyInvokeHandler"/> 字段的 <see cref="FieldInfo"/> 对象。
        /// </summary>
        private FieldInfo InternalHandlerField;

        /// <summary>
        /// 以原型类型初始化 <see cref="ProxyTypeProvider"/> 类的新实例。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        private ProxyTypeProvider(Type baseType)
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
            this.LazyProxyType = new Lazy<Type>(this.CreateProxyType);
            this.LazyHandlerField = new Lazy<FieldInfo>(this.FindHandlerField);
        }

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType { get; }

        /// <summary>
        /// 代理类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type ProxyType => this.LazyProxyType.Value;

        /// <summary>
        /// 代理类型中 <see cref="ProxyInvokeHandler"/> 字段的 <see cref="FieldInfo"/> 对象。
        /// </summary>
        public FieldInfo HandlerField => this.LazyHandlerField.Value;

        /// <summary>
        /// 获取以指定类型为原型类型的 <see cref="ProxyTypeProvider"/> 类的实例。
        /// </summary>
        /// <param name="baseType">原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="baseType"/> 的
        /// <see cref="ProxyTypeProvider"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
        public static ProxyTypeProvider OfType(Type baseType) =>
            ProxyTypeProvider.LazyOfTypes.GetOrAdd(baseType,
                newBaseType => new Lazy<ProxyTypeProvider>(
                    () => new ProxyTypeProvider(newBaseType))).Value;

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
            this.DefineBaseMethodStaticTypes();
            this.DefineProxyOverrideMethods();

            return this.ProxyTypeBuilder.CreateTypeInfo();
        }

        /// <summary>
        /// 搜寻代理类型中 <see cref="ProxyInvokeHandler"/> 字段。
        /// </summary>
        /// <returns>代理类型中 <see cref="ProxyInvokeHandler"/> 字段。</returns>
        private FieldInfo FindHandlerField()
        {
            return this.ProxyType.GetField(nameof(ProxyInvokeHandler),
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
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
        /// 定义代理类型。
        /// </summary>
        private void DefineProxyType()
        {
            var baseType = this.BaseType;

            var validBaseFullName = baseType.ToString().Replace("<", "[").Replace(">", "]");
            var assemblyName = $"Proxy[{validBaseFullName}]";
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
            var typeName = string.Join("-", typeNames);
            var baseGenericArgumentNames = Array.ConvertAll(
                baseType.GetGenericArguments(), genericArgument => genericArgument.ToString());
            var genericArgumentNames = Array.ConvertAll(
                baseGenericArgumentNames, name => name.Replace('.', '-').Replace('+', '-'));
            var genericArguments = baseType.IsGenericType ?
                $"<{string.Join(",", genericArgumentNames)}>" : "";
            var fullName = $"{@namespace}<Proxy>{typeName}{genericArguments}";

            var isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseType.GetInterfaces() :
                baseType.GetInterfaces().Concat(new[] { baseType }).ToArray();

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
                constructor => constructor.IsInheritableInstance()).ToArray();

            foreach (var baseConstructor in baseConstructors)
            {
                var constructor = type.DefineBaseInvokeConstructor(baseConstructor);
            }
        }

        /// <summary>
        /// 定义代理类型中访问原型类型方法的方法。
        /// </summary>
        private void DefineBaseInvokeMethods()
        {
            var type = this.ProxyTypeBuilder;
            var baseMethods = this.BaseMethods;

            var methods = new Dictionary<MethodInfo, MethodInfo>();
            for (int i = 0; i < baseMethods.Length; i++)
            {
                var baseMethod = baseMethods[i];
                var method = type.DefineBaseInvokeMethod(baseMethod);
                methods[baseMethod] = method;
            }

            this.BaseInvokeMethods = methods;
        }

        /// <summary>
        /// 定义所有原型类型方法的静态类型。
        /// </summary>
        private void DefineBaseMethodStaticTypes()
        {
            var baseMethods = this.BaseMethods;

            var delegateFields = new Dictionary<MethodInfo, FieldInfo>();
            var infoFields = new Dictionary<MethodInfo, FieldInfo>();

            this.BaseMethodDelegateFields = delegateFields;
            this.BaseMethodInfoFields = infoFields;

            foreach (var baseMethod in baseMethods)
            {
                this.DefineBaseMethodStaticType(baseMethod);
            }
        }

        /// <summary>
        /// 定义原型类型方法的静态类型。
        /// </summary>
        /// <param name="baseMethod">原型类型的方法。</param>
        private void DefineBaseMethodStaticType(MethodInfo baseMethod)
        {
            var baseType = this.BaseType;
            var proxyType = this.ProxyTypeBuilder;
            var baseInvokeMethod = this.BaseInvokeMethods[baseMethod];
            var delegateFields = this.BaseMethodDelegateFields;
            var methodInfoFields = this.BaseMethodInfoFields;

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var type = proxyType.DefineNestedType($"{baseMethod.ToNameHandleString()}",
                TypeAttributes.Class | TypeAttributes.NestedAssembly |
                TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                type.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericConstraintsAs(baseGenericParam);
            }

            var delegateMethod = type.DefineMethod(nameof(MethodDelegate.Invoke),
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(object), new[] { typeof(object), typeof(object[]) });
            {
                delegateMethod.DefineParameter(1, ParameterAttributes.None, "instance");
                delegateMethod.DefineParameter(2, ParameterAttributes.None, "arguments");

                var il = delegateMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Castclass, proxyType);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    int index = Array.IndexOf(
                        baseGenericParams, baseParameter.ParameterType);
                    var parameterType = (index == -1) ?
                        baseParameter.ParameterType : genericParams[index];
                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitLdcI4(i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.Emit(OpCodes.Unbox_Any, parameterType);
                }
                il.Emit(OpCodes.Call,
                    !baseInvokeMethod.IsGenericMethod ? baseInvokeMethod :
                    baseInvokeMethod.MakeGenericMethod(type.GetGenericArguments()));
                if (baseMethod.ReturnType != typeof(void))
                {
                    int index = Array.IndexOf(
                        baseGenericParams, baseReturnParam.ParameterType);
                    var returnType = (index == -1) ?
                        baseReturnParam.ParameterType : genericParams[index];
                    il.Emit(OpCodes.Box, returnType);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }
                il.Emit(OpCodes.Ret);
            }

            var delegateField = type.DefineField(nameof(MethodDelegate), typeof(MethodDelegate),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

            var infoField = type.DefineField(nameof(MethodInfo), typeof(MethodInfo),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

            var constructor = type.DefineTypeInitializer();
            {
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ldftn, delegateMethod);
                il.Emit(OpCodes.Newobj, typeof(MethodDelegate).GetConstructors()[0]);
                il.Emit(OpCodes.Stsfld, delegateField);
                il.Emit(OpCodes.Ldtoken,
                    !baseMethod.IsGenericMethod ? baseMethod :
                    baseMethod.MakeGenericMethod(type.GetGenericArguments()));
                il.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType);
                il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(
                    nameof(MethodBase.GetMethodFromHandle),
                    new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) }));
                il.Emit(OpCodes.Castclass, typeof(MethodInfo));
                il.Emit(OpCodes.Stsfld, infoField);
                il.Emit(OpCodes.Ret);
            }

            this.BaseMethodDelegateFields[baseMethod] = delegateField;
            this.BaseMethodInfoFields[baseMethod] = infoField;

            type.CreateTypeInfo();
        }

        /// <summary>
        /// 定义所有代理方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineProxyOverrideMethods()
        {
            var baseMethods = this.BaseMethods;
            var type = this.ProxyTypeBuilder;

            var field = type.DefineField(nameof(ProxyInvokeHandler),
                typeof(ProxyInvokeHandler), FieldAttributes.Public);

            this.InternalHandlerField = field;

            foreach (var baseMethod in baseMethods)
            {
                this.DefineProxyOverrideMethod(baseMethod);
            }
        }

        /// <summary>
        /// 定义代理方法，并重写原型类型中的对应方法。
        /// </summary>
        /// <param name="baseMethod">原型类型的方法。</param>
        private void DefineProxyOverrideMethod(MethodInfo baseMethod)
        {
            var type = this.ProxyTypeBuilder;
            var baseMethodDelegateField = this.BaseMethodDelegateFields[baseMethod];
            var baseMethodInfoField = this.BaseMethodInfoFields[baseMethod];
            var handlerField = this.InternalHandlerField;

            var baseInInterface = baseMethod.DeclaringType.IsInterface;
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

            var method = type.DefineMethod($"{baseMethod.ToNameHandleString()}",
                attributes, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));

            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() : method.DefineGenericParameters(
                Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var methodGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericParameterAttributes(
                    methodGenericParam.GenericParameterAttributes);
            }

            if (baseMethod.IsGenericMethod)
            {
                baseMethodDelegateField = TypeBuilder.GetField(
                    baseMethodDelegateField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodDelegateField);
                baseMethodInfoField = TypeBuilder.GetField(
                    baseMethodInfoField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodInfoField);
            }

            var returnParam = method.DefineParameter(0,
                baseReturnParam.Attributes, baseReturnParam.Name);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(i + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            var il = method.GetILGenerator();
            var invokeBaseLabel = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, handlerField);
            il.Emit(OpCodes.Brfalse_S, invokeBaseLabel);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, handlerField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, typeof(object));
            il.Emit(OpCodes.Ldsfld, baseMethodInfoField);
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(i);
                il.EmitLdarg(i + 1);
                il.Emit(OpCodes.Box, baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            il.Emit(OpCodes.Call,
                typeof(ProxyInvokeHandler).GetMethod(nameof(ProxyInvokeHandler.Invoke)));
            var returnLabel = il.DefineLabel();
            il.Emit(OpCodes.Br_S, returnLabel);
            il.MarkLabel(invokeBaseLabel);
            il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            il.Emit(OpCodes.Ldarg_0);
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(i);
                il.EmitLdarg(i + 1);
                il.Emit(OpCodes.Box, baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Call,
                typeof(MethodDelegate).GetMethod(nameof(MethodDelegate.Invoke)));
            il.MarkLabel(returnLabel);
            if (baseMethod.ReturnType != typeof(void))
            {
                il.Emit(OpCodes.Unbox_Any, baseMethod.ReturnType);
            }
            else
            {
                il.Emit(OpCodes.Pop);
            }
            il.Emit(OpCodes.Ret);

            type.DefineMethodOverride(method, baseMethod);
        }
    }
}
