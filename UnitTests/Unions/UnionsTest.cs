using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.Unions
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
            for (int offset = 0; offset < HandleUnion.Size; offset++)
            {
                handle.Bytes[offset] = (byte)offset;
                Assert.AreEqual((byte)offset, handle.Bytes[offset]);
            }
        }
    }
}
