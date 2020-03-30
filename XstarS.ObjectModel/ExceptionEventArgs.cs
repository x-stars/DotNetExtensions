using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为 <see cref="IDispatchException.Exception"/> 事件提供数据。
    /// </summary>
    public class ExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// 以捕获到的异常初始化 <see cref="ExceptionEventArgs"/> 类的新实例。
        /// </summary>
        /// <param name="exception">捕获到的异常。</param>
        public ExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
            this.IsHandled = false;
        }

        /// <summary>
        /// 获取捕获到的异常。
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// 获取或设置当前异常是否已经被处理。
        /// </summary>
        public bool IsHandled { get; set; }
    }
}
