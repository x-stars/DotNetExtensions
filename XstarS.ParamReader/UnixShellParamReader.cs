using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
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
    public class UnixShellParamReader : ParamReader
    {
        /// <summary>
        /// 待解析的参数列表。
        /// </summary>
        private readonly string[] arguments;
        /// <summary>
        /// 有名参数名称列表。
        /// </summary>
        private readonly string[] paramNames;
        /// <summary>
        /// 开关参数名称列表。
        /// </summary>
        private readonly string[] switchNames;
        /// <summary>
        /// 比较参数名称时采用的字符串比较器。
        /// </summary>
        private readonly IEqualityComparer<string> stringComparer;

        /// <summary>
        /// 初始化 Unix / Linux Shell 风格命令行参数解析器
        /// <see cref="UnixShellParamReader"/> 的新实例。
        /// </summary>
        /// <remarks><para>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可不写。
        /// </para><para>
        /// 参数名称应带有作为提示符的连字符 "-" 和双连字符 "--"。
        /// 参数名称列表应同时包含长名称和短名称。
        /// </para></remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="paramNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public UnixShellParamReader(string[] arguments,
            string[] paramNames = null, string[] switchNames = null) :
            base(arguments, false, paramNames, switchNames)
        {
            this.arguments = arguments ?? new string[0];
            this.paramNames = paramNames ?? new string[0];
            this.switchNames = switchNames ?? new string[0];
            this.stringComparer = StringComparer.InvariantCulture;
        }
        
        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="paramName">要解析的有名参数的名称，以逗号 "," 分隔同义名称。 </param>
        /// <returns>
        /// 名称为 <paramref name="paramName"/> 的有名参数的值；
        /// 不存在则返回 <see langword="null"/>。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paramName"/> 为 <see langword="null"/>。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="paramName"/> 不带参数提示符 "-" 或长度过短。
        /// </exception>
        public override string GetParam(string paramName)
        {
            // null 检查。
            if (paramName is null)
            { throw new ArgumentNullException(nameof(paramName)); }

            // 分隔同义名称。
            string[] alterParamNames = paramName.Split(',');
            foreach (string alterParamName in alterParamNames)
            {
                // 参数检查。
                if (alterParamName.StartsWith("--"))
                {
                    if (alterParamName.Length < 3)
                    { throw new ArgumentException("LongNameTooShort", nameof(paramName)); }
                }
                else if (alterParamName.StartsWith("-"))
                {
                    if (alterParamName.Length < 2)
                    { throw new ArgumentException("ShortNameTooShort", nameof(paramName)); }
                    else if (alterParamName.Length > 2)
                    { throw new ArgumentException("ShortNameTooLong", nameof(paramName)); }
                }
                else
                { throw new ArgumentException("NoPrefix", nameof(paramName)); }

                // 尝试解析有名参数。
                if (base.GetParam(alterParamName) is string paramValue)
                { return paramValue; }
            }

            // 全部解析失败。
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
            { throw new ArgumentOutOfRangeException(nameof(paramIndex)); }

            for (int i = 0, currParamIndex = 0; i < this.arguments.Length; i++)
            {
                // 当前为有名参数名称。
                if (this.paramNames.Contains(this.arguments[i], this.stringComparer))
                { i++; }
                // 当前为开关参数名称。
                else if (this.arguments[i].StartsWith("-"))
                { }
                // 当前为对应位置的无名参数。
                else if (currParamIndex == paramIndex)
                { return this.arguments[i]; }
                // 当前为其他位置的无名参数。
                else
                { currParamIndex++; }
            }

            return null;
        }

        /// <summary>
        /// 解析指定名称的开关参数。
        /// </summary>
        /// <param name="switchName">要解析的开关参数的名称，以逗号 "," 分隔同义名称。</param>
        /// <returns>
        /// 名称为 <paramref name="switchName"/> 的开关参数存在，
        /// 为 <see langword="true"/>； 否则为 <see langword="false"/>。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="switchName"/> 为 <see langword="null"/>。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="switchName"/> 不带参数提示符 "-" 或长度过短。
        /// </exception>
        public override bool GetSwitch(string switchName)
        {
            // null 检查。
            if (switchName is null)
            { throw new ArgumentNullException(nameof(switchName)); }

            // 分隔同义名称。
            string[] alterSwitchNames = switchName.Split(',');
            foreach (string alterSwitchName in alterSwitchNames)
            {
                // 当前为长名称。
                if (alterSwitchName.StartsWith("--"))
                {
                    // 参数检查。
                    if (alterSwitchName.Length < 3)
                    { throw new ArgumentException("LongNameTooShort", nameof(switchName)); }
                    else
                    {
                        // 尝试解析长开关参数。
                        if (base.GetSwitch(alterSwitchName))
                        { return true; }
                    }
                }
                // 当前为短名称。
                else if (alterSwitchName.StartsWith("-"))
                {
                    // 参数检查。
                    if (alterSwitchName.Length < 2)
                    { throw new ArgumentException("ShortNameTooShort", nameof(switchName)); }
                    else if (alterSwitchName.Length > 2)
                    { throw new ArgumentException("ShortNameTooLong", nameof(switchName)); }
                    else
                    {
                        // 尝试解析短开关参数。
                        if (this.arguments.Any(
                            arg =>
                            arg.StartsWith("-") &&
                            !arg.StartsWith("--") &&
                            arg.Contains(alterSwitchName.Substring(1, 1))))
                        { return true; }
                    }
                }
                else
                { throw new ArgumentException("NoPrefix", nameof(switchName)); }
            }

            // 全部解析失败。
            return false;
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
        /// <exception cref="ArgumentException">
        /// <paramref name="paramName"/> 不带参数提示符 "-" 或长度过短。
        /// </exception>
        public virtual string[] GetParams(string paramName)
        {
            // null 检查。
            if (paramName is null)
            { throw new ArgumentNullException(nameof(paramName)); }

            var paramValueList = new List<string>();
            // 分隔同义名称。
            string[] alterParamNames = paramName.Split(',');
            foreach (string alterParamName in alterParamNames)
            {
                // 参数检查。
                if (alterParamName.StartsWith("--"))
                {
                    if (alterParamName.Length < 3)
                    { throw new ArgumentException("LongNameTooShort", nameof(paramName)); }
                }
                else if (alterParamName.StartsWith("-"))
                {
                    if (alterParamName.Length < 2)
                    { throw new ArgumentException("ShortNameTooShort", nameof(paramName)); }
                    else if (alterParamName.Length > 2)
                    { throw new ArgumentException("ShortNameTooLong", nameof(paramName)); }
                }
                else
                { throw new ArgumentException("NoPrefix", nameof(paramName)); }

                // 尝试解析所有同名有名参数。
                for (int i = 0; i < this.arguments.Length - 1; i++)
                {
                    // 当前为指定有名参数的名称。
                    if (this.stringComparer.Equals(this.arguments[i], paramName))
                    { paramValueList.Add(this.arguments[i + 1]); }
                }
            }
            
            return paramValueList.ToArray();
        }
    }
}
