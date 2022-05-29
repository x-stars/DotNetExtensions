using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Runtime.CompilerServices
{
    [TestClass]
    public class ObjectRuntimeTypeTest
    {
        [TestMethod]
        public void RefTypeHandle_GetTypeHandle_ValueEquals()
        {
            foreach (var value in new[] { 0, 0.0, "", new object() })
            {
                var expected = value.GetType().TypeHandle.Value;
                Assert.AreEqual(expected, value.RefTypeHandle());
            }
        }

        [TestMethod]
        public void UncheckedCast_CompatibleDelegate_CanInvoke()
        {
            var negFunc = (Func<int, bool>)(num => num < 0);
            var negPredicate = negFunc.UncheckedCast<Predicate<int>>();
            foreach (var number in ^8..8)
            {
                Assert.AreEqual(negFunc(number), negPredicate(number));
            }
        }
    }
}
