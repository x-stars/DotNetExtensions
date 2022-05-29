using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Collections.Generic
{
    [TestClass]
    public class IndexedLinkedListTest
    {
        [TestMethod]
        public void GetItem_IndexBeforeHalf_GetExpectedValue()
        {
            var list = new IndexedLinkedList<int>() { 0, 1, 2, 3, 4 };
            Assert.AreEqual(1, list[1]);
        }

        [TestMethod]
        public void GetItem_IndexAfterHalf_GetExpectedValue()
        {
            var list = new IndexedLinkedList<int>() { 0, 1, 2, 3, 4 };
            Assert.AreEqual(3, list[3]);
        }

        [TestMethod]
        public void IndexOf_ContainsValue_GetFirstIndex()
        {
            var list = new IndexedLinkedList<int>() { 0, 1, 2, 3, 4 };
            Assert.AreEqual(2, list.IndexOf(2));
        }

        [TestMethod]
        public void IndexOf_NotContainsValue_GetMinusOne()
        {
            var list = new IndexedLinkedList<int>() { 0, 1, 2, 1, 0 };
            Assert.AreEqual(-1, list.IndexOf(3));
        }

        [TestMethod]
        public void LastIndexOf_ContainsValue_GetLastIndex()
        {
            var list = new IndexedLinkedList<int>() { 0, 1, 2, 1, 0 };
            Assert.AreEqual(3, list.LastIndexOf(1));
        }

        [TestMethod]
        public void NonGenericIndexOf_NullValue_GetFirstIndex()
        {
            var list = new IndexedLinkedList<object?>() { "", null };
            Assert.AreEqual(1, ((IList)list).IndexOf(null));
        }
    }
}
