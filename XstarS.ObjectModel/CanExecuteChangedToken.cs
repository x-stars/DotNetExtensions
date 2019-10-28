using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 在 <see cref="Command"/> 外部引发 <see cref="Command.CanExecuteChanged"/> 事件。
    /// </summary>
    public sealed class CanExecuteChangedToken
    {
        /// <summary>
        /// 初始化 <see cref="CanExecuteChangedToken"/> 类的新实例。
        /// </summary>
        /// <param name="owner"></param>
        internal CanExecuteChangedToken(DelegateCommand owner)
        {
            if (owner is null)
            {
                throw new ArgumentNullException(nameof(owner));
            }

            this.OwnerCommand = owner;
        }

        /// <summary>
        /// 拥有当前实例的 <see cref="DelegateCommand"/> 对象。
        /// </summary>
        internal DelegateCommand OwnerCommand { get; }

        /// <summary>
        /// 引发此实例对应的 <see cref="Command.CanExecuteChanged"/> 事件。
        /// </summary>
        public void OnCanExecuteChanged()
        {
            this.OwnerCommand.OnCanExecuteChangedInternal();
        }
    }
}
