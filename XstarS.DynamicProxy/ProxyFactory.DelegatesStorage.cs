using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    public sealed partial class ProxyFactory
    {
        private sealed class DelegatesStorage
        {
            private TypeBuilder DelegatesTypeBuilder;

            internal DelegatesStorage(ProxyFactory proxyType)
            {
                this.ProxyType = proxyType;
                this.DelegatesType = this.BuildDelegatesType();
            }

            public ProxyFactory ProxyType { get; }

            public Type DelegatesType { get; }

            public Dictionary<MethodInfo, Type> MethodDelegateTypes { get; private set; }

            public Dictionary<MethodInfo, MethodInfo> ProxyMethods { get; private set; }

            public Dictionary<MethodInfo, FieldInfo> ProxyDelegateFields { get; private set; }

            private Type BuildDelegatesType()
            {
                // 定义保存所有代理方法的委托的类型。
                this.DefineDelegatesType();

                // 定义所有保存代理方法的委托的类型。
                this.DefineMethodDelegateTypes();

                // 完成类型创建。
                return this.DelegatesTypeBuilder.CreateTypeInfo();
            }

            private void DefineDelegatesType()
            {
                var proxyType = this.ProxyType.ProxyTypeBuilder;

                // 定义保存所有代理方法的委托的类型。
                var delegatesType = proxyType.DefineNestedType($"<{nameof(Delegate)}>",
                    TypeAttributes.Class | TypeAttributes.NestedAssembly |
                    TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                this.DelegatesTypeBuilder = delegatesType;
            }

            private void DefineMethodDelegateTypes()
            {
                var baseMethods = this.ProxyType.BaseMethods;
                var methodDelegateTypes = new Dictionary<MethodInfo, Type>();
                var proxyMethods = new Dictionary<MethodInfo, MethodInfo>();
                var proxyDelegateFields = new Dictionary<MethodInfo, FieldInfo>();

                // 定义所有保存代理方法的委托的类型。
                foreach (var baseMethod in baseMethods)
                {
                    var methodDelegateTypeBuilder = new MethodDelegateTypeProvider(this, baseMethod);
                    methodDelegateTypes[baseMethod] = methodDelegateTypeBuilder.MethodDelegateType;
                    proxyMethods[baseMethod] = methodDelegateTypeBuilder.ProxyMethod;
                    proxyDelegateFields[baseMethod] = methodDelegateTypeBuilder.ProxyDelegateField;
                }

                this.MethodDelegateTypes = methodDelegateTypes;
                this.ProxyMethods = proxyMethods;
                this.ProxyDelegateFields = proxyDelegateFields;
            }

            private sealed class MethodDelegateTypeProvider
            {
                private TypeBuilder MethodDelegateTypeBuilder;

                private FieldInfo BaseMethodInfoField;

                private GenericTypeParameterBuilder[] GenericParameters;

                private ConstructorBuilder Constructor;

                private OnMemberInvokeAttribute[] OnMemberInvokeAttributes;

                private OnMethodInvokeAttribute[] OnMethodInvokeAttributes;

                internal MethodDelegateTypeProvider(
                    DelegatesStorage delegatesType, MethodInfo baseMethod)
                {
                    this.DelegatesType = delegatesType;
                    this.BaseMethod = baseMethod;
                    this.MethodDelegateType = this.BuildMethodDelegateType();
                }

                public DelegatesStorage DelegatesType { get; }

                public MethodInfo BaseMethod { get; }

                public Type MethodDelegateType { get; }

                public MethodInfo ProxyMethod { get; private set; }

                public FieldInfo ProxyDelegateField { get; private set; }

                private Type BuildMethodDelegateType()
                {
                    var baseType = this.DelegatesType.ProxyType.BaseType;
                    var baseMethod = this.BaseMethod;
                    var delegatesType = this.DelegatesType.DelegatesTypeBuilder;

                    // 获取相关特性。
                    var onMemberInvokeAttributes = Array.ConvertAll(
                        baseType.GetCustomAttributes(ReflectionData.T_OnMemberInvokeAttribute, false),
                        attribute => (OnMemberInvokeAttribute)attribute);
                    var onMethodInvokeAttributes = Array.ConvertAll(
                        baseMethod.GetCustomAttributes(ReflectionData.T_OnMethodInvokeAttribute, false),
                        attribute => (OnMethodInvokeAttribute)attribute);
                    this.OnMemberInvokeAttributes = onMemberInvokeAttributes;
                    this.OnMethodInvokeAttributes = onMethodInvokeAttributes;

                    // 对于无需创建代理的方法则直接返回空引用。
                    if ((onMethodInvokeAttributes.Length == 0) && (
                        (onMemberInvokeAttributes.Length == 0) ||
                        (onMemberInvokeAttributes.All(attribute => !attribute.FilterMethod(baseMethod)))))
                    {
                        this.MethodDelegateTypeBuilder = null;
                        this.ProxyMethod = null;
                        this.ProxyDelegateField = null;
                        return null;
                    }

                    // 定义保存代理方法的委托的类型。
                    this.DefineMethodDelegateType();

                    // 定义基类方法元数据字段。
                    this.DefineMethodInfoFields();
                    // 依次生成各级代理方法和委托。
                    this.DefineBaseProxyMethod();
                    this.DefineOnMethodInvokeProxyMethods();
                    this.DefineOnMemberInvokeProxyMethods();
                    var ilGen = this.Constructor.GetILGenerator();
                    {
                        ilGen.Emit(OpCodes.Ret);
                    }

                    // 完成类型创建。
                    return this.MethodDelegateTypeBuilder.CreateTypeInfo();
                }

                private void DefineMethodDelegateType()
                {
                    var baseType = this.DelegatesType.ProxyType.BaseType;
                    var baseMethod = this.BaseMethod;
                    var delegatesType = this.DelegatesType.DelegatesTypeBuilder;

                    // 定义保存代理方法的委托的类型。
                    var methodDelegateType = delegatesType.DefineNestedType(
                        $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                        TypeAttributes.Class | TypeAttributes.NestedAssembly |
                        TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                    this.MethodDelegateTypeBuilder = methodDelegateType;
                    // 处理方法的泛型参数。
                    var methodGenericParams = baseMethod.GetGenericArguments();
                    var typeGenericParams = (methodGenericParams.Length == 0) ?
                        Array.Empty<GenericTypeParameterBuilder>() :
                        methodDelegateType.DefineGenericParameters(
                            Array.ConvertAll(methodGenericParams, param => param.Name));
                    for (int i = 0; i < methodGenericParams.Length; i++)
                    {
                        var methodGenericParam = methodGenericParams[i];
                        var typeGenericParam = typeGenericParams[i];
                        var constraints = methodGenericParam.GetGenericParameterConstraints();
                        var baseTypeConstraint = constraints.Where(
                            constraint => !constraint.IsInterface).SingleOrDefault();
                        var interfaceConstraints = constraints.Where(
                            constraint => constraint.IsInterface).ToArray();
                        typeGenericParam.SetGenericParameterAttributes(
                            methodGenericParam.GenericParameterAttributes);
                        if (!(baseTypeConstraint is null))
                        {
                            typeGenericParam.SetBaseTypeConstraint(baseTypeConstraint);
                        }
                        if (interfaceConstraints.Length != 0)
                        {
                            typeGenericParam.SetInterfaceConstraints(interfaceConstraints);
                        }
                    }
                    this.GenericParameters = typeGenericParams;
                    // 定义静态构造函数。
                    var constructor = methodDelegateType.DefineTypeInitializer();
                    this.Constructor = constructor;
                }

                private void DefineMethodInfoFields()
                {
                    var baseMethod = this.BaseMethod;
                    var methodDelegateType = this.MethodDelegateTypeBuilder;
                    var constructor = this.Constructor;

                    // 定义基类方法元数据字段。
                    var baseMethodInfoField = methodDelegateType.DefineField(
                        $"<{nameof(MethodInfo)}>#0", typeof(MethodInfo),
                        FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                    this.BaseMethodInfoField = baseMethodInfoField;
                    var ilGen = constructor.GetILGenerator();
                    {
                        ilGen.Emit(OpCodes.Ldtoken, baseMethod);
                        ilGen.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType);
                        ilGen.Emit(OpCodes.Call, ReflectionData.T_MethodBase_SM_GetMethodFromHandle_2);
                        ilGen.Emit(OpCodes.Castclass, typeof(MethodInfo));
                        ilGen.Emit(OpCodes.Stsfld, baseMethodInfoField);
                    }
                }

                private void DefineBaseProxyMethod()
                {
                    var proxyType = this.DelegatesType.ProxyType.ProxyTypeBuilder;
                    var baseMethod = this.BaseMethod;
                    var baseAccessMethod =
                        this.DelegatesType.ProxyType.BaseAccessMethods[baseMethod];
                    var methodDelegateType = this.MethodDelegateTypeBuilder;
                    var typeGenericParams = this.GenericParameters;
                    var constructor = this.Constructor;
                    var hasReturns = baseMethod.ReturnType != typeof(void);
                    var methodGenericParams = baseMethod.GetGenericArguments();
                    baseAccessMethod = (methodGenericParams.Length == 0) ? baseAccessMethod :
                        baseAccessMethod.MakeGenericMethod(methodDelegateType.GetGenericArguments());

                    // 定义调用基类方法的代理方法。
                    var proxyMethod = methodDelegateType.DefineMethod($"<Base>#0",
                        MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                        typeof(object), new[] { typeof(object), typeof(object[]) });
                    proxyMethod.DefineParameter(1, ParameterAttributes.None, "target");
                    proxyMethod.DefineParameter(2, ParameterAttributes.None, "arguments");
                    var proxyDelegateField = methodDelegateType.DefineField(
                        $"<Base>{nameof(Delegate)}#0",
                        ReflectionData.T_MethodInvoker,
                        FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                    // 生成 IL 代码。
                    var ilGen = proxyMethod.GetILGenerator();
                    {
                        ilGen.Emit(OpCodes.Ldarg_0);
                        ilGen.Emit(OpCodes.Castclass, proxyType);
                        var baseGenericParams = baseMethod.GetGenericArguments();
                        var baseParameters = baseMethod.GetParameters();
                        if (baseParameters.Length != 0)
                        {
                            for (int i = 0; i < baseParameters.Length; i++)
                            {
                                ilGen.Emit(OpCodes.Ldarg_1);
                                var baseParameter = baseParameters[i];
                                int indexPtInGpt = Array.IndexOf(
                                    baseGenericParams, baseParameter.ParameterType);
                                var parameterType = (indexPtInGpt == -1) ?
                                    baseParameter.ParameterType : typeGenericParams[indexPtInGpt];
                                ilGen.EmitLdcI4(i);
                                ilGen.Emit(OpCodes.Ldelem_Ref);
                                ilGen.Emit(OpCodes.Unbox_Any, parameterType);
                            }
                        }
                        ilGen.Emit(OpCodes.Call, baseAccessMethod);
                        if (hasReturns)
                        {
                            var baseReturnType = baseMethod.ReturnParameter.ParameterType;
                            int indexRptInGpt = Array.IndexOf(
                                baseGenericParams, baseReturnType);
                            var returnType = (indexRptInGpt == -1) ?
                                baseReturnType : typeGenericParams[indexRptInGpt];
                            ilGen.Emit(OpCodes.Box, returnType);
                        }
                        else
                        {
                            ilGen.Emit(OpCodes.Ldnull);
                        }
                        ilGen.Emit(OpCodes.Ret);
                    }
                    // 初始化代理方法委托。
                    ilGen = constructor.GetILGenerator();
                    {
                        ilGen.Emit(OpCodes.Ldnull);
                        ilGen.Emit(OpCodes.Ldftn, proxyMethod);
                        ilGen.Emit(OpCodes.Newobj, ReflectionData.T_MethodInvoker_IC_ctor);
                        ilGen.Emit(OpCodes.Stsfld, proxyDelegateField);
                    }

                    this.ProxyMethod = proxyMethod;
                    this.ProxyDelegateField = proxyDelegateField;
                }

                private void DefineOnMethodInvokeProxyMethods()
                {
                    var baseMethod = this.BaseMethod;
                    var baseMethodInfoField = this.BaseMethodInfoField;
                    var onMethodInvokeFields =
                        this.DelegatesType.ProxyType.MethodsOnMethodInvokeFields[baseMethod];
                    var methodDelegateType = this.MethodDelegateTypeBuilder;
                    var typeGenericParams = this.GenericParameters;
                    var constructor = this.Constructor;
                    var onMethodInvokeAttributes = this.OnMethodInvokeAttributes;

                    for (int i = onMethodInvokeAttributes.Length - 1; i >= 0; i--)
                    {
                        var lastProxyDelagateField = this.ProxyDelegateField;

                        // 定义方法特性的代理方法。
                        var onMethodInvokeField = onMethodInvokeFields[i];
                        var proxyMethod = methodDelegateType.DefineMethod(
                            $"<{nameof(OnMethodInvokeAttribute)}>#{i.ToString()}",
                            MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                        typeof(object), new[] { typeof(object), typeof(object[]) });
                        proxyMethod.DefineParameter(1, ParameterAttributes.None, "target");
                        proxyMethod.DefineParameter(2, ParameterAttributes.None, "arguments");
                        var proxyDelegateField = methodDelegateType.DefineField(
                            $"<{nameof(OnMethodInvokeAttribute)}>{nameof(Delegate)}#{i.ToString()}",
                            ReflectionData.T_MethodInvoker,
                            FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                        // 生成 IL 代码。
                        var ilGen = proxyMethod.GetILGenerator();
                        {
                            ilGen.Emit(OpCodes.Ldsfld, onMethodInvokeField);
                            ilGen.Emit(OpCodes.Ldsfld, lastProxyDelagateField);
                            ilGen.Emit(OpCodes.Ldarg_0);
                            ilGen.Emit(OpCodes.Ldsfld, baseMethodInfoField);
                            ilGen.EmitLdcI4(typeGenericParams.Length);
                            ilGen.Emit(OpCodes.Newarr, typeof(Type));
                            for (int j = 0; j < typeGenericParams.Length; j++)
                            {
                                var typeGenericParam = typeGenericParams[j];
                                ilGen.Emit(OpCodes.Dup);
                                ilGen.EmitLdcI4(j);
                                ilGen.Emit(OpCodes.Ldtoken, typeGenericParam);
                                ilGen.Emit(OpCodes.Call, ReflectionData.T_Type_SM_GetTypeFromHandle);
                                ilGen.Emit(OpCodes.Stelem_Ref);
                            }
                            ilGen.Emit(OpCodes.Ldarg_1);
                            ilGen.Emit(OpCodes.Callvirt, ReflectionData.T_OnMethodInvokeAttribute_IM_Invoke);
                            ilGen.Emit(OpCodes.Ret);
                        }
                        // 初始化代理方法委托。
                        ilGen = constructor.GetILGenerator();
                        {
                            ilGen.Emit(OpCodes.Ldnull);
                            ilGen.Emit(OpCodes.Ldftn, proxyMethod);
                            ilGen.Emit(OpCodes.Newobj, ReflectionData.T_MethodInvoker_IC_ctor);
                            ilGen.Emit(OpCodes.Stsfld, proxyDelegateField);
                        }

                        this.ProxyMethod = proxyMethod;
                        this.ProxyDelegateField = proxyDelegateField;
                    }
                }

                private void DefineOnMemberInvokeProxyMethods()
                {
                    var baseMethod = this.BaseMethod;
                    var baseMethodInfoField = this.BaseMethodInfoField;
                    var onMemberInvokeFields =
                        this.DelegatesType.ProxyType.OnMemberInvokeFields;
                    var methodDelegateType = this.MethodDelegateTypeBuilder;
                    var typeGenericParams = this.GenericParameters;
                    var constructor = this.Constructor;
                    var onMemberInvokeAttributes = this.OnMemberInvokeAttributes;

                    for (int i = onMemberInvokeAttributes.Length - 1; i >= 0; i--)
                    {
                        var lastProxyDelagateField = this.ProxyDelegateField;

                        // 仅对满足条件的成员方法设置代理。
                        var onMemberInvokeAttribute = onMemberInvokeAttributes[i];
                        if (!onMemberInvokeAttribute.FilterMethod(baseMethod)) { continue; }

                        // 定义类型特性的代理方法。
                        var onMemberInvokeField = onMemberInvokeFields[i];
                        var proxyMethod = methodDelegateType.DefineMethod(
                            $"<{nameof(OnMemberInvokeAttribute)}>#{i.ToString()}",
                            MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                        typeof(object), new[] { typeof(object), typeof(object[]) });
                        proxyMethod.DefineParameter(1, ParameterAttributes.None, "target");
                        proxyMethod.DefineParameter(2, ParameterAttributes.None, "arguments");
                        var proxyDelegateField = methodDelegateType.DefineField(
                            $"<{nameof(OnMemberInvokeAttribute)}>{nameof(Delegate)}#{i.ToString()}",
                            ReflectionData.T_MethodInvoker,
                            FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                        // 生成 IL 代码。
                        var ilGen = proxyMethod.GetILGenerator();
                        {
                            ilGen.Emit(OpCodes.Ldsfld, onMemberInvokeField);
                            ilGen.Emit(OpCodes.Ldsfld, lastProxyDelagateField);
                            ilGen.Emit(OpCodes.Ldarg_0);
                            ilGen.Emit(OpCodes.Ldsfld, baseMethodInfoField);
                            ilGen.EmitLdcI4(typeGenericParams.Length);
                            ilGen.Emit(OpCodes.Newarr, typeof(Type));
                            for (int j = 0; j < typeGenericParams.Length; j++)
                            {
                                var typeGenericParam = typeGenericParams[j];
                                ilGen.Emit(OpCodes.Dup);
                                ilGen.EmitLdcI4(j);
                                ilGen.Emit(OpCodes.Ldtoken, typeGenericParam);
                                ilGen.Emit(OpCodes.Call, ReflectionData.T_Type_SM_GetTypeFromHandle);
                                ilGen.Emit(OpCodes.Stelem_Ref);
                            }
                            ilGen.Emit(OpCodes.Ldarg_1);
                            ilGen.Emit(OpCodes.Callvirt, ReflectionData.T_OnMemberInvokeAttribute_IM_Invoke);
                            ilGen.Emit(OpCodes.Ret);
                        }
                        // 初始化代理方法委托。
                        ilGen = constructor.GetILGenerator();
                        {
                            ilGen.Emit(OpCodes.Ldnull);
                            ilGen.Emit(OpCodes.Ldftn, proxyMethod);
                            ilGen.Emit(OpCodes.Newobj, ReflectionData.T_MethodInvoker_IC_ctor);
                            ilGen.Emit(OpCodes.Stsfld, proxyDelegateField);
                        }

                        this.ProxyMethod = proxyMethod;
                        this.ProxyDelegateField = proxyDelegateField;
                    }
                }
            }
        }
    }
}
