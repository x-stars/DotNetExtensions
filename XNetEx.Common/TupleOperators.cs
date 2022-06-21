using System;

namespace XNetEx;

/// <summary>
/// 提供值元组的扩展运算符。
/// </summary>
public static class TupleOperators
{
    /// <summary>
    /// 返回将指定元素附加到当前空值元组的末尾得到的新的 1 值元组。
    /// </summary>
    /// <typeparam name="T1">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的空值元组。</param>
    /// <param name="item1">要附加到当前空值元组末尾的值。</param>
    /// <returns>将 <paramref name="item1"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 1 值元组。</returns>
    public static ValueTuple<T1> Append<T1>(
        this ValueTuple tuple, T1 item1)
    {
        return new ValueTuple<T1>(item1);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 1 值元组的末尾得到的新的 2 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 1 值元组。</param>
    /// <param name="item2">要附加到 1 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item2"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 2 值元组。</returns>
    public static (T1, T2) Append<T1, T2>(
        this ValueTuple<T1> tuple, T2 item2)
    {
        var item1 = tuple.Item1;
        return (item1, item2);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 2 值元组的末尾得到的新的 3 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 2 值元组。</param>
    /// <param name="item3">要附加到 2 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item3"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 3 值元组。</returns>
    public static (T1, T2, T3) Append<T1, T2, T3>(
        this (T1, T2) tuple, T3 item3)
    {
        var (item1, item2) = tuple;
        return (item1, item2, item3);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 3 值元组的末尾得到的新的 4 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 3 值元组。</param>
    /// <param name="item4">要附加到 3 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item4"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 4 值元组。</returns>
    public static (T1, T2, T3, T4) Append<T1, T2, T3, T4>(
        this (T1, T2, T3) tuple, T4 item4)
    {
        var (item1, item2, item3) = tuple;
        return (item1, item2, item3, item4);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 4 值元组的末尾得到的新的 5 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">值元组的第 4 个元素的类型。</typeparam>
    /// <typeparam name="T5">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 4 值元组。</param>
    /// <param name="item5">要附加到 4 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item5"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 5 值元组。</returns>
    public static (T1, T2, T3, T4, T5) Append<T1, T2, T3, T4, T5>(
        this (T1, T2, T3, T4) tuple, T5 item5)
    {
        var (item1, item2, item3, item4) = tuple;
        return (item1, item2, item3, item4, item5);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 5 值元组的末尾得到的新的 6 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">值元组的第 4 个元素的类型。</typeparam>
    /// <typeparam name="T5">值元组的第 5 个元素的类型。</typeparam>
    /// <typeparam name="T6">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 5 值元组。</param>
    /// <param name="item6">要附加到 5 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item6"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 6 值元组。</returns>
    public static (T1, T2, T3, T4, T5, T6) Append<T1, T2, T3, T4, T5, T6>(
        this (T1, T2, T3, T4, T5) tuple, T6 item6)
    {
        var (item1, item2, item3, item4, item5) = tuple;
        return (item1, item2, item3, item4, item5, item6);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 6 值元组的末尾得到的新的 7 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">值元组的第 4 个元素的类型。</typeparam>
    /// <typeparam name="T5">值元组的第 5 个元素的类型。</typeparam>
    /// <typeparam name="T6">值元组的第 6 个元素的类型。</typeparam>
    /// <typeparam name="T7">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 6 值元组。</param>
    /// <param name="item7">要附加到 6 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item7"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 7 值元组。</returns>
    public static (T1, T2, T3, T4, T5, T6, T7) Append<T1, T2, T3, T4, T5, T6, T7>(
        this (T1, T2, T3, T4, T5, T6) tuple, T7 item7)
    {
        var (item1, item2, item3, item4, item5, item6) = tuple;
        return (item1, item2, item3, item4, item5, item6, item7);
    }

    /// <summary>
    /// 返回将指定元素附加到当前 7 值元组的末尾得到的新的 8 值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">值元组的第 4 个元素的类型。</typeparam>
    /// <typeparam name="T5">值元组的第 5 个元素的类型。</typeparam>
    /// <typeparam name="T6">值元组的第 6 个元素的类型。</typeparam>
    /// <typeparam name="T7">值元组的第 7 个元素的类型。</typeparam>
    /// <typeparam name="T8">要附加到末尾的值的类型。</typeparam>
    /// <param name="tuple">要在末尾附加值的 7 值元组。</param>
    /// <param name="item8">要附加到 7 值元组末尾的值。</param>
    /// <returns>将 <paramref name="item8"/> 附加到
    /// <paramref name="tuple"/> 末尾得到的新的 8 值元组。</returns>
    public static (T1, T2, T3, T4, T5, T6, T7, T8) Append<T1, T2, T3, T4, T5, T6, T7, T8>(
        this (T1, T2, T3, T4, T5, T6, T7) tuple, T8 item8)
    {
        var (item1, item2, item3, item4, item5, item6, item7) = tuple;
        return (item1, item2, item3, item4, item5, item6, item7, item8);
    }

    /// <summary>
    /// 返回将当前 7 值元组与另一值元组连接得到的新的值元组。
    /// </summary>
    /// <typeparam name="T1">值元组的第 1 个元素的类型。</typeparam>
    /// <typeparam name="T2">值元组的第 2 个元素的类型。</typeparam>
    /// <typeparam name="T3">值元组的第 3 个元素的类型。</typeparam>
    /// <typeparam name="T4">值元组的第 4 个元素的类型。</typeparam>
    /// <typeparam name="T5">值元组的第 5 个元素的类型。</typeparam>
    /// <typeparam name="T6">值元组的第 6 个元素的类型。</typeparam>
    /// <typeparam name="T7">值元组的第 7 个元素的类型。</typeparam>
    /// <typeparam name="TRest">要连接的另一值元组的类型。</typeparam>
    /// <param name="tuple">要与另一值元组连接的 7 值元组。</param>
    /// <param name="rest">要连接到当前 7 值元组的另一值元组。</param>
    /// <returns>将 <paramref name="tuple"/> 与
    /// <paramref name="rest"/> 连接得到的新的值元组。</returns>
    public static ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest> Concat<T1, T2, T3, T4, T5, T6, T7, TRest>(
        this (T1, T2, T3, T4, T5, T6, T7) tuple, TRest rest) where TRest : struct
    {
        var (item1, item2, item3, item4, item5, item6, item7) = tuple;
        return new ValueTuple<T1, T2, T3, T4, T5, T6, T7, TRest>(
            item1, item2, item3, item4, item5, item6, item7, rest);
    }
}
