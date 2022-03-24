using Microsoft.VisualStudio.TestTools.UnitTesting;
using static XstarS.Operators;

namespace XstarS
{
    [TestClass]
    public class OperatorsTest
    {
        [TestMethod]
        public void InitializeArray_FiveLayers_ValueEqualsIndices()
        {
            var length = 10;
            var array = InitArray(length,
                index1 => InitArray(length,
                    index2 => InitArray(length,
                        index3 => InitArray(length,
                            index4 => InitArray(length,
                                index5 => new[] {
                                    index1, index2, index3, index4, index5 })))));
            for (int index = 0; index < 5; index++)
            {
                Assert.AreEqual(index, array[0][1][2][3][4][index]);
            }
        }
    }
}
