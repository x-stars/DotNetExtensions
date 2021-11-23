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

        [TestMethod]
        public void RefBoxed_Int32Object_CanEditRefValue()
        {
            var box = (object)0;
            var value = 10;
            ByRefConverter.RefBoxed<int>(box) = value;
            Assert.AreEqual(value, box);
        }
    }
}
