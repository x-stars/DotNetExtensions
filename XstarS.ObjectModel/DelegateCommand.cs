using System;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示由委托 <see cref="Delegate"/> 定义的无参数的命令 <see cref="ICommand"/>。
    /// </summary>
    public sealed class DelegateCommand : CommandBase, ICanExecuteToken
    {
        /// <summary>
        /// <see cref="DelegateCommand.Execute()"/> 方法的委托。
        /// </summary>
        private readonly Action ExecuteDelegate;

        /// <summary>
        /// <see cref="DelegateCommand.CanExecute()"/> 方法的委托。
        /// </summary>
        private readonly Func<bool> CanExecuteDelegate;

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand.Execute()"/> 方法的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand.CanExecute()"/> 方法的委托。</param>
        public DelegateCommand(
            Action executeDelegate = null,
            Func<bool> canExecuteDelegate = null)
        {
            this.ExecuteDelegate = executeDelegate ?? base.Execute;
            this.CanExecuteDelegate = canExecuteDelegate ?? base.CanExecute;
        }

        /// <summary>
        /// 获取或设置此命令是否可在其当前状态下执行。
        /// </summary>
        bool ICanExecuteToken.IsExecutable
        {
            get => this.IsExecutable;
            set => this.IsExecutable = value;
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        public override void Execute() => this.ExecuteDelegate.Invoke();

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute() => this.CanExecuteDelegate.Invoke();

        /// <summary>
        /// 获取更改此命令可执行状态的 <see cref="ICanExecuteToken"/>。
        /// </summary>
        /// <returns>更改此命令可执行状态的 <see cref="ICanExecuteToken"/>。</returns>
        public ICanExecuteToken GetCanExecuteToken() => (ICanExecuteToken)this;

        /// <summary>
        /// 引发 <see cref="CommandBase.CanExecuteChanged"/> 事件。
        /// </summary>
        void ICanExecuteToken.OnCanExecuteChanged() => this.OnCanExecuteChanged();
    }

    /// <summary>
    /// 表示由委托 <see cref="Delegate"/> 定义的有参数的命令 <see cref="ICommand"/>。
    /// </summary>
    /// <typeparam name="TParameter">命令使用的数据的类型。</typeparam>
    public sealed class DelegateCommand<TParameter> : CommandBase<TParameter>, ICanExecuteToken
    {
        /// <summary>
        /// <see cref="DelegateCommand{TParameter}.Execute(TParameter)"/> 方法的委托。
        /// </summary>
        private readonly Action<TParameter> ExecuteDelegate;

        /// <summary>
        /// <see cref="DelegateCommand{TParameter}.CanExecute(TParameter)"/> 方法的委托。
        /// </summary>
        private readonly Func<TParameter, bool> CanExecuteDelegate;

        /// <summary>
        /// 使用指定的委托初始化 <see cref="DelegateCommand{TParameter}"/> 类的新实例。
        /// </summary>
        /// <param name="executeDelegate">
        /// <see cref="DelegateCommand{TParameter}.Execute(TParameter)"/> 方法的委托。</param>
        /// <param name="canExecuteDelegate">
        /// <see cref="DelegateCommand{TParameter}.CanExecute(TParameter)"/> 方法的委托。</param>
        public DelegateCommand(
            Action<TParameter> executeDelegate = null,
            Func<TParameter, bool> canExecuteDelegate = null)
        {
            this.ExecuteDelegate = executeDelegate ?? base.Execute;
            this.CanExecuteDelegate = canExecuteDelegate ?? base.CanExecute;
        }

        /// <summary>
        /// 获取或设置此命令是否可在其当前状态下执行。
        /// </summary>
        bool ICanExecuteToken.IsExecutable
        {
            get => this.IsExecutable;
            set => this.IsExecutable = value;
        }

        /// <summary>
        /// 在当前状态下执行此命令。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public override void Execute(TParameter parameter) => this.ExecuteDelegate.Invoke(parameter);

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public override bool CanExecute(TParameter parameter) => this.CanExecuteDelegate.Invoke(parameter);

        /// <summary>
        /// 获取更改此命令可执行状态的 <see cref="ICanExecuteToken"/>。
        /// </summary>
        /// <returns>更改此命令可执行状态的 <see cref="ICanExecuteToken"/>。</returns>
        public ICanExecuteToken GetCanExecuteToken() => (ICanExecuteToken)this;

        /// <summary>
        /// 引发 <see cref="CommandBase{TParameter}.CanExecuteChanged"/> 事件。
        /// </summary>
        void ICanExecuteToken.OnCanExecuteChanged() => this.OnCanExecuteChanged();
    }
}
