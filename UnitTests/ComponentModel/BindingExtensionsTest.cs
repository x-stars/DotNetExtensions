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
        public void PropertyChanged_Common_CallsHandler()
        {
            var i = 0;
            var o = new BindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            o.Value++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_Explicit_CallsHandler()
        {
            var i = 0;
            var o = new ExplicitBindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            o.Value++;
            Assert.AreEqual(i, 1);
        }

        [TestMethod]
        public void PropertyChanged_Derived_CallsHandler()
        {
            var i = 0;
            var o = new DerivedBindingValue<int>();
            ((INotifyPropertyChanged)o).PropertyChanged += (sender, e) => i++;
            o.Value++;
            Assert.AreEqual(i, 1);
        }
    }
}
