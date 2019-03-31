using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Reflection.TestTypes;

namespace XstarS.Reflection
{
    [TestClass]
    public class ProxyBuilderTest
    {
        [TestMethod]
        public void CreateInstance_SimpleClass_WorksProperly()
        {
            var o = ProxyBuilder<ProxyBinding<int>>.Default.CreateInstance(0);
            int i = 0;
            o.PropertyChanged += (sender, e) => i++;
            for (int j = 0; j < 1000; j++) { o.Value++; }
            Assert.AreEqual(i, 1000);
            Assert.AreEqual(o.Value, 1000);
            Assert.AreEqual(100, o.Function(100));
        }

        [TestMethod]
        public void CreateInstance_ComplexClass_WorksProperly()
        {
            var o = ProxyBuilder<ProxyCollection<int>>.Default.CreateInstance();
            for (int j = 0; j < 1000; j++) { o.Add(j); }
            Assert.AreEqual(o.Count, 1000);
        }

        [TestMethod]
        public void CreateInstance_AbstractClass_WorksProperly()
        {
            var o = ProxyBuilder<ProxyEqualityComparer<object>>.Default.CreateInstance();
            Assert.IsFalse(o.Equals(0, 0));
        }

        [TestMethod]
        public void CreateInstance_Interface_WorksProperly()
        {
            var o = ProxyBuilder<IProxyList<object>>.Default.CreateInstance();
            Assert.AreEqual(o.Count, 0);
        }
    }
}
