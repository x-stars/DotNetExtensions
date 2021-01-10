using System;

namespace XstarS.Text
{
    /// <summary>
    /// 提供解析字符串的扩展方法。
    /// </summary>
    public static class StringParseExtensions
    {
        /// <summary>
        /// 提供类似于 <see cref="int.Parse(string)"/> 的字符串解析方法的委托。
        /// </summary>
        /// <typeparam name="T">要解析为的数值类型。</typeparam>
        private static class ParseMethod<T>
        {
            /// <summary>
            /// 表示 <typeparamref name="T"/> 类型的字符串解析方法的委托。
            /// </summary>
            internal static readonly Converter<string, T> Delegate = ParseMethod<T>.CreateDelegate();

            /// <summary>
            /// 创建 <typeparamref name="T"/> 类型的字符串解析方法的委托。
            /// </summary>
            /// <returns><typeparamref name="T"/> 类型的字符串解析方法的委托。</returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            private static Converter<string, T> CreateDelegate()
            {
                try
                {
                    var method = typeof(T).GetMethod(nameof(int.Parse), new[] { typeof(string) });
                    return (!(method is null) && method.IsStatic && (method.ReturnType == typeof(T))) ?
                        (Converter<string, T>)method.CreateDelegate(typeof(Converter<string, T>)) : null;
                }
                catch (Exception) { return null; }
            }
        }

        /// <summary>
        /// 将当前字符串表示形式转换为其等效的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型，
        /// 应为枚举类型或包含类似于 <see cref="int.Parse(string)"/> 的方法。</typeparam>
        /// <param name="text">包含要转换数值的字符串。</param>
        /// <returns>与 <paramref name="text"/> 等效的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="text"/> 不表示有效的值。</exception>
        /// <exception cref="FormatException"><paramref name="text"/> 的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// <paramref name="text"/> 表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        public static T ParseAs<T>(this string text)
        {
            return !(ParseMethod<T>.Delegate is null) ? ParseMethod<T>.Delegate.Invoke(text) :
                typeof(T).IsEnum ? (T)Enum.Parse(typeof(T), text) : throw new InvalidCastException();
        }
    }
}
