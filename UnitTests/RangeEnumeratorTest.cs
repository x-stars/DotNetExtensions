using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS
{
#if !DEBUG
    using System;
    using System.Diagnostics;
#endif

    [TestClass]
    public class RangeEnumeratorTest
    {
        [TestMethod]
        public void ForEachLoop_PositiveRange_GetExpectedSum()
        {
            var sum = 0;
            foreach (var index in 0..100)
            {
                sum += index;
            }
            var expected = Enumerable.Range(0, 100).Sum();
            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void ForEachLoop_NegativeRange_GetExpectedSum()
        {
            var sum = 0;
            foreach (var index in ^100..0)
            {
                sum += index;
            }
            var expected = Enumerable.Range(-100, 100).Sum();
            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void ForEachLoop_HybridRange_GetExpectedSum()
        {
            var sum = 0;
            foreach (var index in ^100..100)
            {
                sum += index;
            }
            var expected = Enumerable.Range(-100, 200).Sum();
            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void ForEachLoop_InvalidRange_GetZeroSum()
        {
            var sum = 0;
            foreach (var index in 100..^100)
            {
                sum += index;
            }
            var expected = 0;
            Assert.AreEqual(expected, sum);
        }

        [TestMethod]
        public void BenchTest_CompareWithForLoop_TimeLessThanTwo()
        {
#if DEBUG
            Assert.IsTrue(true);
#else
            const int loopCount = 1000000;
            Console.WriteLine("Loop count: {0}", loopCount);
            var sw = new Stopwatch();
            for (int index = 0; index < loopCount; index++) { }
            sw.Restart();
            for (int index = 0; index < loopCount; index++) { }
            sw.Stop();
            var forLoopTicks = sw.ElapsedTicks;
            Console.WriteLine("Legacy for-loop: {0}", sw.Elapsed);
            sw.Restart();
            foreach (var index in 0..loopCount) { }
            sw.Stop();
            var rangeLoopTicks = sw.ElapsedTicks;
            Console.WriteLine("Range foreach-loop: {0}", sw.Elapsed);
            Assert.IsTrue(rangeLoopTicks < forLoopTicks * 2);
#endif
        }

        [TestMethod]
        public void BenchTest_CompareWithEnumerable_TimeLessThanOneQuater()
        {
#if DEBUG
            Assert.IsTrue(true);
#else
            const int loopCount = 1000000;
            Console.WriteLine("Loop count: {0}", loopCount);
            var sw = new Stopwatch();
            for (int index = 0; index < loopCount; index++) { }
            sw.Restart();
            foreach (var index in Enumerable.Range(0, loopCount)) { }
            sw.Stop();
            var forLoopTicks = sw.ElapsedTicks;
            Console.WriteLine("Enumerable foreach-loop: {0}", sw.Elapsed);
            sw.Restart();
            foreach (var index in 0..loopCount) { }
            sw.Stop();
            var rangeLoopTicks = sw.ElapsedTicks;
            Console.WriteLine("Range foreach-loop: {0}", sw.Elapsed);
            Assert.IsTrue(rangeLoopTicks < forLoopTicks / 4);
#endif
        }
    }
}
