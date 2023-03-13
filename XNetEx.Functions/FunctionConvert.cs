using System;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Functions;

public static class FunctionConvert
{
    public static Action<object?, EventArgs> ToAction(EventHandler handler) =>
        FunctionConvert.CloneAndChangeType<Action<object?, EventArgs>>(handler);

    public static Action<object?, TArgs> ToAction<TArgs>(EventHandler<TArgs> handler) =>
        FunctionConvert.CloneAndChangeType<Action<object?, TArgs>>(handler);

    public static Func<T, T, int> ToFunc<T>(Comparison<T> comparison) =>
        FunctionConvert.CloneAndChangeType<Func<T, T, int>>(comparison);

    public static Func<TIn, TOut> ToFunc<TIn, TOut>(Converter<TIn, TOut> converter) =>
        FunctionConvert.CloneAndChangeType<Func<TIn, TOut>>(converter);

    public static Func<T, bool> ToFunc<T>(Predicate<T> predicate) =>
        FunctionConvert.CloneAndChangeType<Func<T, bool>>(predicate);

    private static TDelegate CloneAndChangeType<TDelegate>(Delegate instance)
        where TDelegate : Delegate
    {
        var copy = RuntimeObject.MemberwiseClone(instance);
        RuntimeObject.TypeHandleOf(copy) = typeof(TDelegate).TypeHandle.Value;
        return (TDelegate)copy;
    }
}
