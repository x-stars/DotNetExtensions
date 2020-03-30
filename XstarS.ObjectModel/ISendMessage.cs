using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 向客户端发送消息。
    /// </summary>
    public interface ISendMessage
    {
        /// <summary>
        /// 当发送消息时发生。
        /// </summary>
        event EventHandler<MessageEventArgs> Message;
    }
}
