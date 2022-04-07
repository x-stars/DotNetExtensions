using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.ComponentModel;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供属性更改通知类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class ObservableTypeBuildingHelpers
    {
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
                var inner = new MissingFieldException();
                throw new ArgumentException(inner.Message, nameof(propertyChangedField), inner);
            }

            var method = type.DefineMethod("OnPropertyChanged",
                MethodAttributes.Family | MethodAttributes.Virtual |
                MethodAttributes.HideBySig | MethodAttributes.NewSlot,
                typeof(void), new[] { typeof(PropertyChangedEventArgs) });

            method.DefineParameter(1, ParameterAttributes.None, "e");

            var ilGen = method.GetILGenerator();
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldfld, propertyChangedField);
            ilGen.Emit(OpCodes.Dup);
            var invokeLabel = ilGen.DefineLabel();
            ilGen.Emit(OpCodes.Brtrue_S, invokeLabel);
            ilGen.Emit(OpCodes.Pop);
            ilGen.Emit(OpCodes.Ret);
            ilGen.MarkLabel(invokeLabel);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.Emit(OpCodes.Callvirt,
                typeof(PropertyChangedEventHandler).GetMethod(
                    nameof(PropertyChangedEventHandler.Invoke),
                    new[] { typeof(object), typeof(PropertyChangedEventArgs) })!);
            ilGen.Emit(OpCodes.Ret);

            return method;
        }

        /// <summary>
        /// 以指定的属性为基础，定义调用此属性并触发属性更改事件的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="onPropertyChangedMethod">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的触发方法。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的属性更改通知属性，调用 <paramref name="baseProperty"/> 属性，
        /// 并在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是抽象属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static PropertyBuilder DefineObservableBaseInvokePropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty,
            MethodInfo onPropertyChangedMethod, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }

            var rPropertyNames = baseProperty.GetCustomAttribute<
                RelatedPropertiesAttribute>()?.PropertyNames ?? Array.Empty<string>();

            var propertyName = baseProperty.Name;
            if (explicitOverride)
            {
                var baseHandle = baseProperty.GetAccessors().First().MethodHandle;
                propertyName += $"#{baseHandle.Value.ToString()}";
            }

            var property = type.DefineProperty(
                propertyName, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod!;

                var method = type.DefineMethodOverride(baseMethod);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int index = 0; index < baseMethod.GetParameters().Length; index++)
                {
                    ilGen.EmitLdarg(index + 1);
                }
                ilGen.Emit(OpCodes.Call, baseMethod);
                ilGen.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod!;

                var method = type.DefineMethodOverride(baseMethod);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int index = 0; index < baseMethod.GetParameters().Length; index++)
                {
                    ilGen.EmitLdarg(index + 1);
                }
                ilGen.Emit(OpCodes.Call, baseMethod);
                var propertyNotifyName = (baseProperty.GetIndexParameters().Length == 0) ?
                    baseProperty.Name : $"{baseProperty.Name}[]";
                var eventArgsConstructor =
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) })!;
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldstr, propertyNotifyName);
                ilGen.Emit(OpCodes.Newobj, eventArgsConstructor);
                ilGen.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                foreach (var rPropertyName in rPropertyNames)
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldstr, rPropertyName);
                    ilGen.Emit(OpCodes.Newobj, eventArgsConstructor);
                    ilGen.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                }
                ilGen.Emit(OpCodes.Ret);

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
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的属性更改通知自动属性及其对应的字段，
        /// 在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineObservableAutoPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty,
            MethodInfo onPropertyChangedMethod, bool explicitOverride = false)
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
                var inner = new TargetParameterCountException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }

            var rPropertyNames = baseProperty.GetCustomAttribute<
                RelatedPropertiesAttribute>()?.PropertyNames ?? Array.Empty<string>();

            var propertyName = baseProperty.Name;
            if (explicitOverride)
            {
                var baseHandle = baseProperty.GetAccessors().First().MethodHandle;
                propertyName += $"#{baseHandle.Value.ToString()}";
            }

            var property = type.DefineProperty(
                propertyName, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            var field = type.DefineField(
                propertyName, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Stfld, field);
                var eventArgsConstructor =
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) })!;
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldstr, baseProperty.Name);
                ilGen.Emit(OpCodes.Newobj, eventArgsConstructor);
                ilGen.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                foreach (var rPropertyName in rPropertyNames)
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldstr, rPropertyName);
                    ilGen.Emit(OpCodes.Newobj, eventArgsConstructor);
                    ilGen.Emit(OpCodes.Callvirt, onPropertyChangedMethod);
                }
                ilGen.Emit(OpCodes.Ret);

                property.SetSetMethod(method);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }
    }
}
