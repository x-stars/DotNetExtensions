using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// PowerShell 风格的命令行参数解析器，参数名称忽略大小写。
    /// </summary>
    /// <remarks>
    /// 不支持无名参数的解析，但支持省略参数名称的有名参数的解析。
    /// 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
    /// 不支持一个参数名称后跟多个参数值的有名参数的解析。
    /// 不支持多个同名的有名参数的解析。
    /// </remarks>
    public class PowerShellParamReader : ParamReader, IEquatable<PowerShellParamReader>
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
        /// 初始化 PowerShell 风格命令行参数解析器
        /// <see cref="PowerShellParamReader"/> 的新实例。
        /// </summary>
        /// <remarks><para>
        /// 输入的参数名称列表用于解析省略参数名称的有名参数；若无需解析省略参数名称的有名参数，可不写。
        /// </para><para>
        /// 参数名称应带有作为提示符的连字符 "-"。
        /// 有名参数名称的顺序会影响省略参数名称的有名参数的解析，因此必选参数应在可选参数之前。
        /// 开关参数名称的顺序则不会产生影响，可任意指定顺序。
        /// </para></remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="paramNames">所有有名参数名称列表，必选参数应在可选参数之前。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public PowerShellParamReader(string[] arguments,
            string[] paramNames = null, string[] switchNames = null) :
            base(arguments, true, paramNames, switchNames)
        {
            this.arguments = arguments ?? new string[0];
            this.paramNames = paramNames ?? new string[0];
            this.switchNames = switchNames ?? new string[0];
            this.stringComparer = StringComparer.InvariantCultureIgnoreCase;
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="PowerShellParamReader"/> 对象是否相等。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="PowerShellParamReader"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(PowerShellParamReader other)
        {
            return other != null &&
                EqualityComparer<string[]>.Default.Equals(this.arguments, other.arguments) &&
                EqualityComparer<string[]>.Default.Equals(this.paramNames, other.paramNames) &&
                EqualityComparer<string[]>.Default.Equals(this.switchNames, other.switchNames);
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="PowerShellParamReader"/> 的实例，且初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as PowerShellParamReader);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = 25551442;
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.arguments);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.paramNames);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.switchNames);
            return hashCode;
        }

        /// <summary>
        /// 解析指定名称的有名参数。
        /// </summary>
        /// <param name="paramName">要解析的有名参数的名称。</param>
        /// <returns>
        /// 名称为 <paramref name="paramName"/> 的有名参数的值；
        /// 不存在则按顺序寻找无名参数的值并返回；
        /// 仍然不存在则返回 <see langword="null"/>。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="paramName"/> 为 <see langword="null"/>。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="paramName"/> 不带参数提示符 "-" 或长度过短。
        /// </exception>
        public override string GetParam(string paramName)
        {
            // 参数检查。
            if (paramName is null)
            { throw new ArgumentNullException(nameof(paramName)); }
            else if (paramName.StartsWith("-"))
            {
                if (paramName.Length < 2)
                { throw new ArgumentException("TooShort", nameof(paramName)); }
            }
            else
            { throw new ArgumentException("NoPrefix", nameof(paramName)); }

            // 未省略参数名称。
            if (base.GetParam(paramName) is string paramValue)
            { return paramValue; }
            // 省略参数名称。
            else
            {
                int paramIndex = 0;
                foreach (string currParamName in this.paramNames)
                {
                    // 当前为指定有名参数。
                    if (this.stringComparer.Equals(currParamName, paramName))
                    {
                        if (base.GetParam(currParamName) is string paramValueByName)
                        { return paramValueByName; }
                        else if (base.GetParam(paramIndex) is string paramValueByIndex)
                        { return paramValueByIndex; }
                        else
                        { return null; }
                    }
                    // 当前为其他有名参数。
                    else
                    {
                        if (base.GetParam(currParamName) is string paramValueByName)
                        { }
                        else if (base.GetParam(paramIndex) is string paramValueByIndex)
                        { paramIndex++; }
                        else
                        { return null; }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 解析指定位置的无名参数。
        /// 不支持此方法，总是抛出 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <param name="paramIndex">要解析的无名参数在所有无名参数中的索引。</param>
        /// <returns><see cref="NotSupportedException"/> 异常。</returns>
        /// <exception cref="NotSupportedException">
        /// 不支持此方法，总是抛出此异常。
        /// </exception>
        public override string GetParam(int paramIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 解析指定名称的开关参数。
        /// </summary>
        /// <param name="switchName">要解析的开关参数的名称。</param>
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
            // 参数检查。
            if (switchName is null)
            { throw new ArgumentNullException(nameof(switchName)); }
            else if (switchName.StartsWith("-"))
            {
                if (switchName.Length < 2)
                { throw new ArgumentException("TooShort", nameof(switchName)); }
            }
            else
            { throw new ArgumentException("NoPrefix", nameof(switchName)); }

            // 解析开关参数。
            return base.GetSwitch(switchName);
        }

        /// <summary>
        /// 指示两 <see cref="PowerShellParamReader"/> 对象是否相等。
        /// </summary>
        /// <param name="reader1">第一个对象。</param>
        /// <param name="reader2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="reader1"/> 和 <paramref name="reader2"/> 的初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(PowerShellParamReader reader1, PowerShellParamReader reader2) =>
            EqualityComparer<PowerShellParamReader>.Default.Equals(reader1, reader2);

        /// <summary>
        /// 指示两 <see cref="PowerShellParamReader"/> 对象是否不等。
        /// </summary>
        /// <param name="reader1">第一个对象。</param>
        /// <param name="reader2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="reader1"/> 和 <paramref name="reader2"/> 的初始化参数不同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(PowerShellParamReader reader1, PowerShellParamReader reader2) =>
            !(reader1 == reader2);
    }
}
