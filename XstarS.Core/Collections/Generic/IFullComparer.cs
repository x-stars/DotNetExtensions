using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 定义比较指定类型的对象是否相等和大小关系的方法。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    public interface IFullComparer<in T> : IComparer<T>, IEqualityComparer<T>
    {
    }
}
