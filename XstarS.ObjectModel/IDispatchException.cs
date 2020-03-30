using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 将捕获到的异常转发到客户端。
    /// </summary>
    public interface IDispatchException
    {
        /// <summary>
        /// 当捕获到异常时发生。
        /// </summary>
        event EventHandler<ExceptionEventArgs> Exception;
    }
}
