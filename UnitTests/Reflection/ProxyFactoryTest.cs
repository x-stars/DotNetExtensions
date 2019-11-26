using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            var o = ProxyFactory<Collection<int>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            for (int i = 0; i < 10; i++) { o.Add(i); }
            Assert.AreEqual(o.Count, 10);
        }

        [TestMethod]
        public void CreateInstance_ClassWithGenericMethod_WorksProperly()
        {
            var o = ProxyFactory<Creator>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            Assert.IsNotNull(o.Create<object>());
            Assert.IsTrue(o.Equals(o));
        }

        [TestMethod]
        public void CreateInstance_AbstractClass_WorksProperly()
        {
            var o = ProxyFactory<EqualityComparer<object>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndReturnDefaultHandler
                ).CreateInstance();
            Assert.AreEqual(o.Equals(0, 0), false);
        }

        [TestMethod]
        public void CreateInstance_Interface_WorksProperly()
        {
            var o = ProxyFactory<IList<object>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndReturnDefaultHandler
                ).CreateInstance();
            Assert.AreEqual(o.Count, 0);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(o[i], null);
            }
        }

        [TestMethod]
        public void CreateInstance_ClassWithConstraintGenericMethod_WorksProperly()
        {
            var o = ProxyFactory<ListCreator<object>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            Assert.IsNotNull(o.Create<List<object>>());
        }
    }
}
