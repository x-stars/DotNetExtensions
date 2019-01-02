using System;

namespace XstarS
{
    /// <summary>
    /// 可解构对象，支持将对象作为空值元组 <see cref="ValueTuple"/> 解构。
    /// </summary>
    public interface IDeconstructable
    {
        /// <summary>
        /// 将当前对象作为空值元组 <see cref="ValueTuple"/> 解构。
        /// </summary>
        void Deconstruct();
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 1 元值元组 <see cref="ValueTuple{T1}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1>
    {
        /// <summary>
        /// 将当前对象作为 1 元值元组 <see cref="ValueTuple{T1}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        void Deconstruct(out T1 item1);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 2 元值元组 <see cref="ValueTuple{T1, T2}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2>
    {
        /// <summary>
        /// 将当前对象作为 2 元值元组 <see cref="ValueTuple{T1, T2}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 3 元值元组 <see cref="ValueTuple{T1, T2, T3}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3>
    {
        /// <summary>
        /// 将当前对象作为 3 元值元组 <see cref="ValueTuple{T1, T2, T3}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 4 元值元组 <see cref="ValueTuple{T1, T2, T3, T4}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3, T4>
    {
        /// <summary>
        /// 将当前对象作为 4 元值元组 <see cref="ValueTuple{T1, T2, T3, T4}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        /// <param name="item4">值元组的第 4 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 5 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// 将当前对象作为 5 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        /// <param name="item4">值元组的第 4 个元素。</param>
        /// <param name="item5">值元组的第 5 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 6 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// 将当前对象作为 6 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        /// <param name="item4">值元组的第 4 个元素。</param>
        /// <param name="item5">值元组的第 5 个元素。</param>
        /// <param name="item6">值元组的第 6 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 7 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3, T4, T5, T6, T7>
    {
        /// <summary>
        /// 将当前对象作为 7 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        /// <param name="item4">值元组的第 4 个元素。</param>
        /// <param name="item5">值元组的第 5 个元素。</param>
        /// <param name="item6">值元组的第 6 个元素。</param>
        /// <param name="item7">值元组的第 7 个元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7);
    }

    /// <summary>
    /// 可解构对象，支持将对象作为 n 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> 解构。
    /// </summary>
    public interface IDeconstructable<T1, T2, T3, T4, T5, T6, T7, TRest> where TRest : struct
    {
        /// <summary>
        /// 将当前对象作为 n 元值元组 <see cref="ValueTuple{T1, T2, T3, T4, T5, T6, T7, TRest}"/> 解构。
        /// </summary>
        /// <param name="item1">值元组的第 1 个元素。</param>
        /// <param name="item2">值元组的第 2 个元素。</param>
        /// <param name="item3">值元组的第 3 个元素。</param>
        /// <param name="item4">值元组的第 4 个元素。</param>
        /// <param name="item5">值元组的第 5 个元素。</param>
        /// <param name="item6">值元组的第 6 个元素。</param>
        /// <param name="item7">值元组的第 7 个元素。</param>
        /// <param name="rest">值元组的剩余元素。</param>
        void Deconstruct(out T1 item1, out T2 item2, out T3 item3, out T4 item4, out T5 item5, out T6 item6, out T7 item7, out TRest rest);
    }
}
