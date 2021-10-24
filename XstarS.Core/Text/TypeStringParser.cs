using System;
using System.Reflection;

namespace XstarS.Text
{
    /// <summary>
    /// 表示将类型的完整名称转换为其等效的类型声明的字符串解析对象。
    /// </summary>
    [Serializable]
    internal sealed class TypeStringParser : SimpleStringParser<Type>
    {
        /// <summary>
        /// 初始化 <see cref="TypeStringParser"/> 类的新实例。
        /// </summary>
        public TypeStringParser() { }

        /// <summary>
        /// 将指定的类型的完整名称转换为其等效的类型声明。
        /// </summary>
        /// <param name="text">类型的完整名称的字符串。</param>
        /// <returns>名为 <paramref name="text"/> 的类型声明。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// 在当前应用程序域中无法找到名为 <paramref name="text"/> 的类型。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public override Type Parse(string text)
        {
            if (text is null) { throw new ArgumentNullException(nameof(text)); }
            try
            {
                return Type.GetType(text) ?? TypeStringParser.FindType(text) ??
                    throw new TypeLoadException();
            }
            catch (Exception e) { throw new ArgumentException(e.Message, e); }
        }

        /// <summary>
        /// 在当前应用程序域中查找具有指定名称的类型。
        /// </summary>
        /// <param name="typeName">类型的完整名称。</param>
        /// <returns>名为 <paramref name="typeName"/> 的类型。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeLoadException">
        /// 在当前应用程序域中无法找到名为 <paramref name="typeName"/> 的类型。</exception>
        private static Type? FindType(string typeName)
        {
            return Type.GetType(typeName,
                assemblyResolver: null,
                typeResolver: TypeStringParser.FindType,
                throwOnError: true,
                ignoreCase: false);
        }

        /// <summary>
        /// 在当前应用程序域中查找具有指定名称的类型。
        /// </summary>
        /// <param name="unused">不使用此参数。</param>
        /// <param name="typeName">类型的完整名称。</param>
        /// <param name="ignoreCase">指定查找时是否忽略类型名称的大小写。。</param>
        /// <returns>名为 <paramref name="typeName"/> 的类型；
        /// 若未找到匹配的类型，则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="typeName"/> 为 <see langword="null"/>。</exception>
        private static Type? FindType(
            Assembly? unused, string typeName, bool ignoreCase)
        {
            var domain = AppDomain.CurrentDomain;
            var assemblies = domain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName,
                    throwOnError: false, ignoreCase);
                if (!(type is null)) { return type; }
            }
            return null;
        }
    }
}
