using Microsoft.VisualStudio.TestTools.UnitTesting;
using static XNetEx.Operators;

namespace XNetEx
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
            foreach (var index in ..5)
            {
                Assert.AreEqual(index, array[0][1][2][3][4][index]);
            }
        }
    }
}
