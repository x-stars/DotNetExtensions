using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindableBaseTest
    {
        [TestMethod]
        public void PropertyChanged_BindableObject_CallsHandler()
        {
            var rectangle = new BindableRectangle() { Length = 10, Width = 10 };
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(rectangle.Length)] = 0,
                [nameof(rectangle.Width)] = 0,
                [nameof(rectangle.Size)] = 0
            };
            rectangle.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(rectangle.Size, rectangle.Length * rectangle.Width);
            rectangle.Length *= 10;
            rectangle.Width *= 10;
            Assert.AreEqual(rectangle.Size, rectangle.Length * rectangle.Width);
            Assert.AreEqual(changedCounts[nameof(rectangle.Length)], 1);
            Assert.AreEqual(changedCounts[nameof(rectangle.Width)], 1);
            Assert.AreEqual(changedCounts[nameof(rectangle.Size)], 2);
        }

        [TestMethod]
        public void PropertyChanged_BindableStorage_CallsHandler()
        {
            var personName = new BindablePersonName()
            {
                FamilyName = "FamilyName",
                GivenName = "GivenName",
                IsEasternStyle = false
            };
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(personName.FamilyName)] = 0,
                [nameof(personName.GivenName)] = 0,
                [nameof(personName.IsEasternStyle)] = 0,
                [nameof(personName.FullName)] = 0
            };
            personName.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(personName.FullName, $"{personName.GivenName} {personName.FamilyName}");
            personName.IsEasternStyle = true;
            Assert.AreEqual(personName.FullName, $"{personName.FamilyName}{personName.GivenName}");
            Assert.AreEqual(changedCounts[nameof(personName.FamilyName)], 0);
            Assert.AreEqual(changedCounts[nameof(personName.GivenName)], 0);
            Assert.AreEqual(changedCounts[nameof(personName.IsEasternStyle)], 1);
            Assert.AreEqual(changedCounts[nameof(personName.FullName)], 1);
        }
    }
}
