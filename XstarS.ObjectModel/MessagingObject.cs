using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供发送消息类型 <see cref="ISendMessage"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class MessagingObject : ISendMessage
    {
        /// <summary>
        /// 初始化 <see cref="MessagingObject"/> 类的新实例。
        /// </summary>
        protected MessagingObject() { }

        /// <summary>
        /// 当发送消息时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler<MessageEventArgs> Message;

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
        /// 使用指定的事件数据引发 <see cref="MessagingObject.Message"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="MessageEventArgs"/>。</param>
        protected virtual void OnMessage(MessageEventArgs e)
        {
            this.Message?.Invoke(this, e);
        }
    }
}
