using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using XstarS.Unions;

namespace XstarS.Runtime.CompilerServices
{
    public class ValueTypeEqualsBenchmark
    {
        [CLSCompliant(false)]
        [Params(1, 10, 100, 1000)]
        public int CompareCount;

        private readonly nint Value = 42;
        private readonly nint Other = 24;
        private readonly object BoxedValue = (nint)42;
        private readonly object BoxedOther = (nint)24;
        private readonly HandleUnion WrappedValue = (nint)42;
        private readonly HandleUnion WrappedOther = (nint)24;

        [Benchmark(Baseline = true)]
        public void PrimitiveCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value == other;
            }
        }

        [Benchmark]
        public void MethodCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.Equals(other);
            }
        }

        [Benchmark]
        public void ComparerCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var comparer = EqualityComparer<nint>.Default;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = comparer.Equals(value, other);
            }
        }

        [Benchmark]
        public void RuntimeCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.RuntimeEquals(other);
            }
        }

        [Benchmark]
        public void BoxedValueCompare()
        {
            var value = this.BoxedValue;
            var other = this.BoxedOther;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.Equals(other);
            }
        }

        [Benchmark]
        public void WrappedValueCompare()
        {
            var value = this.WrappedValue;
            var other = this.WrappedOther;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.Equals(other);
            }
        }

        [Benchmark]
        public void BinaryCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.BinaryEquals(other);
            }
        }

        [Benchmark]
        public void RecursiveCompare()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.RecursiveEquals(other);
            }
        }
    }
}
