using System;

namespace XstarS
{
    /// <summary>
    /// 提供抛出特定异常的方法。
    /// </summary>
    /// <typeparam name="TException">要抛出的异常的类型。</typeparam>
    public static class Throw<TException>
        where TException : Exception, new()
    {
        /// <summary>
        /// 在满足指定条件时抛出特定异常。
        /// </summary>
        /// <param name="condition">抛出异常应满足的条件。</param>
        /// <exception cref="Exception">
        /// <paramref name="condition"/> 为 <see langword="true"/>。</exception>
        public static void When(bool condition)
        {
            if (condition)
            {
                throw new TException();
            }
        }
    }
}
