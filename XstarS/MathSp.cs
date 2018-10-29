using System;

namespace XstarS
{
    /// <summary>
    /// 提供针对特殊情况进行优化的数学运算方法。
    /// </summary>
    public static class MathSp
    {
        /// <summary>
        /// 返回指定整数的指定平方（二次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行平方运算的 32 位有符号整数。</param>
        /// <returns>数字 <paramref name="value"/> 的平方（二次幂）。</returns>
        public static int Square(int value) => value * value;

        /// <summary>
        /// 返回指定数字的指定平方（二次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行平方运算的双精度浮点数。</param>
        /// <returns>数字 <paramref name="value"/> 的平方（二次幂）。</returns>
        public static double Square(double value) => value * value;

        /// <summary>
        /// 返回指定整数的指定立方（三次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行立方运算的 32 位有符号整数。</param>
        /// <returns>数字 <paramref name="value"/> 的立方（三次幂）。</returns>
        public static int Cube(int value) => value * value * value;

        /// <summary>
        /// 返回指定整数的指定立方（三次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行立方运算的双精度浮点数。</param>
        /// <returns>数字 <paramref name="value"/> 的立方（三次幂）。</returns>
        public static double Cube(double value) => value * value * value;
    }
}
