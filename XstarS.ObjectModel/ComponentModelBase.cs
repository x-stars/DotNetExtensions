using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供组件模型的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class ComponentModelBase
        : ObservableDataObject, IDispatchException, ISendMessage
    {
        /// <summary>
        /// 初始化 <see cref="ComponentModelBase"/> 类的新实例。
        /// </summary>
        protected ComponentModelBase() { }

        /// <summary>
        /// 当捕获到异常时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<ExceptionEventArgs> Exception;

        /// <summary>
        /// 当发送消息时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<MessageEventArgs> Message;

        /// <summary>
        /// 转发捕获到的异常。
        /// </summary>
        /// <param name="exception">捕获到的异常。</param>
        protected void DispatchException(Exception exception)
        {
            this.OnException(new ExceptionEventArgs(exception));
        }

        /// <summary>
        /// 发送指定内容的消息。
        /// </summary>
        /// <param name="message">消息的内容。</param>
        protected void SendMessage(string message)
        {
            this.OnMessage(new MessageEventArgs(message));
        }

        /// <summary>
        /// 发送指定内容和等级的消息。
        /// </summary>
        /// <param name="message">消息的内容。</param>
        /// <param name="level">消息的等级。</param>
        protected void SendMessage(string message, MessageLevel level)
        {
            this.OnMessage(new MessageEventArgs(message, level));
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ComponentModelBase.Exception"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="ExceptionEventArgs"/>。</param>
        protected virtual void OnException(ExceptionEventArgs e)
        {
            this.Exception?.Invoke(this, e);
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="ComponentModelBase.Message"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="MessageEventArgs"/>。</param>
        protected virtual void OnMessage(MessageEventArgs e)
        {
            this.Message?.Invoke(this, e);
        }
    }
}
