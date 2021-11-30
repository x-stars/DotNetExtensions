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
        public void CreateInstance_Interface_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var instance = new List<int>();
            var proxy = new WrapProxyFactory<IList<int>>(handler).CreateInstance(instance);
            for (int index = 0; index < 10; index++) { proxy.Add(index); }
            Assert.AreEqual(10, proxy.Count);
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithGenericMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var instance = new Creator();
            var proxy = new WrapProxyFactory<ICreator>(handler).CreateInstance(instance);
            Assert.IsNotNull(proxy.Create<object>());
        }

        [TestMethod]
        public void CreateInstance_DuplicateInterface_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var instance = new FakeClonable();
            var proxy = new WrapProxyFactory<IFakeClonable>(handler).CreateInstance(instance);
            Assert.AreSame(instance, proxy.Clone());
            Assert.AreNotSame(instance, ((ICloneable)proxy).Clone());
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithConstraintGenericMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var instance = new ListCreator<object>();
            var proxy = new WrapProxyFactory<IListCreator<object>>(handler).CreateInstance(instance);
            Assert.IsNotNull(proxy.Create<List<object>>());
        }
    }
}
