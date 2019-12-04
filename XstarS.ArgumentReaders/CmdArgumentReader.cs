using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.CommandLine
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
    [Serializable]
    public class CmdArgumentReader : ArgumentReader
    {
        /// <summary>
        /// 初始化 CMD 风格命令行参数解析器 <see cref="CmdArgumentReader"/> 的新实例。
        /// </summary>
        /// <remarks>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可留空。
        /// </remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="parameterNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public CmdArgumentReader(string[] arguments,
            string[] parameterNames = null, string[] switchNames = null)
            : base(arguments, true, parameterNames, switchNames) { }

        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="name">要解析的有名参数的名称。</param>
        /// <returns>名称为 <paramref name="name"/> 的有名参数的值；
        /// 若不存在则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public override string GetParameter(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            foreach (string argument in this.Arguments)
            {
                if (argument.Contains(":") && this.NameComparer.Equals(
                    argument.Remove(argument.IndexOf(":")), name))
                {
                    return argument.Substring(argument.IndexOf(":") + ":".Length);
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定位置的无名参数。
        /// </summary>
        /// <param name="position">要解析的无名参数在所有无名参数中的位置。</param>
        /// <returns>位置为 <paramref name="position"/> 的无名参数的值；
        /// 若不存在则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position"/> 小于 0。</exception>
        public override string GetParameter(int position)
        {
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            int positionNow = 0;
            foreach (string argument in this.Arguments)
            {
                if (this.SwitchNames.Contains(argument, this.NameComparer))
                {
                }
                else if (argument.Contains(":") && this.ParameterNames.Contains(
                    argument.Remove(argument.IndexOf(":")), this.NameComparer))
                {
                }
                else if (positionNow == position)
                {
                    return argument;
                }
                else
                {
                    positionNow++;
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定名称的可重复有名参数。
        /// </summary>
        /// <param name="name">要解析的可重复有名参数的名称。</param>
        /// <returns>所有名称为 <paramref name="name"/> 的有名参数的参数值的数组；
        /// 若不存在则为空数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        public virtual string[] GetParameters(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var values = new List<string>();
            foreach (string argument in this.Arguments)
            {
                if (argument.Contains(":") && this.NameComparer.Equals(
                    argument.Remove(argument.IndexOf(":")), name))
                {
                    values.Add(argument.Substring(argument.IndexOf(":") + ":".Length));
                }
            }

            return values.ToArray();
        }
    }
}
