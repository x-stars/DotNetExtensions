using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class ObservableFactoryTest
    {
        [TestMethod]
        public void ObservableType_Singleton_IsThreadSafe()
        {
            int testCount = 10000;
            var factories = new ObservableFactory<object>[testCount];
            Parallel.For(0, testCount,
                index => factories[index] = ObservableFactory<object>.Default);
            Assert.IsTrue(factories.All(
                factory => object.ReferenceEquals(factory, factories[0])));
            Assert.IsTrue(factories.All(
                factory => object.ReferenceEquals(factory.ObservableType, factories[0].ObservableType)));
        }

        [TestMethod]
        public void ObservableType_PublicInheritableType_ReturnsType()
        {
            Assert.IsNotNull(ObservableFactory<IMutableRectangle>.Default.ObservableType);
            Assert.IsNotNull(ObservableFactory<ObservableRectangleBase>.Default.ObservableType);
            Assert.IsNotNull(ObservableFactory<ObservableRectangle>.Default.ObservableType);
            Assert.IsNotNull(ObservableFactory<NonObservableRectangle>.Default.ObservableType);
        }

        [TestMethod]
        public void ObservableType_NonPublicOrNonInheritableType_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => ObservableFactory<IInternalMutableRectangle>.Default.ObservableType);
            Assert.ThrowsException<ArgumentException>(
                () => ObservableFactory<SealedMutableRectangle>.Default.ObservableType);
        }

        [TestMethod]
        public void ObservableType_NoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => ObservableFactory<BadObservableRectangle>.Default.ObservableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndNonSealedClass_ReturnsInstance()
        {
            Assert.IsNotNull(ObservableFactory<IMutableRectangle>.Default.CreateInstance());
            Assert.IsNotNull(ObservableFactory<ObservableRectangleBase>.Default.CreateInstance());
            Assert.IsNotNull(ObservableFactory<ObservableRectangle>.Default.CreateInstance(2, 3));
            Assert.IsNotNull(ObservableFactory<NonObservableRectangle>.Default.CreateInstance(2, 3));
        }

        [TestMethod]
        public void CreateInstance_TypeWithAbstractMethod_ThrowsWhenCallMethod()
        {
            var o1 = ObservableFactory<IMutableRectangle>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o1.Deconstruct(out _, out _));
            var o2 = ObservableFactory<ObservableRectangleBase>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o2.Deconstruct(out _, out _));
        }

        [TestMethod]
        public void PropertyChanged_OverridableProperty_CallsHandler()
        {
            var rectangle = ObservableFactory<ObservableRectangle>.Default.CreateInstance(10, 10);
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(rectangle.Height)] = 0,
                [nameof(rectangle.Width)] = 0,
                [nameof(rectangle.Size)] = 0
            };
            rectangle.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(rectangle.Height * rectangle.Width, rectangle.Size);
            rectangle.Height *= 10; rectangle.Width *= 10;
            Assert.AreEqual(rectangle.Height * rectangle.Width, rectangle.Size);
            Assert.AreEqual(1, changedCounts[nameof(rectangle.Height)]);
            Assert.AreEqual(1, changedCounts[nameof(rectangle.Width)]);
            Assert.AreEqual(2, changedCounts[nameof(rectangle.Size)]);
        }

        [TestMethod]
        public void PropertyChanged_SealedProperty_NotCallsHandler()
        {
            var rectangle = ObservableFactory<NonObservableRectangle>.Default.CreateInstance(10, 10);
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(rectangle.Height)] = 0,
                [nameof(rectangle.Width)] = 0,
                [nameof(rectangle.Size)] = 0
            };
            rectangle.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(rectangle.Height * rectangle.Width, rectangle.Size);
            rectangle.Height *= 10; rectangle.Width *= 10;
            Assert.AreEqual(rectangle.Height * rectangle.Width, rectangle.Size);
            Assert.AreEqual(0, changedCounts[nameof(rectangle.Height)]);
            Assert.AreEqual(0, changedCounts[nameof(rectangle.Width)]);
            Assert.AreEqual(0, changedCounts[nameof(rectangle.Size)]);
        }
    }
}
