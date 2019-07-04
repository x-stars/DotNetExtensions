using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XstarS.ComponentModel
{
    [TestClass]
    public class ValueBindingTest
    {
        [TestMethod]
        public void NoBinding_Int32Bindable_NotSameValue()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new Bindable<int>(0);
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(source.Value, i);
                Assert.AreEqual(target.Value, 0);
                source.Value++;
            }
        }

        [TestMethod]
        public void TwoWayBinding_Int32Bindable_SameValue()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new Bindable<int>(0);
            var sourceValue = new BindingProperty<int>(source, nameof(source.Value));
            var targetValue = new BindingProperty<int>(target, nameof(target.Value));
            var binding = new ValueBinding<int>(sourceValue, targetValue, BindingDirection.TwoWay);
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, source.Value);
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
            source.Value = 0;
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, source.Value);
                Assert.AreEqual(source.Value, i);
                target.Value++;
            }
        }

        [TestMethod]
        public void OneWayBinding_Int32Bindable_SameValueOnSourceChange()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new Bindable<int>(0);
            var sourceValue = new BindingProperty<int>(source, nameof(source.Value));
            var targetValue = new BindingProperty<int>(target, nameof(target.Value));
            var binding = new ValueBinding<int>(sourceValue, targetValue, BindingDirection.OneWay);
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, source.Value);
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
            source.Value = 0;
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, i);
                Assert.AreEqual(source.Value, 0);
                target.Value++;
            }
        }

        [TestMethod]
        public void OneWayBindingAndDispose_Int32Bindable_SameValueOnNotDisposed()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new Bindable<int>(0);
            var sourceValue = new BindingProperty<int>(source, nameof(source.Value));
            var targetValue = new BindingProperty<int>(target, nameof(target.Value));
            var binding = new ValueBinding<int>(sourceValue, targetValue, BindingDirection.TwoWay);
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, source.Value);
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
            source.Value = 0;
            binding.Dispose();
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value, 0);
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
        }

        [TestMethod]
        public void OneWayBinding_Int32BindableAndInt32Array_SameValueAtIndex()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new int[10];
            var targetIndices = new HashSet<int>() { 0, 2, 4, 6, 8 };
            foreach (var index in targetIndices)
            {
                var sourceValue = new BindingProperty<int>(source, nameof(source.Value));
                var targetValue = new BindingIndex<int>(target, index);
                var binding = new ValueBinding<int>(sourceValue, targetValue, BindingDirection.OneWay);
            }
            for (int i = 0; i < testCount; i++)
            {
                for (int j = 0; j < target.Length; j++)
                {
                    if (targetIndices.Contains(j))
                    {
                        Assert.AreEqual(target[j], source.Value);
                    }
                    else
                    {
                        Assert.AreEqual(target[j], 0);
                    }
                }
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
        }

        [TestMethod]
        public void OneWayBinding_Int32BindableAndInt32List_SameValueAtIndex()
        {
            int testCount = 10000;
            var source = new Bindable<int>(0);
            var target = new List<int>(new int[10]);
            var targetIndices = new HashSet<int>() { 0, 2, 4, 6, 8 };
            foreach (var index in targetIndices)
            {
                var sourceValue = new BindingProperty<int>(source, nameof(source.Value));
                var targetValue = new BindingIndex<int>(target, index);
                var binding = new ValueBinding<int>(sourceValue, targetValue, BindingDirection.OneWay);
            }
            for (int i = 0; i < testCount; i++)
            {
                for (int j = 0; j < target.Count; j++)
                {
                    if (targetIndices.Contains(j))
                    {
                        Assert.AreEqual(target[j], source.Value);
                    }
                    else
                    {
                        Assert.AreEqual(target[j], 0);
                    }
                }
                Assert.AreEqual(source.Value, i);
                source.Value++;
            }
        }

        [TestMethod]
        public void TwoWayBinding_Int32BindableAndStringBindable_SameToStringValue()
        {
            int testCount = 10000;
            var source = new Bindable<string>("0");
            var target = new Bindable<int>(0);
            var sourceValue = new BindingProperty<string>(source, nameof(source.Value));
            var targetValue = new BindingProperty<int>(target, nameof(target.Value));
            var binding = new ValueBinding<string, int>(
                sourceValue, targetValue, BindingDirection.TwoWay,
                s => int.Parse(s), i => i.ToString());
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value.ToString(), source.Value);
                Assert.AreEqual(source.Value, i.ToString());
                source.Value = (int.Parse(source.Value) + 1).ToString();
            }
            source.Value = "0";
            for (int i = 0; i < testCount; i++)
            {
                Assert.AreEqual(target.Value.ToString(), source.Value);
                Assert.AreEqual(source.Value, i.ToString());
                target.Value++;
            }
        }
    }
}
