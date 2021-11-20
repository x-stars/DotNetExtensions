using System;
using System.Linq.Expressions;
using System.Reflection;

namespace XstarS.Reflection
{
    public static partial class MemberDelegateExtensions
    {
        /// <summary>
        /// 从当前构造函数创建动态调用委托。
        /// </summary>
        /// <param name="constructor">要创建委托的 <see cref="ConstructorInfo"/>。</param>
        /// <returns><paramref name="constructor"/> 构造函数的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="constructor"/> 为 <see langword="null"/>。</exception>
        public static Func<object[], object> CreateDynamicDelegate(this ConstructorInfo constructor)
        {
            if (constructor is null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }
            if (constructor.IsStatic)
            {
                var inner = new InvalidOperationException();
                throw new ArgumentException(inner.Message, nameof(constructor), inner);
            }

            var argsExpr = Expression.Parameter(typeof(object[]), "arguments");
            var paramInfos = constructor.GetParameters();
            var argCastExprs = new Expression[paramInfos.Length];
            for (int index = 0; index < argCastExprs.Length; index++)
            {
                var indexExpr = Expression.Constant(index);
                var argsItemExpr = Expression.ArrayIndex(argsExpr, indexExpr);
                var paramType = paramInfos[index].ParameterType;
                var argCastExpr = Expression.Convert(argsItemExpr, paramType);
                argCastExprs[index] = argCastExpr;
            }
            var newExpr = Expression.New(constructor, argCastExprs);
            var boxExpr = Expression.Convert(newExpr, typeof(object));
            var lambda = Expression.Lambda<Func<object[], object>>(boxExpr, argsExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 从当前字段创建指定类型的获取值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Func<object, object> CreateDynamicGetDelegate(this FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var instExpr = Expression.Parameter(typeof(object), "instance");
            var castExpr = field.IsStatic ? null : Expression.Convert(instExpr, field.DeclaringType);
            var fieldExpr = Expression.Field(castExpr, field);
            var boxExpr = Expression.Convert(fieldExpr, typeof(object));
            var lambda = Expression.Lambda<Func<object, object>>(boxExpr, instExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的获取值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Func<object> CreateDynamicGetDelegate(this FieldInfo field, object target)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var instExpr = field.IsStatic ? null : Expression.Constant(target);
            var fieldExpr = Expression.Field(instExpr, field);
            var boxExpr = Expression.Convert(fieldExpr, typeof(object));
            var lambda = Expression.Lambda<Func<object>>(boxExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 从当前字段创建指定类型的设置值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建设置值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Action<object, object> CreateDynamicSetDelegate(this FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var instExpr = Expression.Parameter(typeof(object), "instance");
            var castExpr = field.IsStatic ? null : Expression.Convert(instExpr, field.DeclaringType);
            var fieldExpr = Expression.Field(castExpr, field);
            var valueExpr = Expression.Parameter(typeof(object), "value");
            var valueCastExpr = Expression.Convert(valueExpr, field.FieldType);
            var assignExpr = Expression.Assign(fieldExpr, valueCastExpr);
            var lambda = Expression.Lambda<Action<object, object>>(assignExpr, instExpr, valueExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的设置值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Action<object> CreateDynamicSetDelegate(this FieldInfo field, object target)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var instExpr = field.IsStatic ? null : Expression.Constant(target);
            var fieldExpr = Expression.Field(instExpr, field);
            var valueExpr = Expression.Parameter(typeof(object), "value");
            var valueCastExpr = Expression.Convert(valueExpr, field.FieldType);
            var assignExpr = Expression.Assign(fieldExpr, valueCastExpr);
            var lambda = Expression.Lambda<Action<object>>(assignExpr, valueExpr);
            return lambda.Compile();
        }

        /// <summary>
        /// 从当前方法创建指定类型的动态调用委托。
        /// </summary>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static Func<object, object[], object> CreateDynamicDelegate(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var instExpr = Expression.Parameter(typeof(object), "instance");
            var castExpr = method.IsStatic ? null : Expression.Convert(instExpr, method.DeclaringType);
            var argsExpr = Expression.Parameter(typeof(object[]), "arguments");
            var paramInfos = method.GetParameters();
            var argCastExprs = new Expression[paramInfos.Length];
            for (int index = 0; index < argCastExprs.Length; index++)
            {
                var indexExpr = Expression.Constant(index);
                var argsItemExpr = Expression.ArrayIndex(argsExpr, indexExpr);
                var paramType = paramInfos[index].ParameterType;
                var argCastExpr = Expression.Convert(argsItemExpr, paramType);
                argCastExprs[index] = argCastExpr;
            }
            if (method.ReturnType == typeof(void))
            {
                var callExpr = Expression.Call(castExpr, method, argCastExprs);
                var lambda = Expression.Lambda<Action<object, object[]>>(callExpr, instExpr, argsExpr);
                var innerAction = lambda.Compile();
                return (instance, arguments) => { innerAction.Invoke(instance, arguments); return null; };
            }
            else
            {
                var callExpr = Expression.Call(castExpr, method, argCastExprs);
                var lambda = Expression.Lambda<Func<object, object[], object>>(callExpr, instExpr, argsExpr);
                return lambda.Compile();
            }
        }

        /// <summary>
        /// 从当前方法创建具有指定目标的指定类型的动态调用委托。
        /// </summary>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static Func<object[], object> CreateDynamicDelegate(this MethodInfo method, object target)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var instExpr = method.IsStatic ? null : Expression.Constant(target);
            var argsExpr = Expression.Parameter(typeof(object[]), "arguments");
            var paramInfos = method.GetParameters();
            var argCastExprs = new Expression[paramInfos.Length];
            for (int index = 0; index < argCastExprs.Length; index++)
            {
                var indexExpr = Expression.Constant(index);
                var argsItemExpr = Expression.ArrayIndex(argsExpr, indexExpr);
                var paramType = paramInfos[index].ParameterType;
                var argCastExpr = Expression.Convert(argsItemExpr, paramType);
                argCastExprs[index] = argCastExpr;
            }
            if (method.ReturnType == typeof(void))
            {
                var callExpr = Expression.Call(instExpr, method, argCastExprs);
                var lambda = Expression.Lambda<Action<object[]>>(callExpr, argsExpr);
                var innerAction = lambda.Compile();
                return arguments => { innerAction.Invoke(arguments); return null; };
            }
            else
            {
                var callExpr = Expression.Call(instExpr, method, argCastExprs);
                var lambda = Expression.Lambda<Func<object[], object>>(callExpr, argsExpr);
                return lambda.Compile();
            }
        }
    }
}
