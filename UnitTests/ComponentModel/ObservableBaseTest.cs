using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class ObservableBaseTest
    {
        [TestMethod]
        public void PropertyChanged_ObservableObject_CallsHandler()
        {
            var box = new ObservableBox() { Length = 10, Width = 10, Height = 10 };
            var changedCounts = new Dictionary<string, int>()
            {
                [nameof(box.Length)] = 0,
                [nameof(box.Width)] = 0,
                [nameof(box.Height)] = 0,
                [nameof(box.Size)] = 0
            };
            box.PropertyChanged += (sender, e) => changedCounts[e.PropertyName]++;
            Assert.AreEqual(box.Size, box.Length * box.Width * box.Height);
            box.Length *= 10; box.Width *= 10; box.Height *= 10;
            Assert.AreEqual(box.Size, box.Length * box.Width * box.Height);
            Assert.AreEqual(changedCounts[nameof(box.Length)], 1);
            Assert.AreEqual(changedCounts[nameof(box.Width)], 1);
            Assert.AreEqual(changedCounts[nameof(box.Height)], 1);
            Assert.AreEqual(changedCounts[nameof(box.Size)], 3);
        }

        [TestMethod]
        public void PropertyChanged_ObservableStorage_CallsHandler()
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
