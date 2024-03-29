﻿using System;

namespace XNetEx.Diagnostics;

/// <summary>
/// 提供将指定基元类型的值表示为其 <see cref="object.ToString()"/> 方法返回的字符串的方法。
/// </summary>
/// <typeparam name="T">要表示为字符串的基元类型。</typeparam>
internal sealed class PrimitiveRepresenter<T> : Representer<T>
{
    /// <summary>
    /// 初始化 <see cref="PrimitiveRepresenter{T}"/> 类的新实例。
    /// </summary>
    /// <exception cref="InvalidOperationException">当前类型不为基元类型
    /// （<see cref="Type.IsPrimitive"/> 返回 <see langword="false"/>）。</exception>
    public PrimitiveRepresenter()
    {
        if (!typeof(T).IsPrimitive)
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// 将指定对象表示为其 <see cref="object.ToString"/> 方法返回的字符串。
    /// </summary>
    /// <param name="value">要表示为字符串的对象。</param>
    /// <returns>调用 <paramref name="value"/> 的
    /// <see cref="object.ToString"/> 方法返回的字符串。</returns>
    public override string Represent(T? value) => value!.ToString()!;
}
