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
        public void CreateInstance_Class_WorksProperly()
        {
            var o = DirectProxyFactory<Collection<int>>.WithHandler(
                ProxyTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            for (int index = 0; index < 10; index++) { o.Add(index); }
            Assert.AreEqual(10, o.Count);
        }

        [TestMethod]
        public void CreateInstance_ClassWithGenericMethod_WorksProperly()
        {
            var o = DirectProxyFactory<Creator>.WithHandler(
                ProxyTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            Assert.IsNotNull(o.Create<object>());
            Assert.IsTrue(o.Equals(o));
        }

        [TestMethod]
        public void CreateInstance_AbstractClass_WorksProperly()
        {
            var o = DirectProxyFactory<EqualityComparer<object>>.WithHandler(
                ProxyTestHandlers.WriteMethodAndReturnDefaultHandler
                ).CreateInstance();
            Assert.AreEqual(false, o.Equals(0, 0));
        }

        [TestMethod]
        public void CreateInstance_Interface_WorksProperly()
        {
            var o = DirectProxyFactory<IList<object>>.WithHandler(
                ProxyTestHandlers.WriteMethodAndReturnDefaultHandler
                ).CreateInstance();
            Assert.AreEqual(0, o.Count);
            for (int index = 0; index < 10; index++)
            {
                Assert.AreEqual(null, o[index]);
            }
        }

        [TestMethod]
        public void CreateInstance_ClassWithConstraintGenericMethod_WorksProperly()
        {
            var o = DirectProxyFactory<ListCreator<object>>.WithHandler(
                ProxyTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            Assert.IsNotNull(o.Create<List<object>>());
        }

        [TestMethod]
        public void CreateInstance_ClassWithByRefParameterMethod_WorksProperly()
        {
            var o = DirectProxyFactory<Int32Increaser>.WithHandler(
                ProxyTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            var value = 0;
            o.Increase(ref value);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void CreateInstance_ClassWithByRefReturnMethod_WorksProperly()
        {
            var o = DirectProxyFactory<ByRefValueBox<int>>.WithHandler(
                ProxyTestHandlers.WriteMethodAndInvokeBaseHandler
                ).CreateInstance();
            Assert.AreEqual(0, o.Value);
            ref var value = ref o.RefValue;
            value = 1;
            Assert.AreEqual(1, o.Value);
        }
    }
}
