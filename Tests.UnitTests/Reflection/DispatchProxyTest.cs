using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XNetEx.Collections.Generic;

namespace XNetEx.Reflection;

[TestClass]
public class DispatchProxyTest
{
    [TestMethod]
    public void Create_WithDefaultHandler_GetProxyInstance()
    {
        var list = new Collection<object>() { new object() };
        var handler = DispatchProxyServices.DefaultHandler;
        var pList = DispatchProxy<IList<object>>.Create(list, handler);
        Assert.IsNotNull(pList);
        Assert.IsInstanceOfType(pList, typeof(DispatchProxy<IList<object>>));
        Assert.AreEqual(1, pList.Count);
    }

    [TestMethod]
    public void Create_WithCallCountHandler_GetExpectedCount()
    {
        var list = new Collection<object?>() { new object() };
        var callCounts = new Dictionary<MethodInfo, int>();
        var handler = (InvocationHandler)((instance, method, arguments) =>
        {
            callCounts[method] = callCounts.GetOrAdd(method, 0) + 1;
            return method.InvokeFast(instance, arguments);
        });
        var pList = DispatchProxy<IList<object?>>.Create(list, handler);
        Assert.AreEqual(0, callCounts.Count);
        pList.Add(new object());
        Assert.AreEqual(1, callCounts.Count);
        Assert.AreEqual(2, pList.Count);
        Assert.AreEqual(2, callCounts.Count);
        pList[0] = null;
        Assert.AreEqual(3, callCounts.Count);
        Assert.IsNull(pList[0]);
        Assert.AreEqual(4, callCounts.Count);
    }
}
