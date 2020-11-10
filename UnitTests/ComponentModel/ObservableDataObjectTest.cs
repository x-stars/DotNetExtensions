using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class ObservableDataObjectTest
    {
        [TestMethod]
        public void SetProperty_ObservableDataObject_CallsHandler()
        {
            var personName = new ObservablePersonName()
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
            Assert.AreEqual($"{personName.GivenName} {personName.FamilyName}", personName.FullName);
            personName.IsEasternStyle = true;
            Assert.AreEqual($"{personName.FamilyName}{personName.GivenName}", personName.FullName);
            Assert.AreEqual(0, changedCounts[nameof(personName.FamilyName)]);
            Assert.AreEqual(0, changedCounts[nameof(personName.GivenName)]);
            Assert.AreEqual(1, changedCounts[nameof(personName.IsEasternStyle)]);
            Assert.AreEqual(1, changedCounts[nameof(personName.FullName)]);
        }

        [TestMethod]
        public void SetProperty_ObservableValidDataObject_CallsHandler()
        {
            var box = new ObservableBox() { Length = 10, Width = 10, Height = 10 };
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(box.Length)] = 0,
                [nameof(box.Width)] = 0,
                [nameof(box.Height)] = 0,
                [nameof(box.Size)] = 0,
                [nameof(box.HasErrors)] = 0
            };
            box.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(box.Length * box.Width * box.Height, box.Size);
            box.Length *= 10; box.Width *= 10; box.Height *= 10;
            Assert.AreEqual(box.Length * box.Width * box.Height, box.Size);
            Assert.AreEqual(1, changedCounts[nameof(box.Length)]);
            Assert.AreEqual(1, changedCounts[nameof(box.Width)]);
            Assert.AreEqual(1, changedCounts[nameof(box.Height)]);
            Assert.AreEqual(3, changedCounts[nameof(box.Size)]);
            Assert.IsFalse(box.HasErrors);
            var errorsCounts = new Dictionary<string, int>()
            {
                [nameof(box.Length)] = 0,
                [nameof(box.Width)] = 0,
                [nameof(box.Height)] = 0
            };
            box.ErrorsChanged += (sender, e) => errorsCounts[e.PropertyName]++;
            box.Length *= -1; box.Width *= -1; box.Height *= -1;
            Assert.AreEqual(1, errorsCounts[nameof(box.Length)]);
            Assert.AreEqual(1, errorsCounts[nameof(box.Width)]);
            Assert.AreEqual(1, errorsCounts[nameof(box.Height)]);
            Assert.IsTrue(box.HasErrors);
            Assert.IsNotNull(box.GetErrors(nameof(box.Length)));
            Assert.IsNotNull(box.GetErrors(nameof(box.Width)));
            Assert.IsNotNull(box.GetErrors(nameof(box.Height)));
        }
    }
}
