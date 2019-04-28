using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    public static partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 反转当前比较器 <see cref="IComparer{T}"/> 并返回。
        /// </summary>
        /// <typeparam name="T"><see cref="IComparer{T}"/> 要比较的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="IComparer{T}"/> 对象。</param>
        /// <returns>将 <paramref name="source"/> 反转后得到的 <see cref="IComparer{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IComparer<T> Reverse<T>(IComparer<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReversedComparer<T>(source);
        }
    }
}
