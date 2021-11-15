using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.Runtime.CompilerServices
{
    [TestClass]
    public class ByRefConverterTest
    {
        [TestMethod]
        public void ToIntPtr_Int32ByRef_CanEditRefValue()
        {
            int value = 0;
            var reference = ByRefConverter.ToIntPtr(ref value);
            ByRefConverter.FromIntPtr<int>(reference) = 1;
            Assert.AreEqual(1, value);
        }
    }
}
