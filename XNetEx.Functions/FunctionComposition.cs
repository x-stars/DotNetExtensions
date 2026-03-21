using System;

namespace XNetEx.Functions;

public static class FunctionComposition
{
    public static Action<T1>
        Compose<T1, TM>(
            this Action<TM> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1) =>
            outerAction.Invoke(innerFunc.Invoke(arg1));
    }

    public static Func<T1, TR>
        Compose<T1, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1));
    }

    public static Action<T1>
        ComposeNext<T1, TM>(
            this Func<T1, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1) =>
            outerAction.Invoke(innerFunc.Invoke(arg1));
    }

    public static Func<T1, TR>
        ComposeNext<T1, TM, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1));
    }

    public static Action<T1, T2>
        Compose<T1, TM, T2>(
            this Action<TM, T2> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2);
    }

    public static Func<T1, T2, TR>
        Compose<T1, TM, T2, TR>(
            this Func<TM, T2, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2);
    }

    public static Action<T1, T2>
        ComposeNext<T1, TM, T2>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2> outerAction)
    {
        return (arg1, arg2) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2);
    }

    public static Func<T1, T2, TR>
        ComposeNext<T1, TM, T2, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, TR> outerFunc)
    {
        return (arg1, arg2) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2);
    }

    public static Action<T1, T2>
        Compose<T1, T2, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2));
    }

    public static Func<T1, T2, TR>
        Compose<T1, T2, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2));
    }

    public static Action<T1, T2>
        ComposeNext<T1, T2, TM>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2));
    }

    public static Func<T1, T2, TR>
        ComposeNext<T1, T2, TM, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2));
    }

    public static Action<T1, T2, T3>
        Compose<T1, TM, T2, T3>(
            this Action<TM, T2, T3> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3);
    }

    public static Func<T1, T2, T3, TR>
        Compose<T1, TM, T2, T3, TR>(
            this Func<TM, T2, T3, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3);
    }

    public static Action<T1, T2, T3>
        ComposeNext<T1, TM, T2, T3>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3> outerAction)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3);
    }

    public static Func<T1, T2, T3, TR>
        ComposeNext<T1, TM, T2, T3, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, TR> outerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3);
    }

    public static Action<T1, T2, T3>
        Compose<T1, T2, TM, T3>(
            this Action<TM, T3> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3);
    }

    public static Func<T1, T2, T3, TR>
        Compose<T1, T2, TM, T3, TR>(
            this Func<TM, T3, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3);
    }

    public static Action<T1, T2, T3>
        ComposeNext<T1, T2, TM, T3>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3> outerAction)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3);
    }

    public static Func<T1, T2, T3, TR>
        ComposeNext<T1, T2, TM, T3, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, TR> outerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3);
    }

    public static Action<T1, T2, T3>
        Compose<T1, T2, T3, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3));
    }

    public static Func<T1, T2, T3, TR>
        Compose<T1, T2, T3, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3));
    }

    public static Action<T1, T2, T3>
        ComposeNext<T1, T2, T3, TM>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3));
    }

    public static Func<T1, T2, T3, TR>
        ComposeNext<T1, T2, T3, TM, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3));
    }

    public static Action<T1, T2, T3, T4>
        Compose<T1, TM, T2, T3, T4>(
            this Action<TM, T2, T3, T4> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        Compose<T1, TM, T2, T3, T4, TR>(
            this Func<TM, T2, T3, T4, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4);
    }

    public static Action<T1, T2, T3, T4>
        ComposeNext<T1, TM, T2, T3, T4>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4> outerAction)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        ComposeNext<T1, TM, T2, T3, T4, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4);
    }

    public static Action<T1, T2, T3, T4>
        Compose<T1, T2, TM, T3, T4>(
            this Action<TM, T3, T4> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        Compose<T1, T2, TM, T3, T4, TR>(
            this Func<TM, T3, T4, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4);
    }

    public static Action<T1, T2, T3, T4>
        ComposeNext<T1, T2, TM, T3, T4>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4> outerAction)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        ComposeNext<T1, T2, TM, T3, T4, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4);
    }

    public static Action<T1, T2, T3, T4>
        Compose<T1, T2, T3, TM, T4>(
            this Action<TM, T4> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        Compose<T1, T2, T3, TM, T4, TR>(
            this Func<TM, T4, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4);
    }

    public static Action<T1, T2, T3, T4>
        ComposeNext<T1, T2, T3, TM, T4>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4> outerAction)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4);
    }

    public static Func<T1, T2, T3, T4, TR>
        ComposeNext<T1, T2, T3, TM, T4, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4);
    }

    public static Action<T1, T2, T3, T4>
        Compose<T1, T2, T3, T4, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4));
    }

    public static Func<T1, T2, T3, T4, TR>
        Compose<T1, T2, T3, T4, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4));
    }

    public static Action<T1, T2, T3, T4>
        ComposeNext<T1, T2, T3, T4, TM>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4));
    }

    public static Func<T1, T2, T3, T4, TR>
        ComposeNext<T1, T2, T3, T4, TM, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4));
    }

    public static Action<T1, T2, T3, T4, T5>
        Compose<T1, TM, T2, T3, T4, T5>(
            this Action<TM, T2, T3, T4, T5> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        Compose<T1, TM, T2, T3, T4, T5, TR>(
            this Func<TM, T2, T3, T4, T5, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        ComposeNext<T1, TM, T2, T3, T4, T5>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        Compose<T1, T2, TM, T3, T4, T5>(
            this Action<TM, T3, T4, T5> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        Compose<T1, T2, TM, T3, T4, T5, TR>(
            this Func<TM, T3, T4, T5, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        ComposeNext<T1, T2, TM, T3, T4, T5>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        Compose<T1, T2, T3, TM, T4, T5>(
            this Action<TM, T4, T5> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        Compose<T1, T2, T3, TM, T4, T5, TR>(
            this Func<TM, T4, T5, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        ComposeNext<T1, T2, T3, TM, T4, T5>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        Compose<T1, T2, T3, T4, TM, T5>(
            this Action<TM, T5> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        Compose<T1, T2, T3, T4, TM, T5, TR>(
            this Func<TM, T5, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        ComposeNext<T1, T2, T3, T4, TM, T5>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5);
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5);
    }

    public static Action<T1, T2, T3, T4, T5>
        Compose<T1, T2, T3, T4, T5, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5));
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        Compose<T1, T2, T3, T4, T5, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5));
    }

    public static Action<T1, T2, T3, T4, T5>
        ComposeNext<T1, T2, T3, T4, T5, TM>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5));
    }

    public static Func<T1, T2, T3, T4, T5, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5));
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, TM, T2, T3, T4, T5, T6>(
            this Action<TM, T2, T3, T4, T5, T6> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, TR>(
            this Func<TM, T2, T3, T4, T5, T6, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, T2, TM, T3, T4, T5, T6>(
            this Action<TM, T3, T4, T5, T6> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, TR>(
            this Func<TM, T3, T4, T5, T6, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, T2, T3, TM, T4, T5, T6>(
            this Action<TM, T4, T5, T6> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, TR>(
            this Func<TM, T4, T5, T6, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, T2, T3, T4, TM, T5, T6>(
            this Action<TM, T5, T6> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, TR>(
            this Func<TM, T5, T6, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, T2, T3, T4, T5, TM, T6>(
            this Action<TM, T6> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, TR>(
            this Func<TM, T6, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6);
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6);
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        Compose<T1, T2, T3, T4, T5, T6, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    public static Action<T1, T2, T3, T4, T5, T6>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    public static Func<T1, T2, T3, T4, T5, T6, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7>(
            this Action<TM, T2, T3, T4, T5, T6, T7> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7>(
            this Action<TM, T3, T4, T5, T6, T7> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, TR>(
            this Func<TM, T3, T4, T5, T6, T7, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7>(
            this Action<TM, T4, T5, T6, T7> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, TR>(
            this Func<TM, T4, T5, T6, T7, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7>(
            this Action<TM, T5, T6, T7> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, TR>(
            this Func<TM, T5, T6, T7, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7>(
            this Action<TM, T6, T7> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, TR>(
            this Func<TM, T6, T7, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7>(
            this Action<TM, T7> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, TR>(
            this Func<TM, T7, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8>(
            this Action<TM, T3, T4, T5, T6, T7, T8> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8>(
            this Action<TM, T4, T5, T6, T7, T8> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, TR>(
            this Func<TM, T4, T5, T6, T7, T8, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8>(
            this Action<TM, T5, T6, T7, T8> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, TR>(
            this Func<TM, T5, T6, T7, T8, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8>(
            this Action<TM, T6, T7, T8> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, TR>(
            this Func<TM, T6, T7, T8, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8>(
            this Action<TM, T7, T8> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, TR>(
            this Func<TM, T7, T8, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8>(
            this Action<TM, T8> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, TR>(
            this Func<TM, T8, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9>(
            this Action<TM, T4, T5, T6, T7, T8, T9> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9>(
            this Action<TM, T5, T6, T7, T8, T9> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, TR>(
            this Func<TM, T5, T6, T7, T8, T9, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9>(
            this Action<TM, T6, T7, T8, T9> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, TR>(
            this Func<TM, T6, T7, T8, T9, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9>(
            this Action<TM, T7, T8, T9> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, TR>(
            this Func<TM, T7, T8, T9, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9>(
            this Action<TM, T8, T9> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, TR>(
            this Func<TM, T8, T9, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9>(
            this Action<TM, T9> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, TR>(
            this Func<TM, T9, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10>(
            this Action<TM, T5, T6, T7, T8, T9, T10> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10>(
            this Action<TM, T6, T7, T8, T9, T10> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, TR>(
            this Func<TM, T6, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10>(
            this Action<TM, T7, T8, T9, T10> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, TR>(
            this Func<TM, T7, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10>(
            this Action<TM, T8, T9, T10> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, TR>(
            this Func<TM, T8, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10>(
            this Action<TM, T9, T10> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, TR>(
            this Func<TM, T9, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10>(
            this Action<TM, T10> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, TR>(
            this Func<TM, T10, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11>(
            this Action<TM, T6, T7, T8, T9, T10, T11> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11>(
            this Action<TM, T7, T8, T9, T10, T11> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, TR>(
            this Func<TM, T7, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11>(
            this Action<TM, T8, T9, T10, T11> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, TR>(
            this Func<TM, T8, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11>(
            this Action<TM, T9, T10, T11> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, TR>(
            this Func<TM, T9, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11>(
            this Action<TM, T10, T11> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, TR>(
            this Func<TM, T10, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11>(
            this Action<TM, T11> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, TR>(
            this Func<TM, T11, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T6, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12>(
            this Action<TM, T7, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T7, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12>(
            this Action<TM, T8, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, TR>(
            this Func<TM, T8, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12>(
            this Action<TM, T9, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, TR>(
            this Func<TM, T9, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12>(
            this Action<TM, T10, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, TR>(
            this Func<TM, T10, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12>(
            this Action<TM, T11, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, TR>(
            this Func<TM, T11, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12>(
            this Action<TM, T12> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, TR>(
            this Func<TM, T12, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM, T12> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, T12, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T6, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T7, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13>(
            this Action<TM, T8, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T8, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13>(
            this Action<TM, T9, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, TR>(
            this Func<TM, T9, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13>(
            this Action<TM, T10, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, TR>(
            this Func<TM, T10, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13>(
            this Action<TM, T11, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, TR>(
            this Func<TM, T11, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13>(
            this Action<TM, T12, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, TR>(
            this Func<TM, T12, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM, T12, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, T12, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13>(
            this Action<TM, T13> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, TR>(
            this Func<TM, T13, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Action<TM, T13> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Func<TM, T13, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T7, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T8, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14>(
            this Action<TM, T9, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T9, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14>(
            this Action<TM, T10, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, TR>(
            this Func<TM, T10, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14>(
            this Action<TM, T11, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, TR>(
            this Func<TM, T11, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14>(
            this Action<TM, T12, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, TR>(
            this Func<TM, T12, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM, T12, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, T12, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14>(
            this Action<TM, T13, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, TR>(
            this Func<TM, T13, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Action<TM, T13, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Func<TM, T13, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14>(
            this Action<TM, T14> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, TR>(
            this Func<TM, T14, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Action<TM, T14> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Func<TM, T14, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T8, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T9, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15>(
            this Action<TM, T10, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T10, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15>(
            this Action<TM, T11, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, TR>(
            this Func<TM, T11, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15>(
            this Action<TM, T12, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, TR>(
            this Func<TM, T12, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM, T12, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, T12, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15>(
            this Action<TM, T13, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, TR>(
            this Func<TM, T13, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Action<TM, T13, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Func<TM, T13, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15>(
            this Action<TM, T14, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, TR>(
            this Func<TM, T14, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Action<TM, T14, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Func<TM, T14, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15>(
            this Action<TM, T15> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, TR>(
            this Func<TM, T15, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Action<TM, T15> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Func<TM, T15, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, TM> innerFunc,
            Action<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, TM> innerFunc,
            Func<TM, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1), arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, TM> innerFunc,
            Action<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, TM> innerFunc,
            Func<TM, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2), arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, TM> innerFunc,
            Action<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, TM> innerFunc,
            Func<TM, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3), arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Action<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, TM> innerFunc,
            Func<TM, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4), arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Action<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, TM> innerFunc,
            Func<TM, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5), arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Action<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, TM> innerFunc,
            Func<TM, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6), arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Action<TM, T8, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, TM, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, TM> innerFunc,
            Func<TM, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7), arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T9, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Action<TM, T9, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, TM, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, TM> innerFunc,
            Func<TM, T9, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8), arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T10, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Action<TM, T10, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM, T10, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TM> innerFunc,
            Func<TM, T10, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9), arg10, arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, T16>(
            this Action<TM, T11, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T11, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Action<TM, T11, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM, T11, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TM> innerFunc,
            Func<TM, T11, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10), arg11, arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, T16>(
            this Action<TM, T12, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, T16, TR>(
            this Func<TM, T12, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Action<TM, T12, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM, T12, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TM> innerFunc,
            Func<TM, T12, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11), arg12, arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, T16>(
            this Action<TM, T13, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, T16, TR>(
            this Func<TM, T13, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Action<TM, T13, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM, T13, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TM> innerFunc,
            Func<TM, T13, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12), arg13, arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, T16>(
            this Action<TM, T14, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, T16, TR>(
            this Func<TM, T14, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Action<TM, T14, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM, T14, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TM> innerFunc,
            Func<TM, T14, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13), arg14, arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, T16>(
            this Action<TM, T15, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, T16, TR>(
            this Func<TM, T15, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Action<TM, T15, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15, arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM, T15, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TM> innerFunc,
            Func<TM, T15, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14), arg15, arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, T16>(
            this Action<TM, T16> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, T16, TR>(
            this Func<TM, T16, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, T16>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc,
            Action<TM, T16> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), arg16);
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM, T16, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TM> innerFunc,
            Func<TM, T16, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15), arg16);
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM>(
            this Action<TM> outerAction,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Compose<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM, TR>(
            this Func<TM, TR> outerFunc,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM> innerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }

    public static Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM> innerFunc,
            Action<TM> outerAction)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerAction.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        ComposeNext<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM, TR>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TM> innerFunc,
            Func<TM, TR> outerFunc)
    {
        return (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16) =>
            outerFunc.Invoke(innerFunc.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16));
    }
}
