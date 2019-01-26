using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 命令提示符 (CMD) 风格的命令行参数解析器，参数名称忽略大小写，有名参数名称与参数值用冒号 ":" 分隔。
    /// </summary>
    /// <remarks>
    /// 支持多个同名的有名参数的解析。
    /// 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
    /// 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
    /// 不支持一个参数名称后跟多个参数值的有名参数的解析。
    /// </remarks>
    public class CmdParamReader : ParamReader
    {
        /// <summary>
        /// 初始化 CMD 风格命令行参数解析器 <see cref="CmdParamReader"/> 的新实例。
        /// </summary>
        /// <remarks>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可留空。
        /// </remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="paramNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public CmdParamReader(string[] arguments,
            string[] paramNames = null, string[] switchNames = null)
            : base(arguments, true, paramNames, switchNames) { }

        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="paramName">要解析的有名参数的名称。</param>
        /// <returns>
        /// 名称为 <paramref name="paramName"/> 的有名参数的值；
        /// 不存在则返回 <see langword="null"/>。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paramName"/> 为 <see langword="null"/>。
        /// </exception>
        public override string GetParam(string paramName)
        {
            // 参数检查。
            if (paramName is null)
            {
                throw new ArgumentNullException(nameof(paramName));
            }

            foreach (string argument in this.Arguments)
            {
                // 当前为指定有名参数的名称。
                if (argument.Contains(":") && this.NameComparer.Equals(
                    argument.Remove(argument.IndexOf(":")), paramName))
                {
                    return argument.Substring(argument.IndexOf(":") + ":".Length);
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定位置的无名参数。
        /// </summary>
        /// <param name="paramIndex">要解析的无名参数在所有无名参数中的索引。</param>
        /// <returns>
        /// 索引为 <paramref name="paramIndex"/> 的无名参数的值；
        /// 不存在则返回 <see langword="null"/>。
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="paramIndex"/> 小于 0。
        /// </exception>
        public override string GetParam(int paramIndex)
        {
            // 参数检查。
            if (paramIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(paramIndex));
            }

            int currParamIndex = 0;
            foreach (string argument in this.Arguments)
            {
                // 当前为开关参数名称。
                if (this.SwitchNames.Contains(argument, this.NameComparer))
                {
                    ;
                }
                // 当前为有名参数名称。
                else if (argument.Contains(":") && this.ParamNames.Contains(
                    argument.Remove(argument.IndexOf(":")), this.NameComparer))
                {
                    ;
                }
                // 当前为对应位置的无名参数。
                else if (currParamIndex == paramIndex)
                {
                    return argument;
                }
                // 当前为其他位置的无名参数。
                else
                {
                    currParamIndex++;
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定名称的可重复有名参数。
        /// </summary>
        /// <param name="paramName">要解析的可重复有名参数的名称。</param>
        /// <returns>
        /// 所有名称为 <paramref name="paramName"/> 的有名参数的参数值的数组；
        /// 不存在则返回空数组。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paramName"/> 为 <see langword="null"/>。
        /// </exception>
        public virtual string[] GetParams(string paramName)
        {
            // 参数检查。
            if (paramName is null)
            {
                throw new ArgumentNullException(nameof(paramName));
            }

            var paramValues = new List<string>();
            foreach (string argument in this.Arguments)
            {
                // 当前为指定有名参数的名称。
                if (argument.Contains(":") && this.NameComparer.Equals(
                    argument.Remove(argument.IndexOf(":")), paramName))
                {
                    paramValues.Add(argument.Substring(argument.IndexOf(":") + ":".Length));
                }
            }

            return paramValues.ToArray();
        }
    }
}
