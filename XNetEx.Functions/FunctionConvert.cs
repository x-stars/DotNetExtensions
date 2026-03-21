using System;

namespace XNetEx.Functions;

public static class FunctionConvert
{
    public static Action<object?, EventArgs> ToAction(this EventHandler handler) => handler.Invoke;

    public static Action<object?, TArgs> ToAction<TArgs>(this EventHandler<TArgs> handler) => handler.Invoke;

    public static Func<T, T, int> ToFunc<T>(this Comparison<T> comparison) => comparison.Invoke;

    public static Func<TIn, TOut> ToFunc<TIn, TOut>(this Converter<TIn, TOut> converter) => converter.Invoke;

    public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate) => predicate.Invoke;
}
