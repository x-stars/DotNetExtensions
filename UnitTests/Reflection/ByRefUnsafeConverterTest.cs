using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.Reflection
{
    [TestClass]
    public class ByRefUnsafeConverterTest
    {
        [TestMethod]
        public void ToIntPtr_Int32ByRef_CanEditRefValue()
        {
            int value = 0;
            var pointer = ByRefUnsafeConverter.ToIntPtr(ref value);
            ByRefUnsafeConverter.ToByRef<int>(pointer) = 1;
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void RefBoxed_Int32Object_CanEditRefValue()
        {
            var box = (object)0;
            var value = 10;
            ByRefUnsafeConverter.RefBoxed<int>(box) = value;
            Assert.AreEqual(value, box);
        }
    }
}
