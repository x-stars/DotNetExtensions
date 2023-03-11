using System;

namespace XNetEx.Functions;

public static class PartialApplication
{
    public static Action<T2>
        Invoke<T1, T2>(
            this Action<T1, T2> action,
            T1 arg1)
    {
        return (arg2) =>
            action.Invoke(arg1, arg2);
    }

    public static Func<T2, TR>
        Invoke<T1, T2, TR>(
            this Func<T1, T2, TR> func,
            T1 arg1)
    {
        return (arg2) =>
            func.Invoke(arg1, arg2);
    }

    public static Action<T2, T3>
        Invoke<T1, T2, T3>(
            this Action<T1, T2, T3> action,
            T1 arg1)
    {
        return (arg2, arg3) =>
            action.Invoke(arg1, arg2, arg3);
    }

    public static Func<T2, T3, TR>
        Invoke<T1, T2, T3, TR>(
            this Func<T1, T2, T3, TR> func,
            T1 arg1)
    {
        return (arg2, arg3) =>
            func.Invoke(arg1, arg2, arg3);
    }

    public static Action<T3>
        Invoke<T1, T2, T3>(
            this Action<T1, T2, T3> action,
            T1 arg1, T2 arg2)
    {
        return (arg3) =>
            action.Invoke(arg1, arg2, arg3);
    }

    public static Func<T3, TR>
        Invoke<T1, T2, T3, TR>(
            this Func<T1, T2, T3, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3) =>
            func.Invoke(arg1, arg2, arg3);
    }

    public static Action<T2, T3, T4>
        Invoke<T1, T2, T3, T4>(
            this Action<T1, T2, T3, T4> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4) =>
            action.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Func<T2, T3, T4, TR>
        Invoke<T1, T2, T3, T4, TR>(
            this Func<T1, T2, T3, T4, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4) =>
            func.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Action<T3, T4>
        Invoke<T1, T2, T3, T4>(
            this Action<T1, T2, T3, T4> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4) =>
            action.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Func<T3, T4, TR>
        Invoke<T1, T2, T3, T4, TR>(
            this Func<T1, T2, T3, T4, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4) =>
            func.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Action<T4>
        Invoke<T1, T2, T3, T4>(
            this Action<T1, T2, T3, T4> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4) =>
            action.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Func<T4, TR>
        Invoke<T1, T2, T3, T4, TR>(
            this Func<T1, T2, T3, T4, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4) =>
            func.Invoke(arg1, arg2, arg3, arg4);
    }

    public static Action<T2, T3, T4, T5>
        Invoke<T1, T2, T3, T4, T5>(
            this Action<T1, T2, T3, T4, T5> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Func<T2, T3, T4, T5, TR>
        Invoke<T1, T2, T3, T4, T5, TR>(
            this Func<T1, T2, T3, T4, T5, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Action<T3, T4, T5>
        Invoke<T1, T2, T3, T4, T5>(
            this Action<T1, T2, T3, T4, T5> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Func<T3, T4, T5, TR>
        Invoke<T1, T2, T3, T4, T5, TR>(
            this Func<T1, T2, T3, T4, T5, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Action<T4, T5>
        Invoke<T1, T2, T3, T4, T5>(
            this Action<T1, T2, T3, T4, T5> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Func<T4, T5, TR>
        Invoke<T1, T2, T3, T4, T5, TR>(
            this Func<T1, T2, T3, T4, T5, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Action<T5>
        Invoke<T1, T2, T3, T4, T5>(
            this Action<T1, T2, T3, T4, T5> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Func<T5, TR>
        Invoke<T1, T2, T3, T4, T5, TR>(
            this Func<T1, T2, T3, T4, T5, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5);
    }

    public static Action<T2, T3, T4, T5, T6>
        Invoke<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T2, T3, T4, T5, T6, TR>
        Invoke<T1, T2, T3, T4, T5, T6, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T3, T4, T5, T6>
        Invoke<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T3, T4, T5, T6, TR>
        Invoke<T1, T2, T3, T4, T5, T6, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T4, T5, T6>
        Invoke<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T4, T5, T6, TR>
        Invoke<T1, T2, T3, T4, T5, T6, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T5, T6>
        Invoke<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T5, T6, TR>
        Invoke<T1, T2, T3, T4, T5, T6, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T6>
        Invoke<T1, T2, T3, T4, T5, T6>(
            this Action<T1, T2, T3, T4, T5, T6> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T6, TR>
        Invoke<T1, T2, T3, T4, T5, T6, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T2, T3, T4, T5, T6, T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T2, T3, T4, T5, T6, T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T3, T4, T5, T6, T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T3, T4, T5, T6, T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T4, T5, T6, T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T4, T5, T6, T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T5, T6, T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T5, T6, T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T6, T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T6, T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T7>
        Invoke<T1, T2, T3, T4, T5, T6, T7>(
            this Action<T1, T2, T3, T4, T5, T6, T7> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T7, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T3, T4, T5, T6, T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T3, T4, T5, T6, T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T4, T5, T6, T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T4, T5, T6, T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T5, T6, T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T5, T6, T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T6, T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T6, T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T7, T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T7, T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T8>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T8, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T4, T5, T6, T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T4, T5, T6, T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T5, T6, T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T5, T6, T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T6, T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T6, T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T7, T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T7, T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T8, T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T8, T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T9>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T9, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T5, T6, T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T5, T6, T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T6, T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T6, T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T7, T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T7, T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T8, T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T8, T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T9, T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T9, T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T10>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T10, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T6, T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T6, T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T7, T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T7, T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T8, T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T8, T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T9, T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T9, T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T10, T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T10, T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T11>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T11, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T7, T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T7, T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T8, T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T8, T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T9, T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T9, T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T10, T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T10, T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T11, T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T11, T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T12>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T12, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T8, T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T8, T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T9, T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T9, T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T10, T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T10, T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T11, T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T11, T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T12, T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T12, T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T13>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T13, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T9, T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T9, T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T10, T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T10, T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T11, T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T11, T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T12, T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T12, T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T13, T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T13, T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T14>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T14, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T9, T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T9, T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T10, T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T10, T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T11, T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T11, T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T12, T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T12, T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T13, T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T13, T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T14, T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14, arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T14, T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14, arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T15>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return (arg15) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T15, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return (arg15) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1)
    {
        return (arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2)
    {
        return (arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3)
    {
        return (arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        return (arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
    {
        return (arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
    {
        return (arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
    {
        return (arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T9, T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
    {
        return (arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T10, T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T10, T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
    {
        return (arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T11, T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T11, T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
    {
        return (arg11, arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T12, T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T12, T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
    {
        return (arg12, arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T13, T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T13, T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
    {
        return (arg13, arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T14, T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14, arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T14, T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
    {
        return (arg14, arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T15, T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return (arg15, arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T15, T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
    {
        return (arg15, arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T16>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return (arg16) =>
            action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T16, TR>
        Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
    {
        return (arg16) =>
            func.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }
}
