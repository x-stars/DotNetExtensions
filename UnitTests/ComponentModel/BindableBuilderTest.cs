using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindableBuilderTest
    {
        [TestMethod]
        public void BindableType_Singleton_IsThreadSafe()
        {
            int testCount = 10000;

            var builders = new BindableBuilder<DisposableBinding<object>>[testCount];
            var builderTasks = new Task[testCount];
            var builderHashCodes = new int[testCount];
            Parallel.For(0, testCount, delegate (int index)
            {
                var builder = BindableBuilder<DisposableBinding<object>>.Default;
                builders[index] = builder;
                builderHashCodes[index] = RuntimeHelpers.GetHashCode(builder);
            });
            Assert.IsTrue(builderHashCodes.All(hashCode => hashCode == builderHashCodes[0]));

            var types = new Type[testCount];
            var typeTasks = new Task[testCount];
            var typeHashCodes = new int[testCount];
            Parallel.For(0, testCount, delegate (int index)
            {
                var type = builders[index].BindableType;
                types[index] = type;
                typeHashCodes[index] = RuntimeHelpers.GetHashCode(type);
            });
            Assert.IsTrue(typeHashCodes.All(hashCode => hashCode == typeHashCodes[0]));
        }

        [TestMethod]
        public void BindableType_InternalType_ThrowsException()
        {
            Assert.ThrowsException<TypeAccessException>(
                () => BindableBuilder<IInternalDisposableBinding<int>>.Default.BindableType);
            Assert.ThrowsException<TypeAccessException>(
                () => BindableBuilder<InternalDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_SealedClass_ThrowsException()
        {
            Assert.ThrowsException<TypeAccessException>(
                () => BindableBuilder<SealedDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NonAbstractEventButNoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindableBuilder<BadDisposableBindingBase<object>>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindableBuilder<IDisposableNotifyPropertyChanged>.Default.CreateInstance());
            Assert.IsNotNull(BindableBuilder<IDisposableBinding<int>>.Default.CreateInstance());
            Assert.IsNotNull(BindableBuilder<DisposableBindingBase<object>>.Default.CreateInstance());
            Assert.IsNotNull(BindableBuilder<DisposableBinding<int>>.Default.CreateInstance(1, 2));
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithMethod_ThrowsWhenCallMethod()
        {
            var o = BindableBuilder<IDisposableBinding<object>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o.Dispose());
        }

        [TestMethod]
        public void CreateInstance_ClassWithAbstractIndexer_ThrowsWhenCallIndexer()
        {
            var o = BindableBuilder<IndexedDisposableBindingBase<double>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o[0]);
            Assert.ThrowsException<NotImplementedException>(() => o[0] = 0D);
        }

        [TestMethod]
        public void CreateInstance_ClassWithMethod_WorksProperly()
        {
            object i1 = new object(), i2 = new object(), i3 = new object(), i4 = new object();
            var o = BindableBuilder<DisposableBinding<object>>.BindableOnly.CreateInstance(i1, i2);
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
            var o = BindableBuilder<DisposableBindingBase<int>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void PropertyChanged_BindableOnly_CallsHandlerOnce()
        {
            int i = 0;
            var o = BindableBuilder<DisposableBindingBase<int>>.BindableOnly.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_NonOverridableProperty_CallsHandlerZeroTimes()
        {
            int i = 0;
            var o = BindableBuilder<NonOverridableDisposableBinding<int>>.Default.CreateInstance(0, 0);
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 0);
        }
    }
}
