using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Linq
{
    /// <summary>
    /// 提供 LINQ 查询方法的扩展。
    /// </summary>
    public static class LinqExtensions
    {
#if NET461
        /// <summary>
        /// 将一个值插入到序列末尾。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要在末尾插入值的序列。</param>
        /// <param name="element">要插入到 <paramref name="source"/> 末尾的值。</param>
        /// <returns>以 <paramref name="element"/> 结尾的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return source.Concat(new[] { element });
        }
#endif

        /// <summary>
        /// 将另一个值插入到序列的指定位置。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="source">要插入值的序列。</param>
        /// <param name="index">要在 <paramref name="source"/> 中插入值的位置。</param>
        /// <param name="element">要插入到 <paramref name="source"/> 的值。</param>
        /// <returns>在 <paramref name="source"/> 的 <paramref name="index"/>
        /// 处插入 <paramref name="element"/> 得到的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> Insert<TSource>(
            this IEnumerable<TSource> source, int index, TSource element)
        {
            return source.Take(index).Append(element).Concat(source.Skip(index));
        }

        /// <summary>
        /// 将一个值插入到序列开头。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要在开头插入值的序列。</param>
        /// <param name="element">要插入到 <paramref name="source"/> 开头的值。</param>
        /// <returns>以 <paramref name="element"/> 开头的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> InsertHead<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return (new[] { element }).Concat(source);
        }

        /// <summary>
        /// 将另一个序列插入到序列的指定位置。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="source">要插入另一序列的序列。</param>
        /// <param name="index">要在 <paramref name="source"/> 中插入序列的位置。</param>
        /// <param name="other">要插入到 <paramref name="source"/> 的序列。</param>
        /// <returns>在 <paramref name="source"/> 的 <paramref name="index"/>
        /// 处插入 <paramref name="other"/> 得到的新序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> InsertRange<TSource>(
            this IEnumerable<TSource> source, int index, IEnumerable<TSource> other)
        {
            return source.Take(index).Concat(other).Concat(source.Skip(index));
        }

        /// <summary>
        /// 将当前序列与另一序列的元素对应连接，得到 2 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的元素的类型。</typeparam>
        /// <typeparam name="T2">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的序列。</param>
        /// <param name="other">要与当前序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 2 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2)> Zip<T1, T2>(
            this IEnumerable<T1> source, IEnumerable<T2> other)
        {
            return Enumerable.Zip(source, other, ValueTuple.Create);
        }

        /// <summary>
        /// 将当前 2 元组序列与另一序列的元素对应连接，得到 3 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 2 元组序列。</param>
        /// <param name="other">要与当前 2 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 3 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>(
            this IEnumerable<(T1, T2)> source, IEnumerable<T3> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 3 元组序列与另一序列的元素对应连接，得到 4 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 3 元组序列。</param>
        /// <param name="other">要与当前 3 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 4 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>(
            this IEnumerable<(T1, T2, T3)> source, IEnumerable<T4> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 4 元组序列与另一序列的元素对应连接，得到 5 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">当前序列的第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 4 元组序列。</param>
        /// <param name="other">要与当前 4 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 5 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>(
            this IEnumerable<(T1, T2, T3, T4)> source, IEnumerable<T5> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 5 元组序列与另一序列的元素对应连接，得到 6 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">当前序列的第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">当前序列的第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 5 元组序列。</param>
        /// <param name="other">要与当前 5 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 6 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>(
            this IEnumerable<(T1, T2, T3, T4, T5)> source, IEnumerable<T6> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 6 元组序列与另一序列的元素对应连接，得到 7 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">当前序列的第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">当前序列的第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">当前序列的第 6 个元素的类型。</typeparam>
        /// <typeparam name="T7">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 6 元组序列。</param>
        /// <param name="other">要与当前 6 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 7 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> Zip<T1, T2, T3, T4, T5, T6, T7>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6)> source, IEnumerable<T7> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 7 元组序列与另一序列的元素对应连接，得到 8 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">当前序列的第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">当前序列的第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">当前序列的第 6 个元素的类型。</typeparam>
        /// <typeparam name="T7">当前序列的第 7 个元素的类型。</typeparam>
        /// <typeparam name="T8">另一序列的元素的类型。</typeparam>
        /// <param name="source">要与另一序列的元素对应连接的 7 元组序列。</param>
        /// <param name="other">要与当前 7 元组序列的元素对应连接的序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 8 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> Zip<T1, T2, T3, T4, T5, T6, T7, T8>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> source, IEnumerable<T8> other)
        {
            return Enumerable.Zip(source, other, TupleOperators.Append);
        }

        /// <summary>
        /// 将当前 7 元组序列与另一元组序列的元素对应连接，得到 n 元组的结果序列。
        /// </summary>
        /// <typeparam name="T1">当前序列的第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">当前序列的第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">当前序列的第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">当前序列的第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">当前序列的第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">当前序列的第 6 个元素的类型。</typeparam>
        /// <typeparam name="T7">当前序列的第 7 个元素的类型。</typeparam>
        /// <typeparam name="TRest">另一元组序列的元组的类型。</typeparam>
        /// <param name="source">要与另一元组序列的元素对应连接的 7 元组序列。</param>
        /// <param name="other">要与当前 7 元组序列的元素对应连接的元组序列。</param>
        /// <returns>一个包含 <paramref name="source"/> 和
        /// <paramref name="other"/> 中对应元素组成的 n 元组的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>> ZipChain<T1, T2, T3, T4, T5, T6, T7, TRest>(
            this IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> source, IEnumerable<TRest> other) where TRest : struct
        {
            return Enumerable.Zip(source, other, TupleOperators.Concat);
        }
    }
}
