using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为发送消息事件 <see cref="ISendMessage.Message"/> 提供数据。
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        /// <summary>
        /// 使用消息内容初始化 <see cref="MessageEventArgs"/> 类的新实例。
        /// </summary>
        /// <param name="message">消息的内容。</param>
        public MessageEventArgs(string message)
        {
            this.Message = message;
            this.Level = MessageLevel.Information;
        }

        /// <summary>
        /// 使用消息内容和等级初始化 <see cref="MessageEventArgs"/> 类的新实例。
        /// </summary>
        /// <param name="message">消息的内容。</param>
        /// <param name="level">消息的等级。</param>
        public MessageEventArgs(string message, MessageLevel level)
        {
            this.Message = message;
            this.Level = level;
        }

        /// <summary>
        /// 消息的内容。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 消息的等级。
        /// </summary>
        public MessageLevel Level { get; }
    }
}
