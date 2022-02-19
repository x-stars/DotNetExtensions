#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS
{
    using IndexPair = ValueTuple<int, int>;

    /// <summary>
    /// 为 <see cref="Range"/> 提供创建枚举器的扩展方法。
    /// </summary>
    public static class RangeEnumerable
    {
        /// <summary>
        /// 返回用于枚举当前 <see cref="Range"/> 中索引值的枚举器。
        /// </summary>
        /// <param name="range">要逐个枚举索引值的 <see cref="Range"/>。</param>
        /// <returns>逐个枚举 <paramref name="range"/> 中索引值的枚举器。</returns>
        public static RangeEnumerator GetEnumerator(this Range range)
        {
            return new RangeEnumerator(range);
        }
    }

    /// <summary>
    /// 为 <see cref="Range"/> 中的索引值提供枚举器。
    /// </summary>
    public struct RangeEnumerator : IEnumerator<int>
    {
        /// <summary>
        /// 表示 <see cref="Range"/> 的包含起始索引值。
        /// </summary>
        private readonly int StartIndex;

        /// <summary>
        /// 表示 <see cref="Range"/> 的不包含结束索引值。
        /// </summary>
        private readonly int EndIndex;

        /// <summary>
        /// 表示枚举器当前位置的索引值。
        /// </summary>
        private int CurrentIndex;

        /// <summary>
        /// 以指定的 <see cref="Range"/> 初始化 <see cref="RangeEnumerator"/> 结构的新实例。
        /// </summary>
        /// <param name="range">要逐个枚举索引值的 <see cref="Range"/>。</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public unsafe RangeEnumerator(Range range)
        {
            var (start, end) = *(IndexPair*)&range;
            this.StartIndex = start - (start >> 31);
            this.EndIndex = end - (end >> 31);
            this.CurrentIndex = this.StartIndex - 1;
        }

        /// <summary>
        /// 获取枚举器当前位置的索引值。
        /// </summary>
        /// <returns>枚举器当前位置的索引值。</returns>
        public int Current => this.CurrentIndex;

        /// <summary>
        /// 获取枚举器当前位置的索引值。
        /// </summary>
        /// <returns>枚举器当前位置的索引值。</returns>
        object IEnumerator.Current => this.CurrentIndex;

        /// <summary>
        /// 将枚举器设置为 <see cref="Range"/> 的末尾之后的位置。
        /// </summary>
        public void Dispose()
        {
            this.CurrentIndex = int.MaxValue - 1;
        }

        /// <summary>
        /// 将枚举器推进到下一个索引值。
        /// </summary>
        /// <returns>如果枚举器成功推进到下一个索引值，则为 <see langword="true"/>；
        /// 如果已到达 <see cref="Range"/> 的末尾，则为 <see langword="false"/>。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            var nextIndex = this.CurrentIndex + 1;
            if (nextIndex >= this.EndIndex) { return false; }
            else { this.CurrentIndex = nextIndex; return true; }
        }

        /// <summary>
        /// 将枚举器设置为其初始位置，该位置位于第一个索引值之前。
        /// </summary>
        public void Reset()
        {
            this.CurrentIndex = this.StartIndex - 1;
        }
    }
}
#endif
