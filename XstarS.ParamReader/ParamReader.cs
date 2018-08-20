using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 简单的通用命令行参数解析器 <see cref="ParamReader"/>。
    /// 为命令行参数解析器提供基类和默认实现。
    /// </summary>
    /// <remarks>
    /// 不支持 Unix / Linux shell 中连字符 "-" 后接多个开关参数的解析。
    /// 不支持 PowerShell 中允许省略参数名称的有名参数的解析。
    /// 不支持一个参数名称后跟多个参数值的有名参数的解析。
    /// 不支持多个同名的有名参数的解析。
    /// </remarks>
    public class ParamReader : IEquatable<ParamReader>
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
        /// 初始化命令行参数解析器 <see cref="ParamReader"/> 的新实例。
        /// </summary>
        /// <remarks>
        /// 输入的参数名称列表用于解析无名参数；若无需解析无名参数，可不写。
        /// </remarks>
        /// <param name="arguments">待解析的参数列表。</param>
        /// <param name="ignoreCase">参数名称是否忽略大小写。</param>
        /// <param name="paramNames">所有有名参数名称列表。</param>
        /// <param name="switchNames">所有开关参数名称列表。</param>
        public ParamReader(string[] arguments,
            bool ignoreCase = true, string[] paramNames = null, string[] switchNames = null)
        {
            this.arguments = arguments ?? new string[0];
            this.switchNames = switchNames ?? new string[0];
            this.paramNames = paramNames ?? new string[0];
            this.stringComparer = ignoreCase ?
                StringComparer.InvariantCultureIgnoreCase :
                StringComparer.InvariantCulture;
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="ParamReader"/> 对象是否相等。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="ParamReader"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(ParamReader other) =>
            !(other is null) &&
            EqualityComparer<string[]>.Default.Equals(this.arguments, other.arguments) &&
            EqualityComparer<string[]>.Default.Equals(this.paramNames, other.paramNames) &&
            EqualityComparer<string[]>.Default.Equals(this.switchNames, other.switchNames) &&
            EqualityComparer<IEqualityComparer<string>>.Default.Equals(this.stringComparer, other.stringComparer);

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="ParamReader"/> 的实例，且初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj) =>
            this.Equals(obj as ParamReader);

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = -201791849;
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.arguments);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.paramNames);
            hashCode = hashCode * -1521134295 + EqualityComparer<string[]>.Default.GetHashCode(this.switchNames);
            hashCode = hashCode * -1521134295 + EqualityComparer<IEqualityComparer<string>>.Default.GetHashCode(this.stringComparer);
            return hashCode;
        }

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
        public virtual string GetParam(string paramName)
        {
            // 参数检查。
            if (paramName is null)
            { throw new ArgumentNullException(nameof(paramName)); }

            for (int i = 0; i < this.arguments.Length - 1; i++)
            {
                // 当前为指定有名参数的名称。
                if (this.stringComparer.Equals(this.arguments[i], paramName))
                { return this.arguments[i + 1]; }
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
        public virtual string GetParam(int paramIndex)
        {
            // 参数检查。
            if (paramIndex < 0)
            { throw new ArgumentOutOfRangeException(nameof(paramIndex)); }

            for (int i = 0, currParamIndex = 0; i < this.arguments.Length; i++)
            {
                // 当前为开关参数名称。
                if (this.switchNames.Contains(this.arguments[i], this.stringComparer))
                { }
                // 当前为有名参数名称。
                else if (this.paramNames.Contains(this.arguments[i], this.stringComparer))
                { i++; }
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
        /// <param name="switchName">要解析的开关参数的名称。</param>
        /// <returns>
        /// 名称为 <paramref name="switchName"/> 的开关参数存在，
        /// 为 <see langword="true"/>； 否则为 <see langword="false"/>。
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="switchName"/> 为 <see langword="null"/>。
        /// </exception>
        public virtual bool GetSwitch(string switchName)
        {
            // 参数检查。
            if (switchName is null)
            { throw new ArgumentNullException(nameof(switchName)); }

            // 直接检查参数列表中是否包含指定开关参数。
            return this.arguments.Contains(switchName, this.stringComparer);
        }

        /// <summary>
        /// 指示两 <see cref="ParamReader"/> 对象是否相等。
        /// </summary>
        /// <param name="reader1">第一个对象。</param>
        /// <param name="reader2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="reader1"/> 和 <paramref name="reader2"/> 的初始化参数相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(ParamReader reader1, ParamReader reader2) =>
            EqualityComparer<ParamReader>.Default.Equals(reader1, reader2);

        /// <summary>
        /// 指示两 <see cref="ParamReader"/> 对象是否不等。
        /// </summary>
        /// <param name="reader1">第一个对象。</param>
        /// <param name="reader2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="reader1"/> 和 <paramref name="reader2"/> 的初始化参数不同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(ParamReader reader1, ParamReader reader2) =>
            !(reader1 == reader2);
    }
}
