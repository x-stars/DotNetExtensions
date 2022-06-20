using System;
using System.Collections.Generic;

namespace XNetEx.CommandLine.Specialized;

/// <summary>
/// PowerShell 风格的命令行参数解析器，参数名称忽略大小写。
/// </summary>
/// <remarks>
/// 不支持无名参数的解析，但支持省略参数名称的有名参数的解析。
/// 不支持 Unix / Linux shell 中连字符 "-" 后接多个选项参数的解析。
/// 不支持一个参数名称后跟多个参数值的有名参数的解析。
/// 不支持多个同名的有名参数的解析。
/// </remarks>
[Serializable]
public class PowerShellArgumentReader : ArgumentReader
{
    /// <summary>
    /// 初始化 PowerShell 风格命令行参数解析器
    /// <see cref="PowerShellArgumentReader"/> 的新实例。
    /// </summary>
    /// <remarks><para>
    /// 输入的参数名称列表用于解析省略参数名称的有名参数；若无需解析省略参数名称的有名参数，可留空。
    /// </para><para>
    /// 参数名称应带有作为提示符的连字符 "-"。
    /// 有名参数名称的顺序会影响省略参数名称的有名参数的解析，因此必选参数应在可选参数之前。
    /// 选项参数名称的顺序则不会产生影响，可任意指定顺序。
    /// </para></remarks>
    /// <param name="arguments">待解析的参数列表。</param>
    /// <param name="argumentNames">所有有名参数名称列表，必选参数应在可选参数之前。</param>
    /// <param name="optionNames">所有选项参数名称列表。</param>
    public PowerShellArgumentReader(string[] arguments,
        IEnumerable<string>? argumentNames = null,
        IEnumerable<string>? optionNames = null)
        : base(arguments, true, argumentNames, optionNames) { }

    /// <summary>
    /// 解析指定名称的有名参数。
    /// </summary>
    /// <param name="name">要解析的有名参数的名称。</param>
    /// <returns>名称为 <paramref name="name"/> 的有名参数的值；
    /// 若不存在则为按顺序寻找的无名参数的值；
    /// 若仍不存在则为 <see langword="null"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
    public override string? GetArgument(string name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (base.GetArgument(name) is string value)
        {
            return value;
        }
        else
        {
            int positionNow = 0;
            foreach (string nameNow in this.ArgumentNames)
            {
                if (this.NameComparer.Equals(nameNow, name))
                {
                    if (base.GetArgument(nameNow) is string valueByName)
                    {
                        return valueByName;
                    }
                    else if (base.GetArgument(positionNow) is string valueByPosition)
                    {
                        return valueByPosition;
                    }
                }
                else
                {
                    if (base.GetArgument(nameNow) is string valueByName)
                    {
                    }
                    else if (base.GetArgument(positionNow) is string valueByPosition)
                    {
                        positionNow++;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 解析指定位置的无名参数。
    /// 不支持此方法，总是引发 <see cref="NotSupportedException"/> 异常。
    /// </summary>
    /// <param name="position">要解析的无名参数在所有无名参数中的位置。</param>
    /// <returns>总是引发 <see cref="NotSupportedException"/> 异常。</returns>
    /// <exception cref="NotSupportedException">不支持此方法。</exception>
    public override string? GetArgument(int position)
    {
        throw new NotSupportedException();
    }
}
