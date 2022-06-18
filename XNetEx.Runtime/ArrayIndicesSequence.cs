using System;
using System.Collections;
using System.Collections.Generic;

namespace XNetEx
{
    internal sealed class ArrayIndicesSequence : IEnumerable<int[]>
    {
        private readonly int[] LowerBounds;

        private readonly int[] UpperBounds;

        private readonly bool ReuseIndices;

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

            this.LowerBounds = lowerBounds;
            this.UpperBounds = upperBounds;
            this.ReuseIndices = reuseIndices;
        }

        public IEnumerator<int[]> GetEnumerator() =>
            new Enumerator(this.LowerBounds, this.UpperBounds, this.ReuseIndices);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        internal sealed class Enumerator : IEnumerator<int[]>
        {
            private readonly int[] LowerBounds;

            private readonly int[] UpperBounds;

            private readonly bool ReuseIndices;

            private readonly int[] CurrentIndices;

            internal Enumerator(int[] lowerBounds, int[] upperBounds,
                                bool reuseIndices = false)
            {
                this.LowerBounds = lowerBounds;
                this.UpperBounds = upperBounds;
                this.ReuseIndices = reuseIndices;
                var rank = lowerBounds.Length;
                this.CurrentIndices = new int[rank];
                this.Reset();
            }

            public int[] Current => this.GetCurrent();

            object IEnumerator.Current => this.Current;

            public void Dispose() { }

            public int[] GetCurrent()
            {
                var indices = this.CurrentIndices;
                var lowerBounds = this.LowerBounds;
                if (indices[^1] < lowerBounds[^1])
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

            public void Reset()
            {
                var indices = this.CurrentIndices;
                var lowerBounds = this.LowerBounds;
                var upperBounds = this.UpperBounds;
                Buffer.BlockCopy(lowerBounds, 0, indices, 0,
                                 indices.Length * sizeof(int));

                for (int dim = 0; dim < indices.Length; dim++)
                {
                    if (lowerBounds[dim] > upperBounds[dim])
                    {
                        indices[0] += 1;
                        break;
                    }
                }
                indices[^1] -= 1;
            }
        }
    }
}
