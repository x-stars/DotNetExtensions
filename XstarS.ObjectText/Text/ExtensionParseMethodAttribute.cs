using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace XstarS.Text
{
    /// <summary>
    /// 指示当前方法是用于将字符串解析为指定类型的对象的扩展字符串解析方法。
    /// 应用此特性的方法应具有类似于 <see cref="int.Parse(string)"/> 的方法签名。
    /// </summary>
    /// <remarks>
    /// 应用此特性的方法应定义于公共静态非嵌套非泛型类型，且方法本身也应为公共静态非泛型方法，
    /// 否则将无法应用于 <see cref="StringParser{T}.Default"/> 属性；
    /// 当存在多个应用于相同类型的扩展字符串解析方法时，仅使用第一个声明此特性的方法。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public sealed class ExtensionParseMethodAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="ExtensionParseMethodAttribute"/> 类的新实例。
        /// </summary>
        public ExtensionParseMethodAttribute() { }

        /// <summary>
        /// 尝试获取指定类型的标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法。
        /// </summary>
        /// <param name="type">要查找扩展字符串解析方法的类型。</param>
        /// <param name="method"><paramref name="type"/> 类型的扩展字符串解析方法；
        /// 若无法找到对应的扩展字符串解析方法，则为 <see langword="null"/>。</param>
        /// <returns>若能够找到 <paramref name="type"/> 类型的扩展字符串解析方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        internal static bool TryGetParseMethod(Type type, [NotNullWhen(true)] out MethodInfo? method)
        {
            return ParseMethods.LookupTable.TryGetValue(type, out method);
        }

        /// <summary>
        /// 提供标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法。
        /// </summary>
        private static class ParseMethods
        {
            /// <summary>
            /// 表示标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法查找表。
            /// </summary>
            internal static readonly ConcurrentDictionary<Type, MethodInfo> LookupTable =
                ExtensionParseMethodAttribute.ParseMethods.CreateLookupTable();

            /// <summary>
            /// 创建标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法查找表。
            /// </summary>
            /// <returns>标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法查找表。</returns>
            private static ConcurrentDictionary<Type, MethodInfo> CreateLookupTable()
            {
                AppDomain.CurrentDomain.AssemblyLoad += ParseMethods.UpdateLookupTableOnAssemblyLoad;
                var lookupTable = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => !assembly.IsDynamic)
                    .SelectMany(assembly => assembly.GetExportedTypes())
                    .Where(type => type.IsAbstract && type.IsSealed)
                    .Where(type => !type.IsNested && !type.IsGenericType)
                    .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    .Where(method => !method.IsGenericMethod)
                    .Where(method => method.IsDefined(typeof(ExtensionParseMethodAttribute)))
                    .GroupBy(method => method.ReturnType)
                    .ToDictionary(methods => methods.Key, methods => methods.First());
                return new ConcurrentDictionary<Type, MethodInfo>(lookupTable);
            }

            /// <summary>
            /// 当加载程序集时，查找程序集中标记为 <see cref="ExtensionParseMethodAttribute"/> 的扩展字符串解析方法，
            /// 并更新到 <see cref="ParseMethods.LookupTable"/> 扩展字符串解析方法查找表中。
            /// </summary>
            /// <param name="sender">加载程序集事件的事件源。</param>
            /// <param name="args">提供程序集事件的事件数据的对象。</param>
            private static void UpdateLookupTableOnAssemblyLoad(object? sender, AssemblyLoadEventArgs args)
            {
                if (args.LoadedAssembly.IsDynamic) { return; }
                var lookupTable = ParseMethods.LookupTable;
                var methods = args.LoadedAssembly.GetExportedTypes()
                    .Where(type => type.IsAbstract && type.IsSealed)
                    .Where(type => !type.IsNested && !type.IsGenericType)
                    .SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    .Where(method => !method.IsGenericMethod)
                    .Where(method => method.IsDefined(typeof(ExtensionParseMethodAttribute)));
                foreach (var method in methods)
                {
                    lookupTable.TryAdd(method.ReturnType, method);
                }
            }
        }
    }
}
