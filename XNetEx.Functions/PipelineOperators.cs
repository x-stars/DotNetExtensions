using System;

namespace XNetEx.Functions;

public static class PipelineOperators
{
    public static T1
        Perform<T1>(
            this T1 arg1,
            Action<T1> action)
    {
        action.Invoke(arg1);
        return arg1;
    }

    public static void
        Apply<T1>(
            this T1 arg1,
            Action<T1> action)
    {
        action.Invoke(arg1);
    }

    public static TR
        Apply<T1, TR>(
            this T1 arg1,
            Func<T1, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static (T1, T2)
        Perform<T1, T2>(
            this (T1, T2) args,
            Action<T1, T2> action)
    {
        action.Invoke(args.Item1, args.Item2);
        return args;
    }

    public static Action<T2>
        Apply<T1, T2>(
            this T1 arg1,
            Action<T1, T2> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, TR>
        Apply<T1, T2, TR>(
            this T1 arg1,
            Func<T1, T2, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static void
        Apply<T1, T2>(
            this (T1, T2) args,
            Action<T1, T2> action)
    {
        action.Invoke(args.Item1, args.Item2);
    }

    public static TR
        Apply<T1, T2, TR>(
            this (T1, T2) args,
            Func<T1, T2, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static (T1, T2, T3)
        Perform<T1, T2, T3>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3);
        return args;
    }

    public static Action<T2, T3>
        Apply<T1, T2, T3>(
            this T1 arg1,
            Action<T1, T2, T3> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, TR>
        Apply<T1, T2, T3, TR>(
            this T1 arg1,
            Func<T1, T2, T3, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3>
        Apply<T1, T2, T3>(
            this (T1, T2) args,
            Action<T1, T2, T3> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, TR>
        Apply<T1, T2, T3, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static void
        Apply<T1, T2, T3>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static TR
        Apply<T1, T2, T3, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static (T1, T2, T3, T4)
        Perform<T1, T2, T3, T4>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
        return args;
    }

    public static Action<T2, T3, T4>
        Apply<T1, T2, T3, T4>(
            this T1 arg1,
            Action<T1, T2, T3, T4> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, TR>
        Apply<T1, T2, T3, T4, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4>
        Apply<T1, T2, T3, T4>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, TR>
        Apply<T1, T2, T3, T4, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4>
        Apply<T1, T2, T3, T4>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, TR>
        Apply<T1, T2, T3, T4, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static void
        Apply<T1, T2, T3, T4>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static TR
        Apply<T1, T2, T3, T4, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static (T1, T2, T3, T4, T5)
        Perform<T1, T2, T3, T4, T5>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
        return args;
    }

    public static Action<T2, T3, T4, T5>
        Apply<T1, T2, T3, T4, T5>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, TR>
        Apply<T1, T2, T3, T4, T5, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5>
        Apply<T1, T2, T3, T4, T5>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, TR>
        Apply<T1, T2, T3, T4, T5, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5>
        Apply<T1, T2, T3, T4, T5>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, TR>
        Apply<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5>
        Apply<T1, T2, T3, T4, T5>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, TR>
        Apply<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static void
        Apply<T1, T2, T3, T4, T5>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static (T1, T2, T3, T4, T5, T6)
        Perform<T1, T2, T3, T4, T5, T6>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6>
        Apply<T1, T2, T3, T4, T5, T6>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, TR>
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6>
        Apply<T1, T2, T3, T4, T5, T6>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, TR>
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6>
        Apply<T1, T2, T3, T4, T5, T6>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, TR>
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6>
        Apply<T1, T2, T3, T4, T5, T6>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, TR>
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6>
        Apply<T1, T2, T3, T4, T5, T6>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, TR>
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static (T1, T2, T3, T4, T5, T6, T7)
        Perform<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7>
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11, T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Action<T12>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Func<T12, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11, T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Action<T12, T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Func<T12, T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Action<T13>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Func<T13, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11, T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Action<T12, T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Func<T12, T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Action<T13, T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Func<T13, T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Action<T14>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static Func<T14, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11, T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Action<T12, T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Func<T12, T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Action<T13, T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Func<T13, T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Action<T14, T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static Func<T14, T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static Action<T15>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static Func<T15, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15);
    }

    public static (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)
        Perform<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15, args.Item16);
        return args;
    }

    public static Action<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this T1 arg1,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(arg1);
    }

    public static Func<T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this T1 arg1,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(arg1);
    }

    public static Action<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2);
    }

    public static Func<T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2);
    }

    public static Action<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Func<T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3);
    }

    public static Action<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Func<T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4);
    }

    public static Action<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Func<T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5);
    }

    public static Action<T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Func<T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6);
    }

    public static Action<T8, T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Func<T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7);
    }

    public static Action<T9, T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Func<T9, T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8);
    }

    public static Action<T10, T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Func<T10, T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9);
    }

    public static Action<T11, T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Func<T11, T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10);
    }

    public static Action<T12, T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Func<T12, T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11);
    }

    public static Action<T13, T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Func<T13, T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12);
    }

    public static Action<T14, T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static Func<T14, T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13);
    }

    public static Action<T15, T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static Func<T15, T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14);
    }

    public static Action<T16>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        return action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15);
    }

    public static Func<T16, TR>
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15);
    }

    public static void
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) args,
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action)
    {
        action.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15, args.Item16);
    }

    public static TR
        Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR>(
            this (T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16) args,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TR> func)
    {
        return func.Invoke(args.Item1, args.Item2, args.Item3, args.Item4, args.Item5, args.Item6, args.Item7, args.Item8, args.Item9, args.Item10, args.Item11, args.Item12, args.Item13, args.Item14, args.Item15, args.Item16);
    }
}
