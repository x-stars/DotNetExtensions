using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.CommandLine
{
    /// <summary>
    /// Unix / Linux shell 风格的命令行参数解析器，参数名称区分大小写。
    /// </summary>
    /// <remarks>
    /// 支持连字符 "-" 后接多个开关参数的解析。
    /// 暂不支持连字符 "-" 开头的参数值的解析。
    /// 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
    /// 不支持一个参数名称后跟多个参数值的有名参数的解析。
    /// </remarks>
    [Serializable]
    public class UnixShellArgumentReader : ArgumentReader
    {
        /// <summary>
        /// 初始化 Unix / Linux Shell 风格命令行参数解析器
        /// <see cref="UnixShellArgumentReader"/> 的新实例。
        /// </summary>
        /// <remarks><para>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可留空。
        /// </para><para>
        /// 参数名称应带有作为提示符的连字符 "-" 和双连字符 "--"。
        /// 参数名称列表应同时包含长名称和短名称。
        /// </para></remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="parameterNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public UnixShellArgumentReader(string[] arguments,
            string[] parameterNames = null, string[] switchNames = null)
            : base(arguments, false, parameterNames, switchNames) { }

        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="name">要解析的有名参数的名称，以逗号 "," 分隔同义名称。 </param>
        /// <returns>名称为 <paramref name="name"/> 的有名参数的值；
        /// 不存在则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> 不带参数提示符 "-" 或长度过短。</exception>
        public override string GetParameter(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var message = new ArgumentException().Message;
            var alterNames = name.Split(',');
            foreach (string alterName in alterNames)
            {
                if (alterName.StartsWith("--"))
                {
                    if (alterName.Length < 3)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                }
                else if (alterName.StartsWith("-"))
                {
                    if (alterName.Length < 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                    else if (alterName.Length > 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                }
                else
                {
                    throw new ArgumentException(message, nameof(name));
                }

                if (base.GetParameter(alterName) is string value)
                {
                    return value;
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

            for (int index = 0, positionNow = 0; index < this.Arguments.Count; index++)
            {
                if (this.ParameterNames.Contains(this.Arguments[index], this.NameComparer))
                {
                    index++;
                }
                else if (this.Arguments[index].StartsWith("-"))
                {
                }
                else if (positionNow == position)
                {
                    return this.Arguments[index];
                }
                else
                {
                    positionNow++;
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定名称的开关参数。
        /// </summary>
        /// <param name="name">要解析的开关参数的名称，以逗号 "," 分隔同义名称。</param>
        /// <returns>若名称为 <paramref name="name"/> 的开关参数存在，
        /// 则为 <see langword="true"/>； 否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> 不带参数提示符 "-" 或长度过短。</exception>
        public override bool GetSwitch(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var message = new ArgumentException().Message;
            var alterNames = name.Split(',');
            foreach (string alterName in alterNames)
            {
                if (alterName.StartsWith("--"))
                {
                    if (alterName.Length < 3)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                    else
                    {
                        if (base.GetSwitch(alterName))
                        {
                            return true;
                        }
                    }
                }
                else if (alterName.StartsWith("-"))
                {
                    if (alterName.Length < 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                    else if (alterName.Length > 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                    else
                    {
                        if (this.Arguments.Any(argument =>
                            argument.StartsWith("-") && !argument.StartsWith("--") &&
                            argument.Contains(alterName.Substring(1, 1))))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException(message, nameof(name));
                }
            }

            return false;
        }

        /// <summary>
        /// 解析指定名称的可重复有名参数。
        /// </summary>
        /// <param name="name">要解析的可重复有名参数的名称。</param>
        /// <returns>所有名称为 <paramref name="name"/> 的有名参数的参数值的数组；
        /// 若不存在则为空数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> 不带参数提示符 "-" 或长度过短。</exception>
        public virtual string[] GetParameters(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var message = new ArgumentException().Message;
            var values = new List<string>();
            var alterNames = name.Split(',');
            foreach (string alterName in alterNames)
            {
                if (alterName.StartsWith("--"))
                {
                    if (alterName.Length < 3)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                }
                else if (alterName.StartsWith("-"))
                {
                    if (alterName.Length < 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                    else if (alterName.Length > 2)
                    {
                        throw new ArgumentException(message, nameof(name));
                    }
                }
                else
                {
                    throw new ArgumentException(message, nameof(name));
                }

                for (int i = 0; i < this.Arguments.Count - 1; i++)
                {
                    if (this.NameComparer.Equals(this.Arguments[i], name))
                    {
                        values.Add(this.Arguments[i + 1]);
                    }
                }
            }

            return values.ToArray();
        }
    }
}
