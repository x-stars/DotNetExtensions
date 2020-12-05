using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Diagnostics;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供将 <see cref="Pointer"/> 包装的指针表示为字符串的方法。
    /// </summary>
    [Serializable]
    internal unsafe sealed class PointerBoxRepresenter : ObjectRepresenter<Pointer>
    {
        /// <summary>
        /// 初始化 <see cref="PointerBoxRepresenter"/> 类的新实例。
        /// </summary>
        public PointerBoxRepresenter() { }

        /// <summary>
        /// 将指定的 <see cref="Pointer"/> 包装的指针表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的 <see cref="Pointer"/> 包装的指针。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 包装的指针的字符串。</returns>
        protected override string RepresentCore(Pointer value, ISet<object> represented)
        {
            return ((IntPtr)Pointer.Unbox(value)).ToString();
        }
    }
}
