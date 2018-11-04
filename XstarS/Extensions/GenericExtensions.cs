using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS
{
    /// <summary>
    /// 提供类型无关的通用扩展方法。
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        /// 返回当前对象的字符串表达形式。
        /// 若当前对象为 <see langword="null"/>，则返回 <see cref="string.Empty"/>。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 的类型。</typeparam>
        /// <param name="source">一个 <typeparamref name="T"/> 类型的对象。</param>
        /// <returns><paramref name="source"/> 的字符串表达形式；
        /// 若 <paramref name="source"/> 为 <see langword="null"/>，
        /// 则为 <see cref="string.Empty"/>。</returns>
        public static string ToStringOrEmpty<T>(this T source) where T : class =>
            (source is null) ? string.Empty : source.ToString();
    }
}
