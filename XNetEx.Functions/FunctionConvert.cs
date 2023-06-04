using System;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Functions;

public static class FunctionConvert
{
    public static Action<object?, EventArgs> ToAction(this EventHandler handler) =>
        handler.CloneAndChangeType<Action<object?, EventArgs>>();

    public static Action<object?, TArgs> ToAction<TArgs>(this EventHandler<TArgs> handler) =>
        handler.CloneAndChangeType<Action<object?, TArgs>>();

    public static Func<T, T, int> ToFunc<T>(this Comparison<T> comparison) =>
        comparison.CloneAndChangeType<Func<T, T, int>>();

    public static Func<TIn, TOut> ToFunc<TIn, TOut>(this Converter<TIn, TOut> converter) =>
        converter.CloneAndChangeType<Func<TIn, TOut>>();

    public static Func<T, bool> ToFunc<T>(this Predicate<T> predicate) =>
        predicate.CloneAndChangeType<Func<T, bool>>();

    private static TDelegate CloneAndChangeType<TDelegate>(this Delegate instance)
        where TDelegate : Delegate
    {
        var copy = RuntimeObject.MemberwiseClone(instance);
        RuntimeObject.TypeHandleOf(copy) = typeof(TDelegate).TypeHandle.Value;
        return (TDelegate)copy;
    }
}
