using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindableFactoryTest
    {
        [TestMethod]
        public void BindableType_Singleton_IsThreadSafe()
        {
            int testCount = 10000;

            var providers = new BindableFactory<DisposableBindingValue<object>>[testCount];
            Parallel.For(0, testCount,
                index => providers[index] = BindableFactory<DisposableBindingValue<object>>.Default);
            Assert.IsTrue(providers.All(
                provider => object.ReferenceEquals(provider, providers[0])));
            Assert.IsTrue(providers.All(
                provider => object.ReferenceEquals(provider.BindableType, providers[0].BindableType)));
        }

        [TestMethod]
        public void BindableType_InternalType_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<IInternalDisposableBindingValue<int>>.Default.BindableType);
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<InternalDisposableBindingValue<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_SealedClass_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<SealedDisposableBindingValue<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindableFactory<BadDisposableBindingBase<object>>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindableFactory<IDisposableNotifyPropertyChanged>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<IDisposableBindingValue<int>>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<DisposableBindingValueBase<object>>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<DisposableBindingValue<int>>.Default.CreateInstance(1));
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithMethod_ThrowsWhenCallMethod()
        {
            var o = BindableFactory<IDisposableBindingValue<object>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o.Dispose());
        }

        [TestMethod]
        public void CreateInstance_ClassWithAbstractMethod_ThrowsWhenCallMethod()
        {
            var o = BindableFactory<DisposableBindingValueBase<object>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o.Load(new CloneableList<object>()));
        }

        [TestMethod]
        public void CreateInstance_ClassWithMethod_WorksProperly()
        {
            object i1 = new object(), i2 = new object();
            var o = BindableFactory<DisposableBindingValue<object>>.Default.CreateInstance(i1);
            var l = new CloneableList<object>() { i2 };
            Assert.AreSame(o.Value, i1);
            o.Load(l);
            Assert.AreSame(o.Value, i2);
        }

        [TestMethod]
        public void PropertyChanged_AbstractProperty_CallsHandler()
        {
            int i = 0;
            var o = BindableFactory<DisposableBindingValueBase<int>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void CreateInstance_VirtualIndexProperty_CallsHandler()
        {
            int i = 0;
            var o = BindableFactory<IndexedDisposableBindingValueBase<double>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o[0] = 0D;
            Assert.AreEqual(o[0], 0D);
            Assert.AreEqual(o.Value, 0D);
            o[0] = 1D;
            Assert.AreEqual(o[0], 1D);
            Assert.AreEqual(o.Value, 1D);
            Assert.AreEqual(i, 4);
        }

        [TestMethod]
        public void PropertyChanged_SealedProperty_NotCallsHandler()
        {
            int i = 0;
            var o = BindableFactory<DisposableSealedBindingValue<int>>.Default.CreateInstance(0);
            o.PropertyChanged += (sender, e) => i++;
            o.Value++;
            Assert.AreEqual(i, 0);
        }
    }
}
