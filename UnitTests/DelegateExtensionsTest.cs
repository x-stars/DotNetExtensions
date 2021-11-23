using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Diagnostics;

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
        public void DynamicInvokeFast_NoParamsNoReturns_FasterThanDefault()
        {
            var noAction = (Action)(() => { });
            noAction.DynamicInvoke();
            noAction.DynamicInvokeFast();
            var testCount = 1_000_000;
            var time = DiagnosticsHelper.ExecutionTime(
                () => noAction.DynamicInvoke(), testCount);
            var fastTime = DiagnosticsHelper.ExecutionTime(
                () => noAction.DynamicInvokeFast(), testCount);
            Assert.IsTrue(time > fastTime);
        }

        [TestMethod]
        public void DynamicInvokeFast_ParamsAndReturns_FasterThanDefault()
        {
            var input = new object();
            var selfFunc = (Func<object, object>)(value => value);
            selfFunc.DynamicInvoke(input);
            selfFunc.DynamicInvokeFast(input);
            var testCount = 1_000_000;
            var time = DiagnosticsHelper.ExecutionTime(
                () => selfFunc.DynamicInvoke(input), testCount);
            var fastTime = DiagnosticsHelper.ExecutionTime(
                () => selfFunc.DynamicInvokeFast(input), testCount);
            Assert.IsTrue(time > fastTime);
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
