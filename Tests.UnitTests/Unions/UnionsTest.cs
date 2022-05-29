using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Unions
{
    [TestClass]
    public class UnionsTest
    {
        [TestMethod]
        public unsafe void DWordUnion_SetInt32ValueM1_GetSingleValueNaN()
        {
            var dword = DWordUnion.Zero;
            dword.Int32 = -1;
            Assert.IsTrue(float.IsNaN(dword.Single));
        }

        [TestMethod]
        public unsafe void HandleUnion_FixedBytes_CanGetSettedValue()
        {
            var handle = HandleUnion.Zero;
            foreach (var offset in ..HandleUnion.Size)
            {
                handle.Bytes[offset] = (byte)offset;
                Assert.AreEqual((byte)offset, handle.Bytes[offset]);
            }
        }
    }
}
