using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从指定原型类型构造代理派生类型及其实例的方法。
    /// </summary>
    public partial class ProxyBuilder : ProxyBuilderBase<object>
    {
        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="ProxyBuilder"/> 类的新实例。
        /// </summary>
        /// <param name="type">作为原型类型的 <see cref="Type"/> 对象。</param>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        internal ProxyBuilder(Type type) : base()
        {
            if (!(((type.IsClass && !type.IsSealed) || type.IsInterface) &&
                type.IsVisible && !type.ContainsGenericParameters))
            {
                throw new TypeAccessException();
            }
            this.PrototypeType = type;
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
        /// 代理类型的 <see cref="TypeBuilder"/> 对象。
        /// </summary>
        internal TypeBuilder ObjectProxyType { get; private set; }

        /// <summary>
        /// 代理类型中用于访问原型类型中对应方法的方法。
        /// </summary>
        internal IDictionary<MethodInfo, MethodBuilder> BaseAccessMethods { get; private set; }

        /// <summary>
        /// 代理类型中用于保存原型类型中定义的 <see cref="OnMemberInvokeAttribute"/> 特性的字段。
        /// </summary>
        internal FieldBuilder[] OnMemberInvokeFields { get; private set; }

        /// <summary>
        /// 代理类型中用于保存原型类型中定义的 <see cref="OnMethodInvokeAttribute"/> 特性的字段。
        /// </summary>
        internal IDictionary<MethodInfo, FieldBuilder[]> MethodsOnMethodInvokeFields { get; private set; }

        /// <summary>
        /// 代理类型中对应原型类型中方法的代理方法的 <see cref="MethodInvoker"/> 委托的字段。
        /// </summary>
        internal IDictionary<MethodInfo, FieldBuilder> ProxyDelegateFields { get; private set; }

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        protected override Type BuildProxyType()
        {
            // 初始化原型类型中可以被重写的方法。
            this.InitializeBaseMethods();

            // 定义代理类型。
            this.DefineProxyType();

            // 定义代理类型的各个成员。
            this.DefineConstructors();
            this.DefineBaseAccessMethods();
            this.DefineAttributesType();
            this.DefineDelegatesType();
            this.DefineOverrideMethods();

            // 完成类型创建。
            return this.ObjectProxyType.CreateTypeInfo();
        }

        /// <summary>
        /// 初始化原型类型中可以重写的方法成员信息。
        /// </summary>
        private void InitializeBaseMethods()
        {
            this.BaseMethods = this.PrototypeType.GetAccessibleMethods().Where(
                baseMethod => baseMethod.IsOverridable()).ToArray();
        }

        /// <summary>
        /// 以原型类型为基础，定义代理派生类型。
        /// </summary>
        private void DefineProxyType()
        {
            var baseType = this.PrototypeType;

            // 定义动态程序集。
            var asmName = baseType.ToString();
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
            bool isInterface = baseType.IsInterface;
            var parent = !isInterface ? baseType : typeof(object);
            var interfaces = !isInterface ? baseType.GetInterfaces() :
                baseType.GetInterfaces().Concat(new[] { baseType }).ToArray();

            // 定义动态类型。
            var objectProxyType = module.DefineType(typeName,
                TypeAttributes.Class | TypeAttributes.Public |
                TypeAttributes.Serializable | TypeAttributes.BeforeFieldInit,
                parent, interfaces);
            this.ObjectProxyType = objectProxyType;
        }

        /// <summary>
        /// 以原型类型中构造函数为基础定义代理类型中的构造函数。
        /// </summary>
        private void DefineConstructors()
        {
            var baseType = this.PrototypeType;
            var objectProxyType = this.ObjectProxyType;

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
        /// 定义用于访问原型类型中对应方法的方法。
        /// </summary>
        private void DefineBaseAccessMethods()
        {
            var objectProxyType = this.ObjectProxyType;
            var baseMethods = this.BaseMethods;

            var baseAccessMethods = new Dictionary<MethodInfo, MethodBuilder>();
            for (int i = 0; i < baseMethods.Length; i++)
            {
                var baseMethod = baseMethods[i];
                var accessMethod = objectProxyType.DefineBaseAccessMethod(baseMethod);
                baseAccessMethods[baseMethod] = accessMethod;
            }
            this.BaseAccessMethods = baseAccessMethods;
        }

        /// <summary>
        /// 定义用于保存原型类型中定义的 <see cref="OnMethodInvokeAttribute"/> 特性的字段的类型。
        /// </summary>
        private void DefineAttributesType()
        {
            var attributesType = new AttributesTypeBuilder(this);
            this.OnMemberInvokeFields = attributesType.OnMemberInvokeFields;
            this.MethodsOnMethodInvokeFields = attributesType.MethodsOnMethodInvokeFields;
        }

        /// <summary>
        /// 定义包含代理方法和代理方法的 <see cref="MethodInvoker"/> 委托字段的类型。
        /// </summary>
        private void DefineDelegatesType()
        {
            var delegatesType = new DelegatesTypeBuilder(this);
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
        private void DefineProxyOverrideMethod(MethodInfo baseMethod)
        {
            var objectProxyType = this.ObjectProxyType;
            var proxyDelegateField = (FieldInfo)this.ProxyDelegateFields[baseMethod];
            bool newSlot = baseMethod.DeclaringType.IsInterface;

            // 对于没有任何代理特性的方法则定义默认重写方法并返回。
            if (proxyDelegateField is null)
            {
                objectProxyType.DefineDefaultOverrideMethod(baseMethod);
                return;
            }

            // 定义方法。
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
            bool hasReturns = baseReturnParam.ParameterType != typeof(void);
            var proxyMethod = objectProxyType.DefineMethod(baseMethod.Name,
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
                ilGen.Emit(OpCodes.Castclass, objectProxyType);
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
