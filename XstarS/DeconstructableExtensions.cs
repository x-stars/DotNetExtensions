using System;

namespace XstarS
{
    /// <summary>
    /// 提供可解构对象 <see cref="IDeconstructable"/> 的扩展方法。
    /// </summary>
    public static class DeconstructableExtensions
    {
        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable"/>
        /// 转换为空值元组 <see cref="ValueTuple"/>。
        /// </summary>
        /// <param name="source">一个 <see cref="IDeconstructable"/> 类型的对象。</param>
        /// <returns>转换后得到的空值元组 <see cref="ValueTuple"/>。</returns>
        public static ValueTuple ToValueTuple(
            this IDeconstructable source)
        {
            return new ValueTuple();
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1}"/>
        /// 转换为 1 元值元组 <see cref="ValueTuple{T1}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1}"/> 类型的对象。</param>
        /// <returns>转换后得到的 1 元值元组 <see cref="ValueTuple{T1}"/>。</returns>
        public static ValueTuple<T1> ToValueTuple<T1>(
            this IDeconstructable<T1> source)
        {
            source.Deconstruct(out var item1);
            return new ValueTuple<T1>(item1);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2}"/>
        /// 转换为 2 元值元组 <see cref="ValueTuple{T1, T2}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2}"/> 类型的对象。</param>
        /// <returns>转换后得到的 2 元值元组 <see cref="ValueTuple{T1, T2}"/>。</returns>
        public static (T1, T2) ToValueTuple<T1, T2>(
            this IDeconstructable<T1, T2> source)
        {
            var (item1, item2) = source;
            return (item1, item2);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3}"/>
        /// 转换为 3 元值元组 <see cref="ValueTuple{T1, T2, T3}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3}"/> 类型的对象。</param>
        /// <returns>转换后得到的 3 元值元组 <see cref="ValueTuple{T1, T2, T3}"/>。</returns>
        public static (T1, T2, T3) ToValueTuple<T1, T2, T3>(
            this IDeconstructable<T1, T2, T3> source)
        {
            var (item1, item2, item3) = source;
            return (item1, item2, item3);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3, T4}"/>
        /// 转换为 4 元值元组 <see cref="ValueTuple{T1, T2, T3, T4}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">第 4 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3, T4}"/> 类型的对象。</param>
        /// <returns>转换后得到的 4 元值元组 <see cref="ValueTuple{T1, T2, T3, T4}"/>。</returns>
        public static (T1, T2, T3, T4) ToValueTuple<T1, T2, T3, T4>(
            this IDeconstructable<T1, T2, T3, T4> source)
        {
            var (item1, item2, item3, item4) = source;
            return (item1, item2, item3, item4);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3, T4, T5}"/>
        /// 转换为 5 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">第 5 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3, T4, T5}"/> 类型的对象。</param>
        /// <returns>转换后得到的 5 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5}"/>。</returns>
        public static (T1, T2, T3, T4, T5) ToValueTuple<T1, T2, T3, T4, T5>(
            this IDeconstructable<T1, T2, T3, T4, T5> source)
        {
            var (item1, item2, item3, item4, item5) = source;
            return (item1, item2, item3, item4, item5);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6}"/>
        /// 转换为 6 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">第 6 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6}"/> 类型的对象。</param>
        /// <returns>转换后得到的 6 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/>。</returns>
        public static (T1, T2, T3, T4, T5, T6) ToValueTuple<T1, T2, T3, T4, T5, T6>(
            this IDeconstructable<T1, T2, T3, T4, T5, T6> source)
        {
            var (item1, item2, item3, item4, item5, item6) = source;
            return (item1, item2, item3, item4, item5, item6);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6, T7}"/>
        /// 转换为 7 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">第 6 个元素的类型。</typeparam>
        /// <typeparam name="T7">第 7 个元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6, T7}"/> 类型的对象。</param>
        /// <returns>转换后得到的 7 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/>。</returns>
        public static (T1, T2, T3, T4, T5, T6, T7) ToValueTuple<T1, T2, T3, T4, T5, T6, T7>(
            this IDeconstructable<T1, T2, T3, T4, T5, T6, T7> source)
        {
            var (item1, item2, item3, item4, item5, item6, item7) = source;
            return (item1, item2, item3, item4, item5, item6, item7);
        }

        /// <summary>
        /// 将可解构对象 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6, T7, TRest}"/>
        /// 转换为 n 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>。
        /// </summary>
        /// <typeparam name="T1">第 1 个元素的类型。</typeparam>
        /// <typeparam name="T2">第 2 个元素的类型。</typeparam>
        /// <typeparam name="T3">第 3 个元素的类型。</typeparam>
        /// <typeparam name="T4">第 4 个元素的类型。</typeparam>
        /// <typeparam name="T5">第 5 个元素的类型。</typeparam>
        /// <typeparam name="T6">第 6 个元素的类型。</typeparam>
        /// <typeparam name="T7">第 7 个元素的类型。</typeparam>
        /// <typeparam name="TRest">剩余元素的值元组类型。</typeparam>
        /// <param name="source">一个 <see cref="IDeconstructable{T1, T2, T3, T4, T5, T6, T7, TRest}"/> 类型的对象。</param>
        /// <returns>转换后得到的 n 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/>。</returns>
        public static (T1, T2, T3, T4, T5, T6, T7, TRest) ToValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
            this IDeconstructable<T1, T2, T3, T4, T5, T6, T7, TRest> source) where TRest : struct
        {
            var (item1, item2, item3, item4, item5, item6, item7, rest) = source;
            return (item1, item2, item3, item4, item5, item6, item7, rest);
        }
    }
}
