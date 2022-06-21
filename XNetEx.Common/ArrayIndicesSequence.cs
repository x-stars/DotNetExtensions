using System;
using System.Collections;
using System.Collections.Generic;

namespace XNetEx;

/// <summary>
/// 为多维数组提其索引数组的 <see cref="IEnumerable{T}"/> 序列。
/// </summary>
internal sealed class ArrayIndicesSequence : IEnumerable<int[]>
{
    /// <summary>
    /// 表示当前多维数组的元素的总数。
    /// </summary>
    private readonly long ArrayLength;

    /// <summary>
    /// 表示当前多维数组的各维度的第一个元素的索引。
    /// </summary>
    private readonly int[] LowerBounds;

    /// <summary>
    /// 表示当前多维数组的各维度的最后一个元素的索引。
    /// </summary>
    private readonly int[] UpperBounds;

    /// <summary>
    /// 表示当前序列在返回索引数组时是否复用同一数组。
    /// </summary>
    private readonly bool ReuseIndices;

    /// <summary>
    /// 使用要枚举索引数组的多维数组初始化 <see cref="ArrayIndicesSequence"/>
    /// 类的新实例，并指定在返回索引数组时是否复用同一数组。
    /// </summary>
    /// <param name="array">要枚举索引数组的多维数组。</param>
    /// <param name="reuseIndices">指定在返回索引数组时是否复用同一数组。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public ArrayIndicesSequence(Array array, bool reuseIndices = false)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var rank = array.Rank;
        var lowerBounds = new int[rank];
        var upperBounds = new int[rank];
        for (int dim = 0; dim < rank; dim++)
        {
            lowerBounds[dim] = array.GetLowerBound(dim);
            upperBounds[dim] = array.GetUpperBound(dim);
        }

        this.ArrayLength = array.LongLength;
        this.LowerBounds = lowerBounds;
        this.UpperBounds = upperBounds;
        this.ReuseIndices = reuseIndices;
    }

    /// <summary>
    /// 获取当前多维数组的索引数组的总数。
    /// </summary>
    /// <returns>当前多维数组的索引数组的总数。</returns>
    /// <exception cref="OverflowException">
    /// 当前多维数组包含的元素总数超出 32 位有符号整数能表示的范围。</exception>
    public int Length => checked((int)this.ArrayLength);

    /// <summary>
    /// 返回一个用于枚举当前多维数组的索引数组的枚举数。
    /// </summary>
    /// <returns>一个用于枚举当前多维数组的索引数组的枚举数。</returns>
    public IEnumerator<int[]> GetEnumerator() => new Enumerator(this);

    /// <summary>
    /// 返回一个用于枚举当前多维数组的索引数组的枚举数。
    /// </summary>
    /// <returns>一个用于枚举当前多维数组的索引数组的枚举数。</returns>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    /// <summary>
    /// 为多维数组提其索引数组的 <see cref="IEnumerator{T}"/> 枚举数。
    /// </summary>
    private sealed class Enumerator : IEnumerator<int[]>
    {
        /// <summary>
        /// 表示当前多维数组的元素的总数。
        /// </summary>
        private readonly long ArrayLength;

        /// <summary>
        /// 表示当前多维数组的各维度的第一个元素的索引。
        /// </summary>
        private readonly int[] LowerBounds;

        /// <summary>
        /// 表示当前多维数组的各维度的最后一个元素的索引。
        /// </summary>
        private readonly int[] UpperBounds;

        /// <summary>
        /// 表示当前枚举器在返回索引数组时是否复用同一数组。
        /// </summary>
        private readonly bool ReuseIndices;

        /// <summary>
        /// 表示位于枚举数当前位置的索引数组。
        /// </summary>
        private readonly int[] CurrentIndices;

        /// <summary>
        /// 使用包含数组索引信息的 <see cref="ArrayIndicesSequence"/>
        /// 初始化 <see cref="ArrayIndicesSequence.Enumerator"/> 类的新实例。
        /// </summary>
        /// <param name="indicesInfo">包含数组索引信息的
        /// <see cref="ArrayIndicesSequence"/> 对象。</param>
        internal Enumerator(ArrayIndicesSequence indicesInfo)
        {
            this.ArrayLength = indicesInfo.ArrayLength;
            this.LowerBounds = indicesInfo.LowerBounds;
            this.UpperBounds = indicesInfo.UpperBounds;
            this.ReuseIndices = indicesInfo.ReuseIndices;
            var rank = this.LowerBounds.Length;
            this.CurrentIndices = new int[rank];
            this.Reset();
        }

        /// <summary>
        /// 获取位于枚举数当前位置的索引数组。
        /// </summary>
        /// <returns>位于枚举数当前位置的索引数组。</returns>
        /// <exception cref="InvalidOperationException">
        /// 枚举尚未开始；或枚举已完成。</exception>
        public int[] Current => this.GetCurrent();

        /// <summary>
        /// 获取位于枚举数当前位置的索引数组。
        /// </summary>
        /// <returns>位于枚举数当前位置的多维数组的索引数组。</returns>
        /// <exception cref="InvalidOperationException">
        /// 枚举尚未开始；或枚举已完成。</exception>
        object IEnumerator.Current => this.Current;

        /// <summary>
        /// 释放当前实例占有的资源。
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// 获取位于枚举数当前位置的索引数组。
        /// </summary>
        /// <returns>位于枚举数当前位置的索引数组。</returns>
        /// <exception cref="InvalidOperationException">
        /// 枚举尚未开始；或枚举已完成。</exception>
        public int[] GetCurrent()
        {
            var indices = this.CurrentIndices;
            var lowerBounds = this.LowerBounds;
            var upperBounds = this.UpperBounds;
            if ((indices[^1] < lowerBounds[^1]) ||
                (indices[0] > upperBounds[0]))
            {
                throw new InvalidOperationException();
            }

            if (!this.ReuseIndices)
            {
                var indicesCopy = new int[indices.Length];
                Buffer.BlockCopy(indices, 0, indicesCopy, 0,
                                 indices.Length * sizeof(int));
                indices = indicesCopy;
            }
            return indices;
        }

        /// <summary>
        /// 将枚举数推进到当前数组的下一个索引。
        /// </summary>
        /// <returns>如果枚举数已成功地推进到下一个位置，则为 <see langword="true"/>；
        /// 如果枚举数已经到数组索引的末尾，则为 <see langword="false"/>。</returns>
        public bool MoveNext()
        {
            var indices = this.CurrentIndices;
            var lowerBounds = this.LowerBounds;
            var upperBounds = this.UpperBounds;
            if (indices[0] > upperBounds[0])
            {
                return false;
            }

            indices[^1] += 1;
            var rank = indices.Length;
            for (int dim = rank - 1; dim >= 1; dim--)
            {
                if (indices[dim] <= upperBounds[dim])
                {
                    break;
                }
                indices[dim] = lowerBounds[dim];
                indices[dim - 1] += 1;
            }
            return indices[0] <= upperBounds[0];
        }

        /// <summary>
        /// 将枚举数设置为其初始位置，该位置位于数组的第一个索引之前。
        /// </summary>
        public void Reset()
        {
            var indices = this.CurrentIndices;
            var lowerBounds = this.LowerBounds;
            Buffer.BlockCopy(lowerBounds, 0, indices, 0,
                             indices.Length * sizeof(int));
            if (this.ArrayLength == 0)
            {
                indices[0] += 1;
            }
            indices[^1] -= 1;
        }
    }
}
