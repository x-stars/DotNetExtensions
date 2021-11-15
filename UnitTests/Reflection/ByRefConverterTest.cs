using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.Reflection
{
    [TestClass]
    public class ByRefConverterTest
    {
        [TestMethod]
        public void ToIntPtr_Int32ByRef_CanEditRefValue()
        {
            int value = 0;
            var pointer = ByRefConverter.ToIntPtr(ref value);
            ByRefConverter.ToByRef<int>(pointer) = 1;
            Assert.AreEqual(1, value);
        }
    }
}
