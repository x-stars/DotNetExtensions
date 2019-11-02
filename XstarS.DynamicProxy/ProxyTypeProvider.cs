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
    public sealed partial class ProxyTypeProvider
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
        /// 代理类型中存储原型类型方法的 <see cref="MethodInfo"/> 的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodInfoFields;

        /// <summary>
        /// 代理类型中所有访问原型类型方法的方法。
        /// </summary>
        private Dictionary<MethodInfo, MethodInfo> BaseInvokeMethods;

        /// <summary>
        /// 代理类型中所有原型类型方法的 <see cref="MethodDelegate"/> 委托的字段。
        /// </summary>
        private Dictionary<MethodInfo, FieldInfo> BaseMethodDelegateFields;

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

            this.DefineBaseMethodInfoFields();
            this.DefineProxyTypeConstructors();
            this.DefineBaseInvokeMethods();
            this.DefineBaseMethodDelegateFields();
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
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
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
        /// 定义所有存储原型类型方法的 <see cref="MethodInfo"/> 的字段。
        /// </summary>
        private void DefineBaseMethodInfoFields()
        {
            var baseMethods = this.BaseMethods;
            var type = this.ProxyTypeBuilder;

            var constructor = type.DefineTypeInitializer();
            var ilGen = constructor.GetILGenerator();

            var fields = new Dictionary<MethodInfo, FieldInfo>();
            foreach (var baseMethod in baseMethods)
            {
                var field = type.DefineField($"<{nameof(MethodInfo)}>" +
                    $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                    typeof(MethodInfo),
                    FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                ilGen = constructor.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldtoken, baseMethod);
                    ilGen.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType);
                    ilGen.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(
                        nameof(MethodBase.GetMethodFromHandle),
                        new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) }));
                    ilGen.Emit(OpCodes.Castclass, typeof(MethodInfo));
                    ilGen.Emit(OpCodes.Stsfld, field);
                }

                fields[baseMethod] = field;
            }

            ilGen.Emit(OpCodes.Ret);

            this.BaseMethodInfoFields = fields;
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
        /// 定义所有原型类型方法的 <see cref="MethodDelegate"/> 委托字段。
        /// </summary>
        private void DefineBaseMethodDelegateFields()
        {
            var baseMethods = this.BaseMethods;

            var fields = new Dictionary<MethodInfo, FieldInfo>();
            foreach (var baseMethod in baseMethods)
            {
                var field = this.DefineBaseMethodDelegateField(baseMethod);
                fields[baseMethod] = field;
            }

            this.BaseMethodDelegateFields = fields;
        }

        /// <summary>
        /// 定义原型类型方法的 <see cref="MethodDelegate"/> 委托字段。
        /// </summary>
        /// <param name="baseMethod">原型类型的方法。</param>
        /// <returns>定义的  <see cref="MethodDelegate"/> 委托字段。</returns>
        private FieldBuilder DefineBaseMethodDelegateField(MethodInfo baseMethod)
        {
            var baseType = this.BaseType;
            var proxyType = this.ProxyTypeBuilder;
            var baseInvokeMethod = this.BaseInvokeMethods[baseMethod];

            var type = proxyType.DefineNestedType($"<{nameof(MethodDelegate)}>" +
                $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                TypeAttributes.Class | TypeAttributes.NestedAssembly |
                TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseParameters = baseMethod.GetParameters();
            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                type.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                var constraints = baseGenericParam.GetGenericParameterConstraints();
                var baseTypeConstraint = constraints.Where(
                    constraint => !constraint.IsInterface).SingleOrDefault();
                var interfaceConstraints = constraints.Where(
                    constraint => constraint.IsInterface).ToArray();
                genericParam.SetGenericParameterAttributes(
                    baseGenericParam.GenericParameterAttributes);
                if (!(baseTypeConstraint is null))
                {
                    genericParam.SetBaseTypeConstraint(baseTypeConstraint);
                }
                if (interfaceConstraints.Length != 0)
                {
                    genericParam.SetInterfaceConstraints(interfaceConstraints);
                }
            }

            baseInvokeMethod = !baseMethod.IsGenericMethod ? baseInvokeMethod :
                baseInvokeMethod.MakeGenericMethod(type.GetGenericArguments());
            var method = type.DefineMethod(nameof(MethodDelegate.Invoke),
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(object), new[] { typeof(object), typeof(object[]) });
            method.DefineParameter(1, ParameterAttributes.None, "instance");
            method.DefineParameter(2, ParameterAttributes.None, "arguments");
            var ilGen = method.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Castclass, proxyType);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    ilGen.Emit(OpCodes.Ldarg_1);
                    var baseParameter = baseParameters[i];
                    int indexPtInGpt = Array.IndexOf(
                        baseGenericParams, baseParameter.ParameterType);
                    var parameterType = (indexPtInGpt == -1) ?
                        baseParameter.ParameterType : genericParams[indexPtInGpt];
                    ilGen.EmitLdcI4(i);
                    ilGen.Emit(OpCodes.Ldelem_Ref);
                    ilGen.Emit(OpCodes.Unbox_Any, parameterType);
                }
                ilGen.Emit(OpCodes.Call, baseInvokeMethod);
                if (baseMethod.ReturnType != typeof(void))
                {
                    var baseReturnType = baseMethod.ReturnParameter.ParameterType;
                    int indexRptInGpt = Array.IndexOf(
                        baseGenericParams, baseReturnType);
                    var returnType = (indexRptInGpt == -1) ?
                        baseReturnType : genericParams[indexRptInGpt];
                    ilGen.Emit(OpCodes.Box, returnType);
                }
                else
                {
                    ilGen.Emit(OpCodes.Ldnull);
                }
                ilGen.Emit(OpCodes.Ret);
            }

            var field = type.DefineField(nameof(MethodDelegate), typeof(MethodDelegate),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

            var constructor = type.DefineTypeInitializer();
            ilGen = constructor.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldnull);
                ilGen.Emit(OpCodes.Ldftn, method);
                ilGen.Emit(OpCodes.Newobj, typeof(MethodDelegate).GetConstructors()[0]);
                ilGen.Emit(OpCodes.Stsfld, field);
                ilGen.Emit(OpCodes.Ret);
            }

            type.CreateTypeInfo();

            return field;
        }

        /// <summary>
        /// 定义所有代理方法，并重写原型类型中的对应方法。
        /// </summary>
        private void DefineProxyOverrideMethods()
        {
            var baseMethods = this.BaseMethods;
            var type = this.ProxyTypeBuilder;

            var field = type.DefineField(nameof(ProxyInvokeHandler),
                typeof(ProxyInvokeHandler), FieldAttributes.Assembly);

            this.InternalHandlerField = field;

            foreach (var baseMethod in baseMethods)
            {
                var method = this.DefineProxyOverrideMethod(baseMethod);
            }
        }

        /// <summary>
        /// 定义代理方法，并重写原型类型中的对应方法。
        /// </summary>
        /// <param name="baseMethod">原型类型的方法。</param>
        /// <returns>定义的代理方法。</returns>
        private MethodBuilder DefineProxyOverrideMethod(MethodInfo baseMethod)
        {
            var baseInInterface = baseMethod.DeclaringType.IsInterface;
            var type = this.ProxyTypeBuilder;
            var baseMethodInvokerField = this.BaseMethodDelegateFields[baseMethod];
            var baseMethodInfoField = this.BaseMethodInfoFields[baseMethod];
            var methodInvokeHandlerField = this.InternalHandlerField;

            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
            var method = type.DefineMethod(baseMethod.Name,
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
                baseMethodInvokerField = TypeBuilder.GetField(
                    baseMethodInvokerField.ReflectedType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodInvokerField);
            }

            var returnParam = method.DefineParameter(0, baseReturnParam.Attributes, null);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }

            var ilGen = method.GetILGenerator();
            {
                var invokeBaseLabel = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
                ilGen.Emit(OpCodes.Brfalse_S, invokeBaseLabel);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Castclass, typeof(object));
                ilGen.Emit(OpCodes.Ldsfld, baseMethodInfoField);
                ilGen.EmitLdcI4(genericParams.Length);
                ilGen.Emit(OpCodes.Newarr, typeof(Type));
                for (int i = 0; i < genericParams.Length; i++)
                {
                    var genericParam = genericParams[i];
                    ilGen.Emit(OpCodes.Dup);
                    ilGen.EmitLdcI4(i);
                    ilGen.Emit(OpCodes.Ldtoken, genericParam);
                    ilGen.Emit(OpCodes.Call,
                        typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle)));
                    ilGen.Emit(OpCodes.Stelem_Ref);
                }
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
                ilGen.Emit(OpCodes.Ldsfld, baseMethodInvokerField);
                ilGen.Emit(OpCodes.Newobj, typeof(ProxyInvokeInfo).GetConstructors()[0]);
                ilGen.Emit(OpCodes.Call,
                    typeof(ProxyInvokeHandler).GetMethod(nameof(ProxyInvokeHandler.Invoke)));
                var returnLabel = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Br_S, returnLabel);
                ilGen.MarkLabel(invokeBaseLabel);
                ilGen.Emit(OpCodes.Ldsfld, baseMethodInvokerField);
                ilGen.Emit(OpCodes.Ldarg_0);
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
                ilGen.Emit(OpCodes.Call,
                    typeof(MethodDelegate).GetMethod(nameof(MethodDelegate.Invoke)));
                ilGen.MarkLabel(returnLabel);
                if (baseMethod.ReturnType != typeof(void))
                {
                    ilGen.Emit(OpCodes.Unbox_Any, baseMethod.ReturnType);
                }
                else
                {
                    ilGen.Emit(OpCodes.Pop);
                }
                ilGen.Emit(OpCodes.Ret);
            }

            return method;
        }
    }
}
