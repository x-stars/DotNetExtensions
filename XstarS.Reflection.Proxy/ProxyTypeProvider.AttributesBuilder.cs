using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    public sealed partial class ProxyTypeProvider
    {
        private sealed class AttributesBuilder
        {
            internal AttributesBuilder(ProxyTypeProvider proxyBuilder)
            {
                this.ProxyBuilder = proxyBuilder;
                this.BuildAttributesType();
            }

            internal ProxyTypeProvider ProxyBuilder { get; }

            internal TypeBuilder AttributesTypeBuilder { get; private set; }

            internal FieldBuilder[] OnMemberInvokeFields { get; private set; }

            internal Dictionary<MethodInfo, TypeBuilder> MethodAttributesTypes { get; private set; }

            internal Dictionary<MethodInfo, FieldBuilder[]> MethodsOnMethodInvokeFields { get; private set; }

            private void BuildAttributesType()
            {
                // 定义存储代理特性的类型。
                this.DefineAttributesType();

                // 定义存储代理特性的字段。
                this.DefineOnMemberInvokeFields();
                this.DefineMethodAttributesTypes();

                // 完成类型创建。
                this.AttributesTypeBuilder.CreateTypeInfo();
            }

            private void DefineAttributesType()
            {
                var objectProxyType = this.ProxyBuilder.ProxyTypeBuilder;

                // 定义存储代理特性的类型。
                var attributesType = objectProxyType.DefineNestedType($"<{nameof(Attribute)}>",
                    TypeAttributes.Class | TypeAttributes.NestedAssembly |
                    TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                this.AttributesTypeBuilder = attributesType;
            }

            private void DefineOnMemberInvokeFields()
            {
                var baseType = this.ProxyBuilder.PrototypeType;
                var attributesType = this.AttributesTypeBuilder;

                // 获取相关特性。
                var onMemberInvokeAttributes =
                    baseType.GetCustomAttributes(ReflectionData.T_OnMemberInvokeAttribute, false);

                // 对于不包含类型代理特性的类型则直接返回空数组。
                if (onMemberInvokeAttributes.Length == 0)
                {
                    this.OnMemberInvokeFields = Array.Empty<FieldBuilder>();
                    return;
                }

                var onMemberInvokeFields = new FieldBuilder[onMemberInvokeAttributes.Length];
                // 定义静态构造函数。
                var constructor = attributesType.DefineTypeInitializer();
                // 生成 IL 代码。
                var ilGen = constructor.GetILGenerator();
                {
                    // 反射获取相关特性。
                    ilGen.Emit(OpCodes.Ldtoken, baseType);
                    ilGen.Emit(OpCodes.Call, ReflectionData.T_Type_SM_GetTypeFromHandle);
                    ilGen.Emit(OpCodes.Ldtoken, ReflectionData.T_OnMemberInvokeAttribute);
                    ilGen.Emit(OpCodes.Call, ReflectionData.T_Type_SM_GetTypeFromHandle);
                    ilGen.Emit(OpCodes.Ldc_I4_0);
                    ilGen.Emit(OpCodes.Callvirt, ReflectionData.T_MemberInfo_IM_GetCustomAttributes_2);
                    // 生成特性的静态字段并存储特性信息。
                    for (int i = 0; i < onMemberInvokeAttributes.Length; i++)
                    {
                        var attributeField = attributesType.DefineField(
                            $"<{nameof(OnMemberInvokeAttribute)}>#{i.ToString()}",
                            ReflectionData.T_OnMemberInvokeAttribute,
                            FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                        onMemberInvokeFields[i] = attributeField;
                        ilGen.Emit(OpCodes.Dup);
                        ilGen.EmitLdcI4(i);
                        ilGen.Emit(OpCodes.Ldelem_Ref);
                        ilGen.Emit(OpCodes.Castclass, ReflectionData.T_OnMemberInvokeAttribute);
                        ilGen.Emit(OpCodes.Stsfld, attributeField);
                    }
                    ilGen.Emit(OpCodes.Pop);
                    ilGen.Emit(OpCodes.Ret);
                }

                this.OnMemberInvokeFields = onMemberInvokeFields;
            }

            private void DefineMethodAttributesTypes()
            {
                var baseMethods = this.ProxyBuilder.BaseMethods;
                var methodAttributesTypes = new Dictionary<MethodInfo, TypeBuilder>();
                var methodsOnMethodInvokeFields = new Dictionary<MethodInfo, FieldBuilder[]>();

                // 依次为每个方法定义代理特性存储类。
                foreach (var baseMethod in baseMethods)
                {
                    var methodAttributesTypeBuilder = new MethodAttributesBuilder(this, baseMethod);
                    methodAttributesTypes[baseMethod] = methodAttributesTypeBuilder.MethodAttributesTypeBuilder;
                    methodsOnMethodInvokeFields[baseMethod] = methodAttributesTypeBuilder.OnMethodInvokeFields;
                }

                this.MethodAttributesTypes = methodAttributesTypes;
                this.MethodsOnMethodInvokeFields = methodsOnMethodInvokeFields;
            }

            private class MethodAttributesBuilder
            {
                internal MethodAttributesBuilder(
                    AttributesBuilder attributesTypeBuilder, MethodInfo baseMethod)
                {
                    this.AttributesBuilder = attributesTypeBuilder;
                    this.BaseMethod = baseMethod;
                    this.BuildMethodAttributesType();
                }

                internal AttributesBuilder AttributesBuilder { get; }

                internal MethodInfo BaseMethod { get; }

                internal TypeBuilder MethodAttributesTypeBuilder { get; private set; }

                internal FieldBuilder[] OnMethodInvokeFields { get; private set; }

                private void BuildMethodAttributesType()
                {
                    var baseMethod = this.BaseMethod;

                    // 获取相关特性。
                    var onMethodInvokeAttributes =
                        baseMethod.GetCustomAttributes(ReflectionData.T_OnMethodInvokeAttribute, false);

                    // 对于不包含方法代理特性的方法则直接返回空引用。
                    if (onMethodInvokeAttributes.Length == 0)
                    {
                        this.MethodAttributesTypeBuilder = null;
                        this.OnMethodInvokeFields = Array.Empty<FieldBuilder>();
                        return;
                    }

                    // 定义保存方法代理特性的类型。
                    this.DefineMethodAttributesType();

                    // 初始化方法代理特性的字段。
                    this.DefineOnMethodInvokeFields();

                    // 完成类型创建。
                    this.MethodAttributesTypeBuilder.CreateTypeInfo();
                }

                private void DefineMethodAttributesType()
                {
                    var baseMethod = this.BaseMethod;
                    var attributesType = this.AttributesBuilder.AttributesTypeBuilder;

                    // 定义保存方法代理特性的类型。
                    var methodAttributesType = attributesType.DefineNestedType(
                        $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                        TypeAttributes.Class | TypeAttributes.NestedAssembly |
                        TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                    this.MethodAttributesTypeBuilder = methodAttributesType;
                }

                private void DefineOnMethodInvokeFields()
                {
                    var baseMethod = this.BaseMethod;
                    var methodAttributesType = this.MethodAttributesTypeBuilder;

                    // 获取相关特性。
                    var onMethodInvokeAttributes =
                        baseMethod.GetCustomAttributes(ReflectionData.T_OnMethodInvokeAttribute, false);

                    var onMethodInvokeFields = new FieldBuilder[onMethodInvokeAttributes.Length];
                    // 定义静态构造函数。
                    var constructor = methodAttributesType.DefineTypeInitializer();
                    // 生成 IL 代码。
                    var ilGen = constructor.GetILGenerator();
                    {
                        // 反射获取相关特性。
                        ilGen.Emit(OpCodes.Ldtoken, baseMethod);
                        ilGen.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType);
                        ilGen.Emit(OpCodes.Call, ReflectionData.T_MethodBase_SM_GetMethodFromHandle_2);
                        ilGen.Emit(OpCodes.Ldtoken, ReflectionData.T_OnMethodInvokeAttribute);
                        ilGen.Emit(OpCodes.Call, ReflectionData.T_Type_SM_GetTypeFromHandle);
                        ilGen.Emit(OpCodes.Ldc_I4_0);
                        ilGen.Emit(OpCodes.Callvirt, ReflectionData.T_MemberInfo_IM_GetCustomAttributes_2);
                        // 生成特性的静态字段并存储特性信息。
                        for (int i = 0; i < onMethodInvokeAttributes.Length; i++)
                        {
                            var onMethodInvokeField = methodAttributesType.DefineField(
                                $"<{nameof(OnMethodInvokeAttribute)}>#{i.ToString()}",
                                ReflectionData.T_OnMethodInvokeAttribute,
                                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);
                            onMethodInvokeFields[i] = onMethodInvokeField;
                            ilGen.Emit(OpCodes.Dup);
                            ilGen.EmitLdcI4(i);
                            ilGen.Emit(OpCodes.Ldelem_Ref);
                            ilGen.Emit(OpCodes.Castclass, ReflectionData.T_OnMethodInvokeAttribute);
                            ilGen.Emit(OpCodes.Stsfld, onMethodInvokeField);
                        }
                        ilGen.Emit(OpCodes.Pop);
                        ilGen.Emit(OpCodes.Ret);
                    }

                    this.OnMethodInvokeFields = onMethodInvokeFields;
                }
            }
        }
    }
}
