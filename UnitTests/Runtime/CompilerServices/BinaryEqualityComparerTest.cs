using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Diagnostics;

namespace XstarS.Runtime.CompilerServices
{
    [TestClass]
    public class BinaryEqualityComparerTest
    {
        [Ignore]
        [TestMethod]
        public void BenchTest_Int32Equals_TimeLessThanTriple()
        {
            var value = 1;
            var other = 2;
            var testCount = 1000000;
            var time = DiagnosticsMetrics.ExecutionTime(
                () => value.Equals(other), testCount).Ticks;
            var binTime = DiagnosticsMetrics.ExecutionTime(
                () => value.BinaryEquals(other), testCount).Ticks;
            Console.WriteLine($"{time} {binTime}");
            Assert.IsTrue(binTime < time * 3);
        }

        [Ignore]
        [TestMethod]
        public void BenchTest_Int64TripletEquals_TimeLessThanSingle()
        {
            var value = (1L, 2L, 3L);
            var other = (3L, 2L, 1L);
            var testCount = 1000000;
            var time = DiagnosticsMetrics.ExecutionTime(
                () => value.Equals(other), testCount).Ticks;
            var binTime = DiagnosticsMetrics.ExecutionTime(
                () => value.BinaryEquals(other), testCount).Ticks;
            Console.WriteLine($"{time} {binTime}");
            Assert.IsTrue(binTime < time * 1);
        }

        [Ignore]
        [TestMethod]
        public void BenchTest_DoubleHashCode_TimeLessThanDouble()
        {
            var value = 12.34D;
            var testCount = 1000000;
            var time = DiagnosticsMetrics.ExecutionTime(
                () => value.GetHashCode(), testCount).Ticks;
            var binTime = DiagnosticsMetrics.ExecutionTime(
                () => value.GetBinaryHashCode(), testCount).Ticks;
            Console.WriteLine($"{time} {binTime}");
            Assert.IsTrue(binTime < time * 2);
        }

        [Ignore]
        [TestMethod]
        public void BenchTest_BooleanQuadrupletHashCode_TimeLessOneHalf()
        {
            var value = (true, false, false, true);
            var testCount = 1000000;
            var time = DiagnosticsMetrics.ExecutionTime(
                () => value.GetHashCode(), testCount).Ticks;
            var binTime = DiagnosticsMetrics.ExecutionTime(
                () => value.GetBinaryHashCode(), testCount).Ticks;
            Console.WriteLine($"{time} {binTime}");
            Assert.IsTrue(binTime < time / 2);
        }
    }
}
