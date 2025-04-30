using System.Runtime.CompilerServices;

namespace XNetEx.Runtime.CompilerServices;

/// <summary>
/// Provides mutable runtime-boxed objects for value types.
/// </summary>
public static class ValueTypeBox
{
    /// <summary>
    /// Creates a new runtime-boxed object represented as <see cref="StrongBox{T}"/>
    /// for the specified value type, which the value can be
    /// modified by <see cref="StrongBox{T}.Value"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to be boxed.</typeparam>
    /// <param name="value">The value to be boxed.</param>
    /// <returns>A new runtime-boxed object of <paramref name="value"/>
    /// represented as <see cref="StrongBox{T}"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe StrongBox<T> Create<T>(T value = default)
        where T : struct
    {
#if NETCOREAPP3_0_OR_GREATER
        return Unsafe.As<StrongBox<T>>((object)value);
#else
        static object Identity(object value) => value;
        return ((delegate*<object, StrongBox<T>>)
                (delegate*<object, object>)&Identity)((object)value);
#endif
    }
}
