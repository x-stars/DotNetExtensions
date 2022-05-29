using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Runtime.CompilerServices
{
    public class ValueTypeEqualsBenchmark
    {
        [CLSCompliant(false)]
        [Params(1, 10, 100, 1000)]
        public int CompareCount;

        private readonly struct ValueBox<T> where T : struct
        {
            public readonly T Value;
            public ValueBox(T value) { this.Value = value; }
        }

        private readonly nint Value = 42;
        private readonly nint Other = 24;
        private readonly ValueBox<nint> WrappedValue = new(42);
        private readonly ValueBox<nint> WrappedOther = new(24);

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
        public void ObjectCompare()
        {
            var value = (object)this.Value;
            var other = (object)this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.Equals(other);
            }
        }

        [Benchmark]
        public void ValueTypeCompare()
        {
            var value = (object)this.WrappedValue;
            var other = (object)this.WrappedOther;
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
        public void RuntimeCompare()
        {
            var value = (object)this.Value;
            var other = (object)this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.RuntimeEquals(other);
            }
        }

        [Benchmark]
        public void RecursiveCompare()
        {
            var value = (object)this.Value;
            var other = (object)this.Other;
            var count = this.CompareCount;
            for (int index = 0; index < count; index++)
            {
                var result = value.RecursiveEquals(other);
            }
        }
    }
}
