using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindableTypeProviderTest
    {
        private readonly Predicate<PropertyInfo> HasBindableAttribute =
            prop => prop.GetCustomAttributes(false).Any(
                attr => (attr is BindableAttribute bAttr) && bAttr.Bindable);

        [TestMethod]
        public void BindableType_Singleton_IsThreadSafe()
        {
            int testCount = 10000;

            var builders = new BindableTypeProvider<DisposableBinding<object>>[testCount];
            var builderTasks = new Task[testCount];
            var builderHashCodes = new int[testCount];
            Parallel.For(0, testCount, delegate (int index)
            {
                var builder = BindableTypeProvider<DisposableBinding<object>>.Default;
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
                () => BindableTypeProvider<IInternalDisposableBinding<int>>.Default.BindableType);
            Assert.ThrowsException<TypeAccessException>(
                () => BindableTypeProvider<InternalDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_SealedClass_ThrowsException()
        {
            Assert.ThrowsException<TypeAccessException>(
                () => BindableTypeProvider<SealedDisposableBinding<string>>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NonAbstractEventButNoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindableTypeProvider<BadDisposableBindingBase<object>>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindableTypeProvider<IDisposableNotifyPropertyChanged>.Default.CreateInstance());
            Assert.IsNotNull(BindableTypeProvider<IDisposableBinding<int>>.Default.CreateInstance());
            Assert.IsNotNull(BindableTypeProvider<DisposableBindingBase<object>>.Default.CreateInstance());
            Assert.IsNotNull(BindableTypeProvider<DisposableBinding<int>>.Default.CreateInstance(1, 2));
        }

        [TestMethod]
        public void CreateInstance_InterfaceWithMethod_ThrowsWhenCallMethod()
        {
            var o = BindableTypeProvider<IDisposableBinding<object>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o.Dispose());
        }

        [TestMethod]
        public void CreateInstance_ClassWithAbstractIndexer_ThrowsWhenCallIndexer()
        {
            var o = BindableTypeProvider<IndexedDisposableBindingBase<double>>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o[0]);
            Assert.ThrowsException<NotImplementedException>(() => o[0] = 0D);
        }

        [TestMethod]
        public void CreateInstance_ClassWithMethod_WorksProperly()
        {
            object i1 = new object(), i2 = new object(), i3 = new object(), i4 = new object();
            var o = BindableTypeProvider<DisposableBinding<object>>.Custom(
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
            var o = BindableTypeProvider<DisposableBindingBase<int>>.Default.CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 2);
        }

        [TestMethod]
        public void PropertyChanged_BindableOnly_CallsHandlerOnce()
        {
            int i = 0;
            var o = BindableTypeProvider<DisposableBindingBase<int>>.Custom(
                this.HasBindableAttribute).CreateInstance();
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_NonOverridableProperty_CallsHandlerZeroTimes()
        {
            int i = 0;
            var o = BindableTypeProvider<NonOverridableDisposableBinding<int>>.Default.CreateInstance(0, 0);
            o.PropertyChanged += (sender, e) => i++;
            o.Value++; o.BindableValue++;
            Assert.AreEqual(i, 0);
        }
    }
}
