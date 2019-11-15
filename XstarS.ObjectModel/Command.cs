using System;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供命令 <see cref="ICommand"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class Command : ICommand
    {
        /// <summary>
        /// 初始化 <see cref="Command"/> 类的新实例。
        /// </summary>
        protected Command() { }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 创建一个 <see cref="Command"/> 类的实例，
        /// 其 <see cref="Command.Execute(object)"/> 方法和
        /// <see cref="Command.CanExecute(object)"/> 方法由指定的委托定义。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="Command.Execute(object)"/> 方法的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="Command.CanExecute(object)"/> 方法的委托。</param>
        /// <param name="canExecuteChangedToken">
        /// 引发此命令的 <see cref="Command.CanExecuteChanged"/> 事件的对象。</param>
        /// <returns>以指定的委托定义的 <see cref="Command"/> 类的实例。</returns>
        public static Command Create(
            Action<object> executeDelegate, Predicate<object> canExecuteDelegate,
            out CanExecuteChangedToken canExecuteChangedToken)
        {
            return new DelegateCommand(
                executeDelegate, canExecuteDelegate, out canExecuteChangedToken);
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。
        /// 如果此命令不需要传递数据，则该对象可以设置为 <see langword="null"/>。</param>
        public abstract void Execute(object parameter);

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。
        /// 如果此命令不需要传递数据，则该对象可以设置为 <see langword="null"/>。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public abstract bool CanExecute(object parameter);

        /// <summary>
        /// 引发 <see cref="Command.CanExecuteChanged"/> 事件。
        /// </summary>
        protected virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
