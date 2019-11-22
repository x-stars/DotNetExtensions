using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class ObservableTypeBuildingHelper
    {
        /// <summary>
        /// 以指定的属性为基础，定义调用此属性并触发属性更改事件的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="onPropertyChangedMethod">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法。</param>
        /// <returns>定义的属性更改通知属性，调用 <paramref name="baseProperty"/> 属性，
        /// 并在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是抽象属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static PropertyBuilder DefineObservableBaseInvokePropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, MethodInfo onPropertyChangedMethod)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (onPropertyChangedMethod is null)
            {
                throw new ArgumentNullException(nameof(onPropertyChangedMethod));
            }
            if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var rPropertyNames = baseProperty.GetCustomAttribute<
                RelatedPropertiesAttribute>()?.PropertyNames ?? Array.Empty<string>();

            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseMethod.GetParameters(), param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    il.EmitLdarg(i + 1);
                }
                il.Emit(OpCodes.Call, baseProperty.GetMethod);
                il.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    il.EmitLdarg(i + 1);
                }
                il.Emit(OpCodes.Call, baseProperty.SetMethod);
                var propertyName = (baseProperty.GetIndexParameters().Length == 0) ?
                    baseProperty.Name : $"{baseProperty.Name}[]";
                var eventArgsConstructor =
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) });
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, propertyName);
                il.Emit(OpCodes.Newobj, eventArgsConstructor);
                il.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                foreach (var rPropertyName in rPropertyNames)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, rPropertyName);
                    il.Emit(OpCodes.Newobj, eventArgsConstructor);
                    il.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                }
                il.Emit(OpCodes.Ret);

                property.SetSetMethod(method);
            }

            return property;
        }

        /// <summary>
        /// 以指定的属性为基础，定义以自动属性模式实现并触发属性更改事件的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="onPropertyChangedMethod">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法。</param>
        /// <returns>定义的属性更改通知自动属性及其对应的字段，
        /// 在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineObservableAutoPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, MethodInfo onPropertyChangedMethod)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (onPropertyChangedMethod is null)
            {
                throw new ArgumentNullException(nameof(onPropertyChangedMethod));
            }
            if (baseProperty.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var rPropertyNames = baseProperty.GetCustomAttribute<
                RelatedPropertiesAttribute>()?.PropertyNames ?? Array.Empty<string>();

            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            var field = type.DefineField(
                baseProperty.Name, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseMethod.GetParameters(), param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, field);
                var eventArgsConstructor =
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) });
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldstr, baseProperty.Name);
                il.Emit(OpCodes.Newobj, eventArgsConstructor);
                il.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                foreach (var rPropertyName in rPropertyNames)
                {
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldstr, rPropertyName);
                    il.Emit(OpCodes.Newobj, eventArgsConstructor);
                    il.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                }
                il.Emit(OpCodes.Ret);

                property.SetSetMethod(method);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 定义 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="propertyChangedField">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托的字段。</param>
        /// <returns>定义的 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法。</returns>
        /// <exception cref="ArgumentException"><paramref name="propertyChangedField"/>
        /// 的类型不为 <see cref="PropertyChangedEventHandler"/>。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineOnPropertyChangedMethod(
            this TypeBuilder type, FieldInfo propertyChangedField)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (propertyChangedField is null)
            {
                throw new ArgumentNullException(nameof(propertyChangedField));
            }
            if (propertyChangedField.FieldType != typeof(PropertyChangedEventHandler))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(propertyChangedField));
            }

            var method = type.DefineMethod("OnPropertyChanged",
                MethodAttributes.Family | MethodAttributes.Virtual |
                MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                typeof(void), new[] { typeof(PropertyChangedEventArgs) });

            method.DefineParameter(1, ParameterAttributes.None, "e");

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, propertyChangedField);
            il.Emit(OpCodes.Dup);
            var labelInvoke = il.DefineLabel();
            il.Emit(OpCodes.Brtrue_S, labelInvoke);
            il.Emit(OpCodes.Pop);
            il.Emit(OpCodes.Ret);
            il.MarkLabel(labelInvoke);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod(
                nameof(PropertyChangedEventHandler.Invoke),
                new[] { typeof(object), typeof(PropertyChangedEventArgs) }));
            il.Emit(OpCodes.Ret);

            return method;
        }
    }
}
