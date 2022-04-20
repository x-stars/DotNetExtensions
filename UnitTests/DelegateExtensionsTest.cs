using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS
{
    [TestClass]
    public class DelegateExtensionsTest
    {
        [TestMethod]
        public void DynamicInvokeFast_ParamsAndReturns_GetSameResult()
        {
            var input = 1;
            var nextIntFunc = (Func<int, int>)(num => num + 1);
            var excepted = nextIntFunc.DynamicInvoke(input);
            var result = nextIntFunc.DynamicInvokeFast(input);
            Assert.AreEqual(excepted, result);
        }

        [TestMethod]
        public void CreateDynamicDelegate_ParamsAndReturns_GetSameResult()
        {
            var input = 1;
            var nextIntFunc = (Func<int, int>)(num => num + 1);
            var excepted = nextIntFunc.DynamicInvoke(input);
            var dynDelegate = nextIntFunc.ToDynamicDelegate();
            var result = dynDelegate.Invoke(new object[] { input });
            Assert.AreEqual(excepted, result);
        }
    }
}
