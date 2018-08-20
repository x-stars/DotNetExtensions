using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 多值命令行参数解析器 <see cref="MultiValueParamReader"/>。
    /// 一个参数名能出现多次，之后也能接多个参数值。
    /// </summary>
    public class MultiValueParamReader
    {
        /// <summary>
        /// 初始化 <see cref="MultiValueParamReader"/> 类的新实例。
        /// </summary>
        public MultiValueParamReader() { }

        /// <summary>
        /// 解析指定名称的多值有名参数。
        /// </summary>
        /// <param name="paramName">要解析的多值有名参数的名称。</param>
        /// <param name="length">多值有名参数的参数值的数量。</param>
        /// <returns>
        /// 名称为 <paramref name="paramName"/> 的多值有名参数的所有参数值的数组；
        /// 不存在则返回空数组。
        /// </returns>
        public string[] GetParam(string paramName, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解析指定名称的可重复有名参数。
        /// </summary>
        /// <param name="paramName">要解析的可重复有名参数的名称。</param>
        /// <returns>
        /// 所有名称为 <paramref name="paramName"/> 的有名参数的参数值的数组；
        /// 不存在则返回空数组。
        /// </returns>
        public string[] GetParams(string paramName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解析指定名称的可重复多值有名参数。
        /// </summary>
        /// <param name="paramName">要解析的可重复多值有名参数的名称。</param>
        /// <param name="length">多值有名参数的参数值的数量。</param>
        /// <returns>
        /// 所有名称为 <paramref name="paramName"/> 的多值有名参数的所有参数值的数组；
        /// 不存在则返回空数组。
        /// </returns>
        public string[][] GetParams(string paramName, int length)
        {
            throw new NotImplementedException();
        }
    }
}
