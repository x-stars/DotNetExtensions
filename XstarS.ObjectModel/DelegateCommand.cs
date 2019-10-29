using System;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个由委托 <see cref="Delegate"/> 定义的命令 <see cref="ICommand"/>。
    /// </summary>
    internal sealed class DelegateCommand : Command
    {
        /// <summary>
        /// 在调用此命令时要调用的方法的委托。
        /// </summary>
        private readonly Action<object> ExecuteDelegate;

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行的方法的委托。
        /// </summary>
        private readonly Predicate<object> CanExecuteDelegate;

        /// <summary>
        /// 以指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">在调用此命令时要调用的方法的委托。</param>
        /// <param name="canExecuteDelegate">确定此命令是否可在其当前状态下执行的方法的委托。</param>
        /// <param name="canExecuteChangedToken">
        /// 引发此命令的 <see cref="Command.CanExecuteChanged"/> 事件的对象。</param>
        internal DelegateCommand(
            Action<object> executeDelegate, Predicate<object> canExecuteDelegate,
            out CanExecuteChangedToken canExecuteChangedToken)
        {
            this.ExecuteDelegate = executeDelegate ?? (_ => { });
            this.CanExecuteDelegate = canExecuteDelegate ?? (_ => true);
            this.StateChangedToken = new CanExecuteChangedToken(this);
            canExecuteChangedToken = this.StateChangedToken;
        }

        /// <summary>
        /// 引发此命令的 <see cref="Command.CanExecuteChanged"/> 事件的对象。
        /// </summary>
        internal CanExecuteChangedToken StateChangedToken { get; }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。
        /// 如果此命令不需要传递数据，则该对象可以设置为 <see langword="null"/>。</param>
        public override void Execute(object parameter)
        {
            this.ExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。
        /// 如果此命令不需要传递数据，则该对象可以设置为 <see langword="null"/>。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute(object parameter)
        {
            return this.CanExecuteDelegate.Invoke(parameter);
        }

        /// <summary>
        /// 引发 <see cref="Command.CanExecuteChanged"/> 事件。
        /// 用于 <see cref="CanExecuteChangedToken"/> 的内部实现。
        /// </summary>
        internal void OnCanExecuteChangedInternal()
        {
            this.OnCanExecuteChanged();
        }
    }
}
