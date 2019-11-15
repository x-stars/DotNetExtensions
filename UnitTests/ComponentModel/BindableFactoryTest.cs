using System;
using System.Collections.Generic;
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
            var factories = new BindableFactory<object>[testCount];
            Parallel.For(0, testCount,
                index => factories[index] = BindableFactory<object>.Default);
            Assert.IsTrue(factories.All(
                factory => object.ReferenceEquals(factory, factories[0])));
            Assert.IsTrue(factories.All(
                factory => object.ReferenceEquals(factory.BindableType, factories[0].BindableType)));
        }

        [TestMethod]
        public void BindableType_PublicInheritableType_ReturnsType()
        {
            Assert.IsNotNull(BindableFactory<IMutableRectangle>.Default.BindableType);
            Assert.IsNotNull(BindableFactory<BindableRectangleBase>.Default.BindableType);
            Assert.IsNotNull(BindableFactory<BindableRectangle>.Default.BindableType);
            Assert.IsNotNull(BindableFactory<NonBindableRectangle>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NonPublicOrNonInheritableType_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<IInternalMutableRectangle>.Default.BindableType);
            Assert.ThrowsException<ArgumentException>(
                () => BindableFactory<SealedMutableRectangle>.Default.BindableType);
        }

        [TestMethod]
        public void BindableType_NoOnPropertyChangedMethod_ThrowsException()
        {
            Assert.ThrowsException<MissingMethodException>(
                () => BindableFactory<BadBindableRectangle>.Default.BindableType);
        }

        [TestMethod]
        public void CreateInstance_InterfaceAndNonSealedClass_ReturnsInstance()
        {
            Assert.IsNotNull(BindableFactory<IMutableRectangle>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<BindableRectangleBase>.Default.CreateInstance());
            Assert.IsNotNull(BindableFactory<BindableRectangle>.Default.CreateInstance(2, 3));
            Assert.IsNotNull(BindableFactory<NonBindableRectangle>.Default.CreateInstance(2, 3));
        }

        [TestMethod]
        public void CreateInstance_TypeWithAbstractMethod_ThrowsWhenCallMethod()
        {
            var o1 = BindableFactory<IMutableRectangle>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o1.Deconstruct(out _, out _));
            var o2 = BindableFactory<BindableRectangleBase>.Default.CreateInstance();
            Assert.ThrowsException<NotImplementedException>(() => o2.Deconstruct(out _, out _));
        }

        [TestMethod]
        public void PropertyChanged_OverridableProperty_CallsHandler()
        {
            var rectangle = BindableFactory<BindableRectangle>.Default.CreateInstance(10, 10);
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(rectangle.Height)] = 0,
                [nameof(rectangle.Width)] = 0,
                [nameof(rectangle.Size)] = 0
            };
            rectangle.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(rectangle.Size, rectangle.Height * rectangle.Width);
            rectangle.Height *= 10; rectangle.Width *= 10;
            Assert.AreEqual(rectangle.Size, rectangle.Height * rectangle.Width);
            Assert.AreEqual(changedCounts[nameof(rectangle.Height)], 1);
            Assert.AreEqual(changedCounts[nameof(rectangle.Width)], 1);
            Assert.AreEqual(changedCounts[nameof(rectangle.Size)], 2);
        }

        [TestMethod]
        public void PropertyChanged_SealedProperty_NotCallsHandler()
        {
            var rectangle = BindableFactory<NonBindableRectangle>.Default.CreateInstance(10, 10);
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(rectangle.Height)] = 0,
                [nameof(rectangle.Width)] = 0,
                [nameof(rectangle.Size)] = 0
            };
            rectangle.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(rectangle.Size, rectangle.Height * rectangle.Width);
            rectangle.Height *= 10; rectangle.Width *= 10;
            Assert.AreEqual(rectangle.Size, rectangle.Height * rectangle.Width);
            Assert.AreEqual(changedCounts[nameof(rectangle.Height)], 0);
            Assert.AreEqual(changedCounts[nameof(rectangle.Width)], 0);
            Assert.AreEqual(changedCounts[nameof(rectangle.Size)], 0);
        }
    }
}
