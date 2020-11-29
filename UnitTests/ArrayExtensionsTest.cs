using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS
{
    [TestClass]
    public class ArrayExtensionsTest
    {
        [TestMethod]
        public void InitializeArray_FiveLayers_ValueEqualsIndices()
        {
            var array = 10.InitializeArray(
                i1 => 10.InitializeArray(
                    i2 => 10.InitializeArray(
                        i3 => 10.InitializeArray(
                            i4 => 10.InitializeArray(
                                i5 => new[] { i1, i2, i3, i4, i5 })))));
            for (int index = 0; index < 5; index++)
            {
                Assert.AreEqual(index, array[0][1][2][3][4][index]);
            }
        }

        [TestMethod]
        public void Reshape_Rank5Array_GetsRank2Array()
        {
            var array = new int[10, 10, 10, 10, 10];
            var reshaped = (int[,])array.Reshape(100, 1000);
            Assert.AreEqual(100000, reshaped.Length);
            Assert.AreEqual(2, reshaped.Rank);
            Assert.AreEqual(100, reshaped.GetLength(0));
            Assert.AreEqual(1000, reshaped.GetLength(1));
        }

        [TestMethod]
        public void Reshape_Rank5JaggedArray_GetsRank3Array()
        {
            var array = new[] { new[] { new[] { new[] { new[] { 100 } } } } };
            var reshaped = (int[,,])array.Reshape(1, 1, 1);
            Assert.AreEqual(array[0][0][0][0][0], reshaped[0, 0, 0]);
            Assert.AreEqual(1, reshaped.Length);
            Assert.AreEqual(3, reshaped.Rank);
        }

        [TestMethod]
        public void ReshapeJagged_Rank1Array_GetsRank5JaggedArray()
        {
            var array = new[] { 10000, 1000, 100, 10, 1, 0 };
            var reshaped = (int[][][][][])array.ReshapeJagged(1, 3, 2, 1, 1);
            for (int index1 = 0; index1 < 3; index1++)
            {
                for (int index2 = 0; index2 < 2; index2++)
                {
                    Assert.AreEqual(array[index1 * 2 + index2], reshaped[0][index1][index2][0][0]);
                }
            }
            Assert.AreEqual(1, reshaped.Length);
            Assert.AreEqual(3, reshaped[0].Length);
            Assert.AreEqual(2, reshaped[0][0].Length);
            Assert.AreEqual(1, reshaped[0][0][0].Length);
            Assert.AreEqual(1, reshaped[0][0][0][0].Length);
            Assert.AreEqual(1, reshaped.Rank);
        }
    }
}
