using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace XNetEx.Reflection;

[TestClass]
public class MemberDelegateExtensionsTest
{
    [TestMethod]
    public void CreateDelegate_PublicConstructor_CanCreateInstance()
    {
        var constructor = typeof(StrongBox<object>).GetConstructor(new[] { typeof(object) })!;
        var createDelegate = constructor.CreateDelegate<Func<object, StrongBox<object>>>();
        var value = new object();
        var result = createDelegate.Invoke(value);
        Assert.AreSame(value, result.Value);
    }

    [TestMethod]
    public void CreateGetDelegate_PublicField_CanGetValue()
    {
        var box = new StrongBox<object>(new object());
        var field = box.GetType().GetField(nameof(box.Value))!;
        var getDelegate = field.CreateGetDelegate<Func<StrongBox<object>, object>>();
        var result = getDelegate.Invoke(box);
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateGetDelegate_PublicFieldWithInstance_CanGetValue()
    {
        var box = new StrongBox<object>(new object());
        var field = box.GetType().GetField(nameof(box.Value))!;
        var getDelegate = field.CreateGetDelegate<Func<object>>(box);
        var result = getDelegate.Invoke();
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateSetDelegate_PublicField_CanSetValue()
    {
        var box = new StrongBox<object>();
        var field = box.GetType().GetField(nameof(box.Value))!;
        var setDelegate = field.CreateSetDelegate<Action<StrongBox<object>, object>>();
        var value = new object();
        setDelegate.Invoke(box, value);
        Assert.AreSame(value, box.Value);
    }

    [TestMethod]
    public void CreateSetDelegate_PublicFieldWithInstance_CanSetValue()
    {
        var box = new StrongBox<object>();
        var field = box.GetType().GetField(nameof(box.Value))!;
        var setDelegate = field.CreateSetDelegate<Action<object>>(box);
        var value = new object();
        setDelegate.Invoke(value);
        Assert.AreSame(value, box.Value);
    }

    [TestMethod]
    public void CreateDynamicDelegate_PublicConstructor_CanCreateInstance()
    {
        var constructor = typeof(StrongBox<object>).GetConstructor(new[] { typeof(object) })!;
        var createDelegate = constructor.CreateDynamicDelegate();
        var value = new object();
        var result = (StrongBox<object>)createDelegate.Invoke(new[] { value });
        Assert.AreSame(value, result.Value);
    }

    [TestMethod]
    public void CreateDynamicGetDelegate_PublicField_CanGetValue()
    {
        var box = new StrongBox<object>(new object());
        var field = box.GetType().GetField(nameof(box.Value))!;
        var getDelegate = field.CreateDynamicGetDelegate();
        var result = getDelegate.Invoke(box);
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateDynamicGetDelegate_PublicFieldWithInstance_CanGetValue()
    {
        var box = new StrongBox<object>(new object());
        var field = box.GetType().GetField(nameof(box.Value))!;
        var getDelegate = field.CreateDynamicGetDelegate(box);
        var result = getDelegate.Invoke();
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateDynamicSetDelegate_PublicField_CanSetValue()
    {
        var box = new StrongBox<object>();
        var field = box.GetType().GetField(nameof(box.Value))!;
        var setDelegate = field.CreateDynamicSetDelegate();
        var value = new object();
        setDelegate.Invoke(box, value);
        Assert.AreSame(value, box.Value);
    }

    [TestMethod]
    public void CreateDynamicSetDelegate_PublicFieldWithInstance_CanSetValue()
    {
        var box = new StrongBox<object>();
        var field = box.GetType().GetField(nameof(box.Value))!;
        var setDelegate = field.CreateDynamicSetDelegate(box);
        var value = new object();
        setDelegate.Invoke(value);
        Assert.AreSame(value, box.Value);
    }

    [TestMethod]
    public void CreateDynamicDelegate_PublicMethod_CanInvoke()
    {
        var box = new StrongBox<object>(new object());
        var method = typeof(IStrongBox).GetProperty(nameof(box.Value))!.GetMethod!;
        var invokeDelegate = method.CreateDynamicDelegate();
        var result = invokeDelegate.Invoke(box, Array.Empty<object>());
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateDynamicDelegate_PublicMethodWithInstance_CanInvoke()
    {
        var box = new StrongBox<object>(new object());
        var method = typeof(IStrongBox).GetProperty(nameof(box.Value))!.GetMethod!;
        var invokeDelegate = method.CreateDynamicDelegate(box);
        var result = invokeDelegate.Invoke(Array.Empty<object>());
        Assert.AreSame(box.Value, result);
    }

    [TestMethod]
    public void CreateDynamicDelegate_PublicVoidMethod_CanInvoke()
    {
        var box = new StrongBox<object>();
        var method = typeof(IStrongBox).GetProperty(nameof(box.Value))!.SetMethod!;
        var invokeDelegate = method.CreateDynamicDelegate();
        var value = new object();
        invokeDelegate.Invoke(box, new[] { value });
        Assert.AreSame(value, box.Value);
    }

    [TestMethod]
    public void CreateDynamicDelegate_PublicVoidMethodWithInstance_CanInvoke()
    {
        var box = new StrongBox<object>();
        var method = typeof(IStrongBox).GetProperty(nameof(box.Value))!.SetMethod!;
        var invokeDelegate = method.CreateDynamicDelegate(box);
        var value = new object();
        invokeDelegate.Invoke(new[] { value });
        Assert.AreSame(value, box.Value);
    }
}
