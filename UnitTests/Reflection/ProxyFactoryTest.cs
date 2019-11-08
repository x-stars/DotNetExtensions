using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Reflection.TestTypes;

namespace XstarS.Reflection
{
    [TestClass]
    public class ProxyFactoryTest
    {
        [TestMethod]
        public void CreateInstance_Class_WorksProperly()
        {
            var o = ProxyFactory<ProxyCollection<int>>.WithHandler(
                TestHandlers.WriteMethodAndInvokeBaseHandler).CreateInstance();
            for (int i = 0; i < 10; i++) { o.Add(i); }
            Assert.AreEqual(o.Count, 10);
        }

        [TestMethod]
        public void CreateInstance_ClassWithGenericMethod_WorksProperly()
        {
            var o = ProxyFactory<ProxyCreator>.WithHandler(
                TestHandlers.WriteMethodAndInvokeBaseHandler).CreateInstance();
            Assert.IsNotNull(o.Create<object>());
        }

        [TestMethod]
        public void CreateInstance_AbstractClass_WorksProperly()
        {
            var o = ProxyFactory<ProxyEqualityComparer<object>>.WithHandler(
                TestHandlers.WriteMethodAndReturnDefaultHandler).CreateInstance();
            Assert.AreEqual(o.Equals(0, 0), false);
        }

        [TestMethod]
        public void CreateInstance_Interface_WorksProperly()
        {
            var o = ProxyFactory<IProxyList<object>>.WithHandler(
                TestHandlers.WriteMethodAndReturnDefaultHandler).CreateInstance();
            Assert.AreEqual(o.Count, 0);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(o[i], null);
            }
        }
    }
}
