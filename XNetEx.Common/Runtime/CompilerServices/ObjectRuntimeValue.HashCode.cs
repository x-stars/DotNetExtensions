﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using XCmpSp = XNetEx.Collections.Specialized;

namespace XNetEx.Runtime.CompilerServices;

using ReferenceEqualityComparer = XCmpSp::ReferenceEqualityComparer;

static partial class ObjectRuntimeValue
{
    /// <summary>
    /// 获取指定对象递归包含的值的哈希代码。
    /// </summary>
    /// <remarks>基于反射调用，可能存在性能问题。</remarks>
    /// <param name="value">要获取递归包含的值的哈希代码的对象。</param>
    /// <returns><paramref name="value"/> 递归包含的值的哈希代码。</returns>
    /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
    public static int GetRecursiveHashCode(this object? value)
    {
        var comparer = ReferenceEqualityComparer.Default;
        var computed = new HashSet<object>(comparer);
        return ObjectRuntimeValue.GetRecursiveHashCode(value, computed);
    }

    /// <summary>
    /// 组合当前哈希代码和新的哈希代码。
    /// </summary>
    /// <param name="hashCode">当前哈希代码。</param>
    /// <param name="nextHashCode">新的哈希代码。</param>
    /// <returns><paramref name="hashCode"/> 与
    /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int CombineHashCode(int hashCode, int nextHashCode)
    {
        return hashCode * -1521134295 + nextHashCode;
    }

    /// <summary>
    /// 获取指定对象递归包含的值的哈希代码。
    /// </summary>
    /// <param name="value">要获取基于值的哈希代码的对象。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="value"/> 递归包含的值的哈希代码。</returns>
    /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
    private static int GetRecursiveHashCode(object? value, HashSet<object> computed)
    {
        if (value is null) { return 0; }

        if (!computed.Add(value)) { return 0; }

        var type = value.GetType();
        if (type.IsPrimitive)
        {
            return ObjectRuntimeValue.GetPrimitiveHashCode(value);
        }
        if (type == typeof(string))
        {
            return ObjectRuntimeValue.GetPrimitiveHashCode(value);
        }
        else if (type.IsArray)
        {
            return ObjectRuntimeValue.GetArrayRecursiveHashCode((Array)value, computed);
        }
        else
        {
            return ObjectRuntimeValue.GetObjectRecursiveHashCode(value, computed);
        }
    }

    /// <summary>
    /// 获取指定基元类型对象 (<see cref="Type.IsPrimitive"/>) 的哈希代码。
    /// </summary>
    /// <param name="value">要获取哈希代码的基元类型对象。</param>
    /// <returns><paramref name="value"/> 的哈希代码。</returns>
    private static int GetPrimitiveHashCode(object value)
    {
        return value.GetHashCode();
    }

    /// <summary>
    /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
    /// </summary>
    /// <param name="value">要获取哈希代码的包装的指针。</param>
    /// <returns><paramref name="value"/> 中包装的指针的哈希代码。</returns>
    private static unsafe int GetBoxedPointerHashCode(object value)
    {
        return ((nint)Pointer.Unbox(value)).GetHashCode();
    }

    /// <summary>
    /// 获取指定数组中所有元素的递归包含的值的哈希代码。
    /// </summary>
    /// <param name="value">要获取基于值的哈希代码的数组。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="value"/> 中所有元素的递归包含的值的哈希代码。</returns>
    /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
    private static int GetArrayRecursiveHashCode(Array value, HashSet<object> computed)
    {
        var hashCode = value.GetType().GetHashCode();

        var typeArray = value.GetType();
        if (typeArray.GetElementType()!.IsPointer)
        {
            var methodGet = typeArray.GetMethod("Get")!;
            foreach (var indices in value.GetIndicesSequence(reuseIndices: true))
            {
                var item = methodGet.Invoke(value, indices.Box())!;
                hashCode = ObjectRuntimeValue.CombineHashCode(
                    hashCode, ObjectRuntimeValue.GetBoxedPointerHashCode(item));
            }
        }
        else if (value.IsSZArray())
        {
            foreach (var index in ..value.Length)
            {
                var item = value.GetValue(index);
                hashCode = ObjectRuntimeValue.CombineHashCode(
                    hashCode, ObjectRuntimeValue.GetRecursiveHashCode(item, computed));
            }
        }
        else
        {
            foreach (var indices in value.GetIndicesSequence(reuseIndices: true))
            {
                var item = value.GetValue(indices);
                hashCode = ObjectRuntimeValue.CombineHashCode(
                    hashCode, ObjectRuntimeValue.GetRecursiveHashCode(item, computed));
            }
        }

        return hashCode;
    }

    /// <summary>
    /// 获取指定对象中所有字段的递归包含的值的哈希代码。
    /// </summary>
    /// <param name="value">要获取基于值的哈希代码的对象。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="value"/> 中所有字段的递归包含的值的哈希代码。</returns>
    /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
    private static int GetObjectRecursiveHashCode(object value, HashSet<object> computed)
    {
        var hashCode = value.GetType().GetHashCode();

        for (var type = value.GetType(); type is not null; type = type.BaseType)
        {
            var fields = type.GetFields(BindingFlags.DeclaredOnly |
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in fields)
            {
                var member = field.GetValue(value);
                if (field.FieldType.IsPointer)
                {
                    hashCode = ObjectRuntimeValue.CombineHashCode(
                        hashCode, ObjectRuntimeValue.GetBoxedPointerHashCode(member!));
                }
                else
                {
                    hashCode = ObjectRuntimeValue.CombineHashCode(
                        hashCode, ObjectRuntimeValue.GetRecursiveHashCode(member, computed));
                }
            }
        }

        return hashCode;
    }
}
