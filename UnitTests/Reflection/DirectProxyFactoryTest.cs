using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.Reflection.TestTypes;

namespace XstarS.Reflection
{
    [TestClass]
    public class DirectProxyFactoryTest
    {
        [TestMethod]
        public void CreateInstance_Class_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var proxy = new DirectProxyFactory<Collection<int>>(handler).CreateInstance();
            foreach (var index in ..10) { proxy.Add(index); }
            Assert.AreEqual(10, proxy.Count);
        }

        [TestMethod]
        public void CreateInstance_ClassWithGenericMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var proxy = new DirectProxyFactory<Creator>(handler).CreateInstance();
            Assert.IsNotNull(proxy.Create<object>());
            Assert.IsTrue(proxy.Equals(proxy));
        }

        [TestMethod]
        public void CreateInstance_AbstractClass_GetDefaultResult()
        {
            var handler = ProxyTestHandlers.WriteMethodAndReturnDefaultHandler;
            var proxy = new DirectProxyFactory<EqualityComparer<object>>(handler).CreateInstance();
            Assert.AreEqual(false, proxy.Equals(0, 0));
        }

        [TestMethod]
        public void CreateInstance_Interface_GetDefaultResult()
        {
            var handler = ProxyTestHandlers.WriteMethodAndReturnDefaultHandler;
            var proxy = new DirectProxyFactory<IList<object>>(handler).CreateInstance();
            Assert.AreEqual(0, proxy.Count);
            foreach (var index in ..10)
            {
                Assert.AreEqual(null, proxy[index]);
            }
        }

        [TestMethod]
        public void CreateInstance_ClassWithConstraintGenericMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var proxy = new DirectProxyFactory<ListCreator<object>>(handler).CreateInstance();
            Assert.IsNotNull(proxy.Create<List<object>>());
        }

        [TestMethod]
        public void CreateInstance_ClassWithByRefParameterMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var proxy = new DirectProxyFactory<Int32Increaser>(handler).CreateInstance();
            var value = 0;
            proxy.Increase(ref value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void CreateInstance_ClassWithByRefReturnMethod_HasSameBehavior()
        {
            var handler = ProxyTestHandlers.WriteMethodAndInvokeBaseHandler;
            var proxy = new DirectProxyFactory<ByRefValueBox<int>>(handler).CreateInstance();
            Assert.AreEqual(0, proxy.Value);
            ref var value = ref proxy.RefValue;
            value = 1;
            Assert.AreEqual(1, proxy.Value);
        }
    }
}
