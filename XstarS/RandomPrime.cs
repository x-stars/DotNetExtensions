using System;

namespace XstarS
{
    /// <summary>
    /// 伪随机质数生成器。
    /// </summary>
    [Serializable]
    public class RandomPrime : Random
    {
        /// <summary>
        /// 初始化 <see cref="RandomPrime"/> 类的新实例，使用依赖于时间的默认种子值。
        /// </summary>
        public RandomPrime() : base() { }

        /// <summary>
        /// 初始化 <see cref="RandomPrime"/> 类的新实例，使用指定的种子值。
        /// </summary>
        /// <param name="seed">用来计算伪随机数序列起始值的数字。
        /// 如果指定的是负数，则使用其绝对值。</param>
        public RandomPrime(int seed) : base(seed) { }

        /// <summary>
        /// 返回一个随机质数。
        /// </summary>
        /// <returns>一个随机质数。</returns>
        public override int Next()
        {
            while (true)
            {
                int ceiling = base.Next();
                int prime = MathSp.MaxPrime(ceiling);
                if (prime != -1) { return prime; }
            }
        }

        /// <summary>
        /// 返回一个在指定范围内的随机质数。
        /// </summary>
        /// <param name="minValue">返回的随机质数的下限。</param>
        /// <param name="maxValue">返回的随机质数的上限，应大于或等于 <paramref name="minValue"/>。</param>
        /// <returns>大于或等于 <paramref name="minValue"/> 且小于 <paramref name="maxValue"/> 的随机质数；
        /// 若不存在此质数，则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="minValue"/> 大于 <paramref name="maxValue"/>。</exception>
        public override int Next(int minValue, int maxValue)
        {
            if (MathSp.MaxPrime(maxValue) == -1) { return -1; }
            
            while (true)
            {
                int ceiling = base.Next(minValue, maxValue);
                int prime = MathSp.MaxPrime(ceiling);
                if (prime != -1) { return prime; }
            }
        }

        /// <summary>
        /// 返回一个小于指定整数的随机质数。
        /// </summary>
        /// <param name="maxValue">返回的随机质数的上限。</param>
        /// <returns>小于 <paramref name="maxValue"/> 的随机质数；
        /// 若不存在此质数，则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxValue"/> 小于 0。</exception>
        public override int Next(int maxValue)
        {
            if (MathSp.MaxPrime(maxValue) == -1) { return -1; }

            while (true)
            {
                int ceiling = base.Next(maxValue);
                int prime = MathSp.MaxPrime(ceiling);
                if (prime != -1) { return prime; }
            }
        }

        /// <summary>
        /// 用 8 位无符号整数型的随机质数填充指定字节数组的元素。
        /// </summary>
        /// <param name="buffer">包含随机质数的字节数组。</param>
        public override void NextBytes(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)this.Next(byte.MaxValue + 1);
            }
        }

        /// <summary>
        /// 返回一个大于或等于 0.0 且小于 1.0 的随机浮点数。
        /// 不支持此方法，总是抛出 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <returns>不支持此方法，总是抛出 <see cref="NotSupportedException"/> 异常。</returns>
        public override double NextDouble()
        {
            throw new NotSupportedException();
        }
    }
}
