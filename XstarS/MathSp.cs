using System;
using System.Runtime.CompilerServices;

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Square(int value) => value * value;

        /// <summary>
        /// 返回指定数字的指定平方（二次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行平方运算的双精度浮点数。</param>
        /// <returns>数字 <paramref name="value"/> 的平方（二次幂）。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Square(double value) => value * value;

        /// <summary>
        /// 返回指定整数的指定立方（三次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行立方运算的 32 位有符号整数。</param>
        /// <returns>数字 <paramref name="value"/> 的立方（三次幂）。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Cube(int value) => value * value * value;

        /// <summary>
        /// 返回指定整数的指定立方（三次幂）。
        /// </summary>
        /// <remarks>采用连续相乘法实现，明显快于 <see cref="Math.Pow(double, double)"/>。
        /// 在 Release 配置下会被优化为内联函数，与直接连续相乘效率一致。</remarks>
        /// <param name="value">要进行立方运算的双精度浮点数。</param>
        /// <returns>数字 <paramref name="value"/> 的立方（三次幂）。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Cube(double value) => value * value * value;

        /// <summary>
        /// 返回数字的卡马克 (Carmack) 快速平方根算法的结果。
        /// </summary>
        /// <param name="value">要进行平方根运算的单精度浮点数。</param>
        /// <returns><paramref name="value"/> 的快速平方根。</returns>
        public static unsafe float CarmackSqrt(float value)
        {
            float f1 = value, f2 = value;
            int* pi1 = (int*)&f1, pi2 = (int*)&f2;
            *pi1 = 0x1FBCF800 + (*pi1 >> 1);
            *pi2 = 0x5F3759DF - (*pi2 >> 1);
            return 0.5F * (f1 + value * f2);
        }

        /// <summary>
        /// 返回指定数字的卡马克 (Carmack) 快速平方根倒数算法的结果。
        /// </summary>
        /// <param name="value">要进行平方根运算的单精度浮点数。</param>
        /// <returns><paramref name="value"/> 的快速平方根倒数。</returns>
        public static unsafe float CarmackInvSqrt(float value)
        {
            float half = 0.5F * value;
            int* pi = (int*)&value;
            *pi = 0x5F3759DF - (*pi >> 1);
            value = value * (1.5F - half * value * value);
            return value;
        }

        /// <summary>
        /// 确定一个整数是否为质数。
        /// </summary>
        /// <param name="value">要判断是否为质数的 32 位整数。</param>
        /// <returns>若 <paramref name="value"/> 是一个质数，
        /// 为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsPrime(int value)
        {
            // 特殊值处理。
            if (value < 2) { return false; }
            if ((value == 2) || (value == 3)) { return true; }

            // 不在 6 的倍数两侧的一定不是质数。
            if ((value % 6 != 1) && (value % 6 != 5)) { return false; }

            // 质因子寻找到平方根为止。
            double sqrt = Math.Sqrt(value);
            int sqrtFloor = (int)Math.Floor(sqrt);
            // 平方根为整数一定不是质数。
            if (sqrt == sqrtFloor) { return false; }

            // 以 6 为步长开始查找质因子。
            for (int i = 5; i <= sqrtFloor; i += 6)
            {
                if ((value % i == 0) || (value % (i + 2) == 0))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 返回小于指定整数的最大质数。
        /// </summary>
        /// <param name="maxValue">作为最大值的 32 位整数。</param>
        /// <returns>小于 <paramref name="maxValue"/> 的最大质数。
        /// 若不存在此质数，则为 -1。</returns>
        public static int MaxPrime(int maxValue)
        {
            for (int i = maxValue; i >= 2; i--)
            {
                if (MathSp.IsPrime(i))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 返回大于或等于指定整数的最大质数。
        /// </summary>
        /// <param name="minValue">作为最小值的 32 位整数。</param>
        /// <returns>大于或等于 <paramref name="minValue"/> 的最大质数。
        /// 若不存在此质数，则为 -1。</returns>
        public static int MinPrime(int minValue)
        {
            for (int i = minValue; i <= int.MaxValue; i++)
            {
                if (MathSp.IsPrime(i))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
