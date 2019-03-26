using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XstarS.ComponentModel.TestTypes;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class BindingExtensionsTest
    {
        [TestMethod]
        public void PropertyChanged_CommonImplement_CallsHandler()
        {
            var i = 0;
            var o = new BindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            for (int j = 0; j < 1000; j++) { o.Value++; }
            Assert.AreEqual(i, 1000);
        }

        [TestMethod]
        public void PropertyChanged_ExplicitImplement_CallsHandler()
        {
            var i = 0;
            var o = new ExplicitBindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            for (int j = 0; j < 1000; j++) { o.Value++; }
            Assert.AreEqual(i, 1000);
        }

        [TestMethod]
        public void PropertyChanged_BaseImplement_CallsHandler()
        {
            var i = 0;
            var o = new DerivedBindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            for (int j = 0; j < 1000; j++) { o.Value++; }
            Assert.AreEqual(i, 1000);
        }
    }
}
