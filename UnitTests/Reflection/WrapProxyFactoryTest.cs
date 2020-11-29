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
            for (int index = 0; index < 10; index++) { o.Add(index); }
            Assert.AreEqual(10, o.Count);
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
            var o = WrapProxyFactory<IFakeClonable>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance(new FakeClonable());
            Assert.AreEqual(o.Clone(), ((IWrapProxy)o).GetInstance());
            Assert.AreNotEqual(((IWrapProxy)o).GetInstance(), ((ICloneable)o).Clone());
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithConstraintGenericMethod_WorksProperly()
        {
            var o = WrapProxyFactory<IListCreator<object>>.WithHandler(
                ProxyFactoryTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance(new ListCreator<object>());
            Assert.IsNotNull(o.Create<List<object>>());
        }
    }
}
