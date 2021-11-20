using System;
using System.Linq.Expressions;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供创建类型成员调用委托的扩展方法。
    /// </summary>
    internal static class MemberDelegateExtensions
    {
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
            var castExpr = method.IsStatic ? null : Expression.Convert(instExpr, method.DeclaringType!);
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
