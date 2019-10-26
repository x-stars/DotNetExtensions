using System;
using System.Windows.Input;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个由委托 <see cref="Delegate"/> 定义的无参数的命令。
    /// </summary>
    public class DelegateCommand : ICommand
    {
        /// <summary>
        /// 默认的 <see cref="DelegateCommand.CanExecuteDelegate"/>，
        /// 总是返回 <see langword="true"/>。
        /// </summary>
        public static readonly Func<bool> DefaultCanExecuteDelegate =
            DelegateCommand.DefaultCanExecute;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand.ExecuteDelegate"/>，不做任何事。
        /// </summary>
        public static readonly Action DefaultExecuteDelegate =
            DelegateCommand.DefaultExecute;

        /// <summary>
        /// <see cref="DelegateCommand.CanExecuteDelegate"/> 的值。
        /// </summary>
        private Func<bool> InternalCanExecuteDelegate;

        /// <summary>
        /// <see cref="DelegateCommand.ExecuteDelegate"/> 的值。
        /// </summary>
        private Action InternalExecuteDelegate;

        /// <summary>
        /// 初始化 <see cref="DelegateCommand"/> 类的新实例。
        /// </summary>
        public DelegateCommand()
        {
            this.CanExecuteDelegate = DelegateCommand.DefaultCanExecuteDelegate;
            this.ExecuteDelegate = DelegateCommand.DefaultExecuteDelegate;
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行的方法的 <see cref="Func{T}"/> 委托。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <see langword="value"/> 为 <see langword="null"/>。</exception>
        public Func<bool> CanExecuteDelegate
        {
            get => this.InternalCanExecuteDelegate;
            set => this.InternalCanExecuteDelegate =
                value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 在调用此命令时要调用的方法的 <see cref="Action"/> 委托。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <see langword="value"/> 为 <see langword="null"/>。</exception>
        public Action ExecuteDelegate
        {
            get => this.InternalExecuteDelegate;
            set => this.InternalExecuteDelegate =
                value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand.CanExecute()"/>，
        /// 总是返回 <see langword="true"/>。
        /// </summary>
        /// <returns>总是返回 <see langword="true"/>。</returns>
        private static bool DefaultCanExecute() => true;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand.Execute()"/>，不做任何事。
        /// </summary>
        private static void DefaultExecute() { }

        /// <summary>
        /// 调用 <see cref="DelegateCommand.CanExecuteDelegate"/> 委托，
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public bool CanExecute() => this.CanExecuteDelegate.Invoke();

        /// <summary>
        /// 调用 <see cref="DelegateCommand.ExecuteDelegate"/> 委托，执行此命令的目标。
        /// </summary>
        public void Execute() => this.ExecuteDelegate.Invoke();

        /// <summary>
        /// 引发 <see cref="ICommand.CanExecuteChanged"/> 事件。
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 调用 <see cref="DelegateCommand.CanExecuteDelegate"/> 委托，
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据，将总是被忽略。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        bool ICommand.CanExecute(object parameter) => this.CanExecute();

        /// <summary>
        /// 调用 <see cref="DelegateCommand.ExecuteDelegate"/> 委托，执行此命令的目标。
        /// </summary>
        /// <param name="parameter">此命令使用的数据，将总是被忽略。</param>
        void ICommand.Execute(object parameter) => this.Execute();
    }

    /// <summary>
    /// 表示一个由委托 <see cref="Delegate"/> 定义的有参数的命令。
    /// </summary>
    /// <typeparam name="TParameter">命令参数的类型。</typeparam>
    public class DelegateCommand<TParameter> : ICommand
    {
        /// <summary>
        /// 默认的 <see cref="DelegateCommand{TParameter}.CanExecuteDelegate"/>，
        /// 总是返回 <see langword="true"/>。
        /// </summary>
        public static readonly Predicate<TParameter> DefaultCanExecuteDelegate =
            DelegateCommand<TParameter>.DefaultCanExecute;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand{TParameter}.ExecuteDelegate"/>，不做任何事。
        /// </summary>
        public static readonly Action<TParameter> DefaultExecuteDelegate =
            DelegateCommand<TParameter>.DefaultExecute;

        /// <summary>
        /// <see cref="DelegateCommand{TParameter}.CanExecuteDelegate"/> 的值。
        /// </summary>
        private Predicate<TParameter> InternalCanExecuteDelegate;

        /// <summary>
        /// <see cref="DelegateCommand{TParameter}.ExecuteDelegate"/> 的值。
        /// </summary>
        private Action<TParameter> InternalExecuteDelegate;

        /// <summary>
        /// 初始化 <see cref="DelegateCommand{TParameter}"/> 类的新实例。
        /// </summary>
        public DelegateCommand()
        {
            this.CanExecuteDelegate = DelegateCommand<TParameter>.DefaultCanExecuteDelegate;
            this.ExecuteDelegate = DelegateCommand<TParameter>.DefaultExecuteDelegate;
        }

        /// <summary>
        /// 确定此命令是否可在其当前状态下执行的方法的 <see cref="Predicate{T}"/> 委托。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <see langword="value"/> 为 <see langword="null"/>。</exception>
        public Predicate<TParameter> CanExecuteDelegate
        {
            get => this.InternalCanExecuteDelegate;
            set => this.InternalCanExecuteDelegate =
                value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 在调用此命令时要调用的方法的 <see cref="Action{T}"/> 委托。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <see langword="value"/> 为 <see langword="null"/>。</exception>
        public Action<TParameter> ExecuteDelegate
        {
            get => this.InternalExecuteDelegate;
            set => this.InternalExecuteDelegate =
                value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 当出现影响是否应执行该命令的更改时发生。
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand{TParameter}.CanExecute(TParameter)"/>，
        /// 总是返回 <see langword="true"/>。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>总是返回 <see langword="true"/>。</returns>
        private static bool DefaultCanExecute(TParameter parameter) => true;

        /// <summary>
        /// 默认的 <see cref="DelegateCommand{TParameter}.Execute(TParameter)"/>，不做任何事。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        private static void DefaultExecute(TParameter parameter) { }

        /// <summary>
        /// 调用 <see cref="DelegateCommand{TParameter}.CanExecuteDelegate"/> 委托，
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public bool CanExecute(TParameter parameter) => this.CanExecuteDelegate.Invoke(parameter);

        /// <summary>
        /// 调用 <see cref="DelegateCommand{TParameter}.ExecuteDelegate"/> 委托，执行此命令的目标。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        public void Execute(TParameter parameter) => this.ExecuteDelegate.Invoke(parameter);

        /// <summary>
        /// 引发 <see cref="ICommand.CanExecuteChanged"/> 事件。
        /// </summary>
        public virtual void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 调用 <see cref="DelegateCommand{TParameter}.CanExecuteDelegate"/> 委托，
        /// 确定此命令是否可在其当前状态下执行。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <returns>如果可执行此命令，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="parameter"/> 为 <see langword="null"/>。</exception>
        bool ICommand.CanExecute(object parameter) => this.CanExecute((TParameter)parameter);

        /// <summary>
        /// 调用 <see cref="DelegateCommand{TParameter}.ExecuteDelegate"/> 委托，执行此命令的目标。
        /// </summary>
        /// <param name="parameter">此命令使用的数据。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="parameter"/> 为 <see langword="null"/>。</exception>
        void ICommand.Execute(object parameter) => this.Execute((TParameter)parameter);
    }
}
