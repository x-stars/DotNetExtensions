using System;
using System.Runtime.CompilerServices;

namespace XstarS.Text
{
    /// <summary>
    /// 表示使用标记为 <see cref="ExtensionParseMethodAttribute"/> 的方法将字符串转换为对象的字符串解析对象。
    /// </summary>
    /// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
    [Serializable]
    internal sealed class ExtensionStringParser<T> : SimpleStringParser<T> where T : notnull
    {
        /// <summary>
        /// 初始化 <see cref="ExtensionStringParser{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// 没有适用于 <typeparamref name="T"/> 类型的扩展字符串解析方法。</exception>
        public ExtensionStringParser()
        {
            if (!ExtensionStringParser<T>.CanParse)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 获取是否有标记为 <see cref="ExtensionParseMethodAttribute"/> 并适用于当前类型的方法。
        /// </summary>
        /// <returns>若有标记为 <see cref="ExtensionParseMethodAttribute"/> 并适用于当前类型的方法；
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        internal static bool CanParse
        {
            [MethodImpl(MethodImplOptions.NoInlining)]
            get => !(ParseMethod.Delegate is null);
        }

        /// <summary>
        /// 将指定的字符串表示形式转换为其等效的对象。
        /// </summary>
        /// <param name="text">包含要转换的对象的字符串。</param>
        /// <returns>与 <paramref name="text"/> 中的内容等效的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public override T Parse(string text) => ParseMethod.Delegate!.Invoke(text);

        /// <summary>
        /// 提供标记为 <see cref="ExtensionParseMethodAttribute"/> 的方法的委托。
        /// </summary>
        private static class ParseMethod
        {
            /// <summary>
            /// 表示将字符串解析为指定类型的对象的扩展方法的委托。
            /// </summary>
            internal static readonly Converter<string, T>? Delegate = ParseMethod.CreateDelegate();

            /// <summary>
            /// 创建 <typeparamref name="T"/> 类型的扩展字符串解析方法的委托。
            /// </summary>
            /// <returns><typeparamref name="T"/> 类型的扩展字符串解析方法的委托。</returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            private static Converter<string, T>? CreateDelegate()
            {
                try
                {
                    return ExtensionParseMethodAttribute.TryGetParseMethod(typeof(T), out var method) ?
                        (Converter<string, T>)method.CreateDelegate(typeof(Converter<string, T>)) : null;
                }
                catch (Exception) { return null; }
            }
        }
    }
}
