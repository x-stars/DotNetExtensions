using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindingBuilderTest
    {
        [TestMethod]
        public void BindableType_InternalType_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindingBuilder<IInternalDisposableBinding<int>>.Default.BindableType);
            Assert.ThrowsException<ArgumentException>(
                () => BindingBuilder<InternalDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_SealedClass_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindingBuilder<SealedDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NonAbstractEventButNoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindingBuilder<BadDisposableBindingBase<object>>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindingBuilder<IDisposableNotifyPropertyChanged>.Default.CreateInstance());
            Assert.IsNotNull(BindingBuilder<IDisposableBinding<int>>.Default.CreateInstance());
            Assert.IsNotNull(BindingBuilder<DisposableBindingBase<object>>.Default.CreateInstance());
            Assert.IsNotNull(BindingBuilder<DisposableBinding<int>>.Default.CreateInstance(1, 2));
        }

        [TestMethod]
        public void PropertyChanged_Default_CallsHandlerTwice()
        {
            int i = 0;
            var o = BindingBuilder<DisposableBindingBase<int>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void PropertyChanged_BindableOnly_CallsHandlerOnce()
        {
            int i = 0;
            var o = BindingBuilder<DisposableBindingBase<int>>.Bindable.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_NonOverridableProperty_CallsHandlerZeroTimes()
        {
            int i = 0;
            var o = BindingBuilder<NonOverridableDisposableBinding<int>>.Default.CreateInstance(0, 0);
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 0);
        }
    }
}
