using System;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace XstarS.Reflection
{
    public class MethodInvokeBenchmark
    {
        [CLSCompliant(false)]
        [Params(1, 10, 100, 1000)]
        public int InvokeCount;

        private readonly object Value = string.Empty;

        private readonly object Other = new string(new char[0]);

        private readonly MethodInfo EqualsMethod =
            typeof(object).GetMethod(nameof(object.Equals), new[] { typeof(object) })!;

        private readonly Func<object, object?, bool> EqualsFunc;

        private readonly Func<object?, object?[]?, object?> EqualsDynFunc;

        public MethodInvokeBenchmark()
        {
            this.EqualsFunc = this.EqualsMethod.CreateDelegate<Func<object, object?, bool>>();
            this.EqualsDynFunc = this.EqualsMethod.CreateDynamicDelegate();
        }

        [Benchmark]
        public void DirectInvoke()
        {
            var value = this.Value;
            var other = this.Other;
            var count = this.InvokeCount;
            foreach (var _ in ..count)
            {
                value.Equals(other);
            }
        }

        [Benchmark]
        public void DelegateInvoke()
        {
            var value = this.Value;
            var other = this.Other;
            var equals = this.EqualsFunc;
            var count = this.InvokeCount;
            foreach (var _ in ..count)
            {
                equals.Invoke(value, other);
            }
        }

        [Benchmark]
        public void ReflectionInvoke()
        {
            var value = this.Value;
            var other = this.Other;
            var equals = this.EqualsMethod;
            var count = this.InvokeCount;
            foreach (var _ in ..count)
            {
                equals.Invoke(value, new[] { other });
            }
        }

        [Benchmark]
        public void ReflectionFastInvoke()
        {
            var value = this.Value;
            var other = this.Other;
            var equals = this.EqualsMethod;
            var count = this.InvokeCount;
            foreach (var _ in ..count)
            {
                equals.InvokeFast(value, new[] { other });
            }
        }

        [Benchmark]
        public void DynamicDelegateInvoke()
        {
            var value = this.Value;
            var other = this.Other;
            var equals = this.EqualsDynFunc;
            var count = this.InvokeCount;
            foreach (var _ in ..count)
            {
                equals.Invoke(value, new[] { other });
            }
        }
    }
}
