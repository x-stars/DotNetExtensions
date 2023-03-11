using System;
using System.Collections.Generic;

namespace XNetEx.Functions;

public static class CoreFunctions
{
    public static T Identity<T>(T value) => value;

    public static bool Not(bool value) => !value;

    public static bool IsOfType<T>(object? instance) => instance is T;

    public static T? CastType<T>(object? instance) => (T?)instance;

    public static T? TryCastType<T>(object? instance) where T : class => instance as T;

    public static T? TryCastNullable<T>(object? instance) where T : struct => instance as T?;

    public static int Compare<T>(T? x, T? y) => Comparer<T>.Default.Compare(x!, y!);

    public static bool Equals<T>(T? x, T? y) => EqualityComparer<T>.Default.Equals(x!, y!);

    public static int GetHashCode<T>(T? obj) => EqualityComparer<T>.Default.GetHashCode(obj!);

    public static Type? GetType<T>(object? instance) => instance?.GetType();

    public static string? ToString<T>(T? value) => value?.ToString();

    public static class NotNull
    {
        public static T CastType<T>(object instance) where T : notnull => (T)instance;

        public static Type GetType<T>(object instance) where T : notnull => instance.GetType();

        public static string? ToString<T>(T value) where T : notnull => value.ToString();
    }
}
