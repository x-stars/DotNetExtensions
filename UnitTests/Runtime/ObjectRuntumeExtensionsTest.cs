using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.Runtime
{
    [TestClass]
    public class ObjectRuntumeExtensionsTest
    {
        [TestMethod]
        public void ObjectRecurseClone_Int32Array_WorksProperly()
        {
            var o1 = new int[] { 1 };
            var o2 = (int[])o1.Clone();
            var o3 = o1.ObjectRecurseClone();
            o2[0] = 2;
            o3[0] = 3;
            Assert.AreEqual(1, o1[0]);
            Assert.AreEqual(2, o2[0]);
            Assert.AreEqual(3, o3[0]);
        }

        [TestMethod]
        public void ObjectRecurseClone_NestedInt32Array_WorksProperly()
        {
            var o1 = new int[][] { new int[] { 1 } };
            var o2 = (int[][])o1.Clone();
            var o3 = o1.ObjectRecurseClone();
            o2[0][0] = 2;
            o3[0][0] = 3;
            Assert.AreNotEqual(o1[0][0], 1);
            Assert.AreEqual(2, o1[0][0]);
            Assert.AreEqual(2, o2[0][0]);
            Assert.AreEqual(3, o3[0][0]);
        }

        [TestMethod]
        public void ObjectRecurseClone_Type_WorksProperly()
        {
            var o1 = typeof(object);
            var o2 = typeof(object);
            var o3 = o1.ObjectRecurseClone();
            Assert.IsTrue(object.ReferenceEquals(o1, o2));
            Assert.IsFalse(object.ReferenceEquals(o1, o3));
        }

        [TestMethod]
        public void ObjectRecurseClone_LinkedList_WorkProperly()
        {
            var l = new LinkedList<object>();
            var n0 = l.AddLast("0");
            var n1 = l.AddLast("1");
            var n2 = l.AddLast("2");
            var lc = l.ObjectRecurseClone();
            var n0c = lc.First;
            var n1c = n0c.Next;
            var n2c = n1c.Next;
            Assert.IsFalse(object.ReferenceEquals(l, lc));
            Assert.IsFalse(object.ReferenceEquals(n0, n0c));
            Assert.IsFalse(object.ReferenceEquals(n1, n1c));
            Assert.IsFalse(object.ReferenceEquals(n2, n2c));
            Assert.IsFalse(object.ReferenceEquals(n0.Value, n0c.Value));
            Assert.IsFalse(object.ReferenceEquals(n1.Value, n1c.Value));
            Assert.IsFalse(object.ReferenceEquals(n2.Value, n1c.Value));
            Assert.IsTrue(object.Equals(n0.Value, n0c.Value));
            Assert.IsTrue(object.Equals(n1.Value, n1c.Value));
            Assert.IsTrue(object.Equals(n2.Value, n2c.Value));
        }

        [TestMethod]
        public void ValueEquals_NewObject_ReturnsTrue()
        {
            var o1 = new object();
            var o2 = new object();
            Assert.IsFalse(o1.Equals(o2));
            Assert.IsTrue(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_EqualKeyValuePair_ReturnsTrue()
        {
            var o1 = new KeyValuePair<string, int>("0", 0);
            var o2 = new KeyValuePair<string, int>("0", 0);
            Assert.IsTrue(o1.Equals(o2));
            Assert.IsTrue(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_EqualInt32Array_ReturnsTrue()
        {
            var o1 = new int[100];
            var o2 = new int[100];
            Assert.IsFalse(o1.Equals(o2));
            Assert.IsTrue(o1.SequenceEqual(o2));
            Assert.IsTrue(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_RankLengthDiffEqualInt32Array_ReturnsFalse()
        {
            var o1 = new int[100, 2, 3];
            var o2 = new int[2, 3, 100];
            Assert.IsFalse(o1.Equals(o2));
            Assert.IsTrue(o1.Cast<int>().SequenceEqual(o1.Cast<int>()));
            Assert.IsFalse(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_DiffOrderInt32HashSet_ReturnsFalse()
        {
            var o1 = new HashSet<int>() { 1, 2, 3 };
            var o2 = new HashSet<int>() { 2, 3, 1 };
            Assert.IsFalse(o1.Equals(o2));
            Assert.IsTrue(o1.SetEquals(o2));
            Assert.IsFalse(o1.SequenceEqual(o2));
            Assert.IsFalse(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_DiffOrderStringSortedSet_ReturnsTrue()
        {
            var o1 = new SortedSet<string>() { "list", "set", "dictionary" };
            var o2 = new SortedSet<string>() { "set", "dictionary", "list" };
            Assert.IsFalse(o1.Equals(o2));
            Assert.IsTrue(o1.SetEquals(o2));
            Assert.IsTrue(o1.SequenceEqual(o2));
            Assert.IsTrue(o1.ValueEquals(o2));
        }

        [TestMethod]
        public void ValueEquals_SameOrderObjectLinkedList_ReturnsTrue()
        {
            var l0 = new LinkedList<object>();
            var l0n0 = l0.AddLast(0);
            var l0n1 = l0.AddLast(1.0);
            var l0n2 = l0.AddLast("2");
            var l1 = new LinkedList<object>();
            var l1n0 = l1.AddLast(0);
            var l1n1 = l1.AddLast(1.0);
            var l1n2 = l1.AddLast("2");
            Assert.IsFalse(l0.Equals(l1));
            Assert.IsFalse(l0n0.Equals(l1n0));
            Assert.IsFalse(l0n1.Equals(l1n1));
            Assert.IsFalse(l0n2.Equals(l1n2));
            Assert.IsTrue(l0.ValueEquals(l1));
            Assert.IsTrue(l0n0.ValueEquals(l1n0));
            Assert.IsTrue(l0n1.ValueEquals(l1n1));
            Assert.IsTrue(l0n2.ValueEquals(l1n2));
        }
    }
}
