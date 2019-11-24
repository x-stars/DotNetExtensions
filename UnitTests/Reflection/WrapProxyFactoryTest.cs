using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Reflection.TestTypes;

namespace XstarS.Reflection
{
    [TestClass]
    public class WrapProxyFactoryTest
    {
        [TestMethod]
        public void CreateInstance_Interface_WorksProperly()
        {
            var o = WrapProxyFactory<IList<int>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance(new List<int>());
            for (int i = 0; i < 10; i++) { o.Add(i); }
            Assert.AreEqual(o.Count, 10);
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithGenericMethod_WorksProperly()
        {
            var o = WrapProxyFactory<ICreator>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance(new Creator());
            Assert.IsNotNull(o.Create<object>());
        }

        [TestMethod]
        public void CreateInstance_DuplicateInterface_WorksProperly()
        {
            var o0 = new FakeClonable();
            var o = WrapProxyFactory<IFakeClonable>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance(o0);
            Assert.AreEqual(o0, o.Clone());
            Assert.AreNotEqual(o0, ((ICloneable)o).Clone());
        }
    }
}
