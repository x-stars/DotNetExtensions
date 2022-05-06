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
            var array = MakeArray(length,
                index1 => MakeArray(length,
                    index2 => MakeArray(length,
                        index3 => MakeArray(length,
                            index4 => MakeArray(length,
                                index5 => new[] {
                                    index1, index2, index3, index4, index5 })))));
            for (int index = 0; index < 5; index++)
            {
                Assert.AreEqual(index, array[0][1][2][3][4][index]);
            }
        }
    }
}
