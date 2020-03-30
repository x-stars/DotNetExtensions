using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供转发异常类型 <see cref="IDispatchException"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ExceptioningObject : IDispatchException
    {
        /// <summary>
        /// 初始化 <see cref="ExceptioningObject"/> 类的新实例。
        /// </summary>
        protected ExceptioningObject() { }

        /// <summary>
        /// 当捕获到异常时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<ExceptionEventArgs> Exception;

        /// <summary>
        /// 转发捕获到的异常。
        /// </summary>
        /// <param name="exception">捕获到的异常。</param>
        protected void DispatchException(Exception exception)
        {
            this.OnException(new ExceptionEventArgs(exception));
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ExceptioningObject.Exception"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ExceptionEventArgs"/>。</param>
        protected virtual void OnException(ExceptionEventArgs e)
        {
            this.Exception?.Invoke(this, e);
        }
    }
}
