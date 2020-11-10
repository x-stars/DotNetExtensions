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
            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(i, array[0][1][2][3][4][i]);
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
        public void ReshapeAsJagged_Rank1Array_GetsRank5JaggedArray()
        {
            var array = new[] { 10000, 1000, 100, 10, 1, 0 };
            var reshaped = (int[][][][][])array.ReshapeAsJagged(1, 3, 2, 1, 1);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Assert.AreEqual(array[i * 2 + j], reshaped[0][i][j][0][0]);
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
