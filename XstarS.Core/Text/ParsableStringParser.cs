using System;
using System.Runtime.CompilerServices;

namespace XstarS.Text
{
    /// <summary>
    /// 表示使用类似于 <see cref="int.Parse(string)"/> 的方法将字符串转换为对象的字符串解析对象。
    /// </summary>
    /// <typeparam name="T">要从字符串解析为对象的类型。</typeparam>
    internal sealed class ParsableStringParser<T> : SimpleStringParser<T> where T : notnull
    {
        /// <summary>
        /// 初始化 <see cref="ParsableStringParser{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <typeparamref name="T"/> 类型不提供解析字符串的静态方法。</exception>
        public ParsableStringParser()
        {
            if (!ParsableStringParser<T>.CanParse)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 获取当前类型是否包含类似于 <see cref="int.Parse(string)"/> 的方法。
        /// </summary>
        /// <returns>若当前类型包含类似于 <see cref="int.Parse(string)"/> 的方法；
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
        public override T Parse(string text) => ParseMethod.Delegate!.Invoke(text)!;

        /// <summary>
        /// 提供类似于 <see cref="int.Parse(string)"/> 的方法的委托。
        /// </summary>
        private static class ParseMethod
        {
            /// <summary>
            /// 表示将字符串解析为指定类型的对象的方法的委托。
            /// </summary>
            internal static readonly Converter<string, T>? Delegate = ParseMethod.CreateDelegate();

            /// <summary>
            /// 创建 <typeparamref name="T"/> 类型的字符串解析方法的委托。
            /// </summary>
            /// <returns><typeparamref name="T"/> 类型的字符串解析方法的委托。</returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage(
                "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            private static Converter<string, T>? CreateDelegate()
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
    }
}
