using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindableFactoryTest
    {
        private readonly Predicate<PropertyInfo> HasBindableAttribute =
            prop => prop.GetCustomAttributes(false).Any(
                attr => (attr is BindableAttribute bAttr) && bAttr.Bindable);

        [TestMethod]
        public void BindableType_Singleton_IsThreadSafe()
        {
            int testCount = 10000;

            var providers = new BindableFactory<DisposableBinding<object>>[testCount];
            Parallel.For(0, testCount,
                index => providers[index] = BindableFactory<DisposableBinding<object>>.Default);
            Assert.IsTrue(providers.All(
                provider => object.ReferenceEquals(provider, providers[0])));
            Assert.IsTrue(providers.All(
                provider => object.ReferenceEquals(provider.BindableType, providers[0].BindableType)));
        }

        [TestMethod]
        public void BindableType_InternalType_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<IInternalDisposableBinding<int>>.Default.BindableType);
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<InternalDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_SealedClass_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<SealedDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NonAbstractEventButNoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindableFactory<BadDisposableBindingBase<object>>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindableFactory<IDisposableNotifyPropertyChanged>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<IDisposableBinding<int>>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<DisposableBindingBase<object>>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<DisposableBinding<int>>.Default.CreateInstance(1, 2));
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithMethod_ThrowsWhenCallMethod()
        {
            var o = BindableFactory<IDisposableBinding<object>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o.Dispose());
        }

        [TestMethod]
        public void CreateInstance_ClassWithAbstractIndexer_ThrowsWhenCallIndexer()
        {
            var o = BindableFactory<IndexedDisposableBindingBase<double>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o[0]);
            Assert.ThrowsException<NotImplementedException>(() => o[0] = 0D);
        }

        [TestMethod]
        public void CreateInstance_ClassWithMethod_WorksProperly()
        {
            object i1 = new object(), i2 = new object(), i3 = new object(), i4 = new object();
            var o = BindableFactory<DisposableBinding<object>>.Custom(
                this.HasBindableAttribute).CreateInstance(i1, i2);
            var l = new CloneableList<object>() { i3, i4 };
            Assert.AreSame(o.Value, i1);
            Assert.AreSame(o.BindableValue, i2);
            o.Load(l);
            Assert.AreSame(o.Value, i3);
            Assert.AreSame(o.BindableValue, i4);
        }

        [TestMethod]
        public void PropertyChanged_Default_CallsHandlerTwice()
        {
            int i = 0;
            var o = BindableFactory<DisposableBindingBase<int>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void PropertyChanged_BindableOnly_CallsHandlerOnce()
        {
            int i = 0;
            var o = BindableFactory<DisposableBindingBase<int>>.Custom(
                this.HasBindableAttribute).CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_NonOverridableProperty_CallsHandlerZeroTimes()
        {
            int i = 0;
            var o = BindableFactory<NonOverridableDisposableBinding<int>>.Default.CreateInstance(0, 0);
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 0);
        }
    }
}
